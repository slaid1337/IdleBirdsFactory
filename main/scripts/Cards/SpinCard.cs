using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpinCard : BasicCard
{
    [SerializeField] private AnimationCurve _scrollCurve;
    [SerializeField] private GameObject _scroller;
    private float _scrollStep;
    private bool _isSpin;
    [SerializeField] private GameObject[] _birdsCells;
    [SerializeField] private Color[] _colors;
    private int _index;
    private Cell _cell;
    [SerializeField] private Button _spinBtn;

    private void Awake()
    {
        _isSpin = false;
    }

    private void Update()
    {
        OpenningCard();
        if (_isSpin)
        {
            Spin();
        }
    }

    private void Spin()
    {
        _scrollStep += Time.deltaTime * 200;

        if (_scrollStep >= 2000f)
        {
            _isSpin = false;
            return;
        }

        _scroller.GetComponent<RectTransform>().anchoredPosition = new Vector3(_scrollCurve.Evaluate(_scrollStep), 0, 0);
    }

    public void SetSpinning()
    {
        _isSpin = true;
        StartCoroutine(OpenCollectCardAfterSpin());
    }

    public void SelfOpen(int birdIndex , Cell cell)
    {
        foreach (var item in _birdsCells)
        {
            int randomBird = Random.Range(0, _gameController.BirdsObjects.Length);
            item.transform.GetChild(1).GetComponent<Image>().sprite = _gameController.BirdsObjects[randomBird].Image;
            item.transform.GetChild(0).GetComponent<Image>().color = _colors[GetColorIndexFromBonus(_gameController.BirdsObjects[randomBird].Bonus)];
        }

        _birdsCells[77].transform.GetChild(1).GetComponent<Image>().sprite = _gameController._availableBirds[birdIndex].Image;
        _birdsCells[77].transform.GetChild(0).GetComponent<Image>().color = _colors[GetColorIndexFromBonus(_gameController._availableBirds[birdIndex].Bonus)];
        _index = birdIndex;
        _cell = cell;
    }

    public void SelfOpen()
    {
        _spinBtn.enabled = true;
        int birdIndex = Random.Range(0, _gameController._availableBirds.Count - 1);

        _cell.BirdIndex = birdIndex;

        foreach (var item in _birdsCells)
        {
            int randomBird = Random.Range(0, _gameController.BirdsObjects.Length);
            item.transform.GetChild(1).GetComponent<Image>().sprite = _gameController.BirdsObjects[randomBird].Image;
            item.transform.GetChild(0).GetComponent<Image>().color = _colors[GetColorIndexFromBonus(_gameController.BirdsObjects[randomBird].Bonus)];
        }

        _birdsCells[77].transform.GetChild(1).GetComponent<Image>().sprite = _gameController._availableBirds[birdIndex].Image;
        _birdsCells[77].transform.GetChild(0).GetComponent<Image>().color = _colors[GetColorIndexFromBonus(_gameController._availableBirds[birdIndex].Bonus)];
        _index = birdIndex;
    }

    private int GetColorIndexFromBonus(float bonus)
    {
        switch (bonus)
        {
            case 1f:
                return 0;
            case 1.25f:
                return 1;
            case 1.5f:
                return 2;
            case 1.75f:
                return 3;
            case 2f:
                return 4;
            default:
                return 0;
        }
    }

    private IEnumerator OpenCollectCardAfterSpin()
    {
        yield return new WaitForSeconds(9f);

        _scrollStep = 0f;
        _isSpin = false;

        CollectCard collectCard = _gameController.CollectCard.GetComponent<CollectCard>();

        collectCard.gameObject.SetActive(true);
        collectCard.OpenCard();
        collectCard.SelfOpen(_index, _cell);

        gameObject.SetActive(false);
    }
}