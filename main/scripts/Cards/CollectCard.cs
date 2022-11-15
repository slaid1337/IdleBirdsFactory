using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectCard : BasicCard
{
    [SerializeField] private Image _birdImage;

    private float _animationTimer;
    [SerializeField] private float _animationSpeed;
    private bool _animationSwitcher;

    [SerializeField] private Button _respinBtn;

    private Sprite _image;
    private Sprite _image2;
    private Sprite _image3;

    private Cell _cell;

    private AdmobAds _ads = null;

    private void Update()
    {
        OpenningCard();

        _animationTimer += Time.deltaTime;
        if (_animationTimer <= _animationSpeed && _animationSwitcher)
        {
            _birdImage.sprite = _image;
            _animationSwitcher = false;
        }
        else if (_animationTimer >= _animationSpeed && _animationTimer <= _animationSpeed * 2 && !_animationSwitcher)
        {
            _birdImage.sprite = _image2;
            _animationSwitcher = true;
        }
        else if (_animationTimer >= _animationSpeed * 2 && _animationTimer <= _animationSpeed * 3 && _animationSwitcher)
        {
            _birdImage.sprite = _image3;
            _animationSwitcher = false;
        }
        else if (_animationTimer >= _animationSpeed * 3 && _animationTimer <= _animationSpeed * 4 && !_animationSwitcher)
        {
            _birdImage.sprite = _image2;
            _animationTimer = 0;
            _animationSwitcher = true;
        }
    }

    public void SelfOpen(int birdIndex , Cell cell)
    {
        _ads = AdmobAds.Instance;
        _image = _gameController._availableBirds[birdIndex].Image;
        _image2 = _gameController._availableBirds[birdIndex].Image2;
        _image3 = _gameController._availableBirds[birdIndex].Image3;
        _cell = cell;

        SpinCard spinCard = _gameController.SpinCard.GetComponent<SpinCard>();
        if (_ads._rewardedRespinRoll.IsLoaded())
        {
            _respinBtn.interactable = true;
            _respinBtn.onClick.AddListener(_ads.ShowRewardedRespinRoll);
        }
        else
        {
            _respinBtn.interactable = false;
            _respinBtn.onClick.RemoveListener(_ads.ShowRewardedRespinRoll);
        }
    }

    public void Collect()
    {
        _cell.CollectBird();
        gameObject.SetActive(false);
    }

    public void Respin()
    {
        _gameController.SpinCard.SetActive(true);
        _gameController.SpinCard.GetComponent<SpinCard>().OpenCard();
        _gameController.SpinCard.GetComponent<SpinCard>().SelfOpen();
        _respinBtn.onClick.RemoveListener(_ads.ShowRewardedRespinRoll);
        gameObject.SetActive(false);
    }
}
