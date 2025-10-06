using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellerCard : BasicCard
{
    [SerializeField] private Text _currentProfitText;
    [SerializeField] private Text _riseProfitText;
    [SerializeField] private Text _currentSpeedText;
    [SerializeField] private Text _riseSpeedText;
    [SerializeField] private Seller _seller;
    [SerializeField] private GameObject _BuyButton;
    [SerializeField] private TextMeshProUGUI _BuyButtonText;
    [SerializeField] private Text _descriptionText;
    private float _speed;
    private int _sellPerTime;
    private int _upgradeCost;

    public void SelfOpen()
    {
        RefreshInfo();
    }

    private void Update()
    {
        OpenningCard();
    }

    public void RefreshInfo()
    {
        _speed = 100 + _seller.Lvl - 1;

        if (_seller.Lvl % 10 == 0)
        {
            _sellPerTime = (int)(14 * Mathf.Pow(10, (_seller.Lvl + 1) / 10));
            _upgradeCost = (int)(60 * Mathf.Pow(10, (_seller.Lvl + 1) / 10));
        }
        else
        {
            _sellPerTime = (int)(14 * Mathf.Max(_seller.Lvl % 10, 1) * Mathf.Pow(10, _seller.Lvl / 10));
            _upgradeCost = (int)(60 * Mathf.Max(_seller.Lvl % 10, 1) * Mathf.Pow(10, _seller.Lvl / 10));
        }

        _currentProfitText.text = _gameController.ShowMoney(_sellPerTime);
        _riseProfitText.text = "+ " + _gameController.ShowMoney((int)(14 * Mathf.Max((_seller.Lvl + 1) % 10, 1) * Mathf.Pow(10, (_seller.Lvl + 1) / 10)) - _sellPerTime);
        _currentSpeedText.text = _speed + "%";
        _riseSpeedText.text = "+" + "1" + "%";
        _BuyButtonText.text = "<sprite=0> " + _gameController.ShowMoney(_upgradeCost);
        _descriptionText.text = "Production line - Level " + _seller.Lvl;
        CheckOnUpgrade();
    }

    private void CheckOnUpgrade()
    {
        bool _isUpgradeble = false;

        if (_gameController.Money - _upgradeCost < new System.Numerics.BigInteger(0))
        {
            _isUpgradeble = false;
        }
        else
        {
            _isUpgradeble = true;
        }
        _BuyButton.GetComponent<Button>().enabled = _isUpgradeble;
    }
}