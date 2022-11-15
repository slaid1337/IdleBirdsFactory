using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    private float _height;
    private float _switchValue;
    private bool _switcher;
    private RectTransform _position;
    [SerializeField] private GameController _gameController;
    [SerializeField] private Text _cashText;
    [SerializeField] private Text _moneyText;
    [SerializeField] private FadeBackground _fadeBackground;
    [SerializeField] private ShopCard _card;
    [SerializeField] private ShopItem[] _shopItems;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private GameObject _scroller;

    [SerializeField] private TextMeshProUGUI _textInApp1;
    [SerializeField] private TextMeshProUGUI _textInApp2;
    [SerializeField] private TextMeshProUGUI _textInApp3;
    [SerializeField] private TextMeshProUGUI _textInApp4;
    [SerializeField] private TextMeshProUGUI _textInApp5;
    [SerializeField] private TextMeshProUGUI _textInApp6;

    private void Awake()
    {
        _height = _gameController.MainCanvas.GetComponent<RectTransform>().sizeDelta.y;
        _switcher = false;
        _switchValue = 0;
        _position = gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_switcher)
        {
            _switchValue += Time.deltaTime * 1.75f;
            _position.anchoredPosition = Vector3.Lerp(new Vector3(0, -_height, 0), Vector3.zero, Mathf.SmoothStep(0.0f, 1.0f, Mathf.Min(_switchValue, 1f)));
            if (_switchValue >= 1f)
            {
                _switcher = false;
                _switchValue = 0;
            }
        }
    }

    public void ShowWindow()
    {
        gameObject.SetActive(true);
        _switcher = true;
        _position.anchoredPosition = new Vector3(0, -_height, 0);
        _cashText.text = _gameController.Cash.ToString();
        _moneyText.text = _gameController.ShowMoney(_gameController.Money);

        foreach (var item in _shopItems)
        {
            item.RefreshHeader("<sprite=0>+" + _gameController.ShowMoney(_gameController.MaxProfitPerHour * item._boosterObject.Duration));
        }

        if (_tutorial._isTutor)
        {
            _scroller.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 2035f, 0);
            _tutorial._hand_buy_booster.SetActive(true);
        }
        else
        {
            _scroller.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }

        IAPManager iap = IAPManager.Instance;

        _textInApp1.text = iap.GetProducePriceFromStore(iap.Cash200);
        _textInApp2.text = iap.GetProducePriceFromStore(iap.Cash530);
        _textInApp3.text = iap.GetProducePriceFromStore(iap.Cash1100);
        _textInApp4.text = iap.GetProducePriceFromStore(iap.Cash2300);
        _textInApp5.text = iap.GetProducePriceFromStore(iap.Cash6000);
        _textInApp6.text = iap.GetProducePriceFromStore(iap.Cash13000);
    }

    public void OpenItem(BoosterObject booster)
    {
        _fadeBackground.gameObject.SetActive(true);
        _fadeBackground.Fade();
        _card.gameObject.SetActive(true);
        _card.OpenCard();
        _card.SelfOpen(booster);
    }
}
