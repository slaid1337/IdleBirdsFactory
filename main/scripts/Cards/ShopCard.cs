using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopCard : BasicCard
{
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private Image _image;
    [SerializeField] private Button _buyButton;
    [SerializeField] private GameObject _buyFade;
    private BoosterObject _booster;
    [SerializeField] private Tutorial _tutorial;

    private void Update()
    {
        OpenningCard();
    }

    public void SelfOpen(BoosterObject booster)
    {
        _image.sprite = booster.BoostImage;
        _cost.text = "Get it for : " + booster.Cost;
        _booster = booster;
        if (_booster.TypeBooster == BoosterType.TimeWrap)
        {
            _description.text = booster.Description + "(" + _gameController.ShowMoney(_gameController.MaxProfitPerHour * _booster.Duration) + ")";
        }
        else
        {
            _description.text = booster.Description;
        }
        
        if (_gameController.Cash >= _booster.Cost)
        {
            _buyFade.SetActive(false);
            _buyButton.enabled = true;
            _buyButton.onClick.AddListener(Buy);
        }

        if (_tutorial._isTutor)
        {
            _tutorial._hand_buy_booster.SetActive(true);
        }
    }

    public void Close()
    {
        _buyButton.onClick.RemoveListener(Buy);
        _buyButton.enabled = false;
        _buyFade.SetActive(true);
    }

    public void Buy()
    {
        if (_booster.TypeBooster == BoosterType.TimeWrap)
        {
            _gameController.SpendCash(_booster.Cost);
            _gameController.AddMoney(_gameController.MaxProfitPerHour * _booster.Duration);
            _gameController.CollectEffect.SetActive(true);
            _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_gameController._egg,"+ " + _gameController.ShowMoney(_gameController.MaxProfitPerHour * _booster.Duration));
        }
        else if (_booster.TypeBooster == BoosterType.MoneyBoost)
        {
            _gameController.SpendCash(_booster.Cost);
            _gameController.BoostersContainer.Add(_booster);
            _gameController.CollectEffect.SetActive(true);
            _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_booster.BoostImage, "");
        }
        if (_tutorial._isTutor)
        {
            _tutorial._hand_buy_booster.SetActive(false);
            _tutorial._hand_booster.SetActive(true);
        }
        _buyButton.onClick.RemoveListener(Buy);
        _buyButton.enabled = false;
        _buyFade.SetActive(true);
    }
}