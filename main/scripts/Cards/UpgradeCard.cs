using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCard : BasicCard
{
    private float _progressStep;
    [SerializeField] GameObject _progressBar;
    
    private bool _isUpgradeble;
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private Image _birdImage;
    [SerializeField] private Text _birdCountText;
    [SerializeField] private Text _currentProfitText;
    [SerializeField] private Text _nextProfitText;
    [SerializeField] private TextMeshProUGUI _buttonText;
    private Vector2 _startSizeBar;

    private Cell _cell;

    private void Update()
    {
        OpenningCard();
    }

    private void Awake()
    {
        _startSizeBar = _progressBar.GetComponent<RectTransform>().sizeDelta;
    }

    public void SelfOpen(Cell cell)
    {
        _cell = cell;

        Refresh();

        _gameController.OnChangeMoney.AddListener(CheckOnUpgradeCard);

        _upgradeButton.GetComponent<Button>().onClick.AddListener(_cell.Upgrade);
    }

    public void CloseCard()
    {
        _upgradeButton.GetComponent<Button>().onClick.RemoveListener(_cell.Upgrade);
        _gameController.OnChangeMoney.RemoveListener(CheckOnUpgradeCard);
    }

    double Normalize(double val, double valmin, double valmax, double min, double max)
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }

    private void CheckOnUpgradeCard(System.Numerics.BigInteger money)
    {
        
        _isUpgradeble = false;
        if (money - _cell.UpgradeCost > new System.Numerics.BigInteger(0))
        {
            _isUpgradeble = true;
        }

        _upgradeButton.GetComponent<Button>().enabled = _isUpgradeble;
    }

    public void Refresh()
    {
        _birdImage.sprite = _cell._BirdObject.Image;
        _birdCountText.text = _cell.BirdCount.ToString();
        _currentProfitText.text = _cell.ShowMoney(_cell.MoneyPerSecond * _cell.BirdCount);
        _nextProfitText.text = "+" + _cell.ShowMoney(_cell.MoneyPerSecond);
        _buttonText.text = "<sprite=0>" + _gameController.ShowMoney(_cell.UpgradeCost);

        _progressStep = Mathf.Min((float)Normalize(_cell.BirdCount, 0, 18, 0, 1), 1f);
        _progressBar.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, _startSizeBar.y), _startSizeBar, _progressStep);
    }

    public void OnMaxLvl()
    {
        //_gameController.CollectEffect.GetComponent<EffectController>().OpenItem();
    }
}
