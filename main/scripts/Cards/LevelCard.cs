using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelCard : BasicCard
{
    [SerializeField] private Text _text1;
    [SerializeField] private Text _text2;
    [SerializeField] private Text _text3;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _videoButton;
    [SerializeField] private Button _goldenButton;

    private System.Numerics.BigInteger _profit;

    private AdmobAds _ads = null;

    public void SelfOpen(System.Numerics.BigInteger profit)
    {
        _ads = AdmobAds.Instance;
        _profit = profit / new System.Numerics.BigInteger(1000);

        _text1.text = _gameController.ShowMoney(_profit);
        _text2.text = _gameController.ShowMoney(_profit * 2);
        _text3.text = _gameController.ShowMoney(_profit * 3);

        _closeButton.onClick.AddListener(GetProfit);

        _gameController._saveController.SaveAll();

        if (_gameController.Cash < 10)
        {
            Image goldenButtonImage = _goldenButton.transform.GetComponent<Image>();
            goldenButtonImage.color = new Color(goldenButtonImage.color.r, goldenButtonImage.color.g, goldenButtonImage.color.b, 0.5f);
            Image[] childImages = _goldenButton.transform.GetComponentsInChildren<Image>();
            foreach (var item in childImages)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, 0.5f);
            }
        }
        else
        {
            Image goldenButtonImage = _goldenButton.transform.GetComponent<Image>();
            goldenButtonImage.color = new Color(goldenButtonImage.color.r, goldenButtonImage.color.g, goldenButtonImage.color.b, 1f);
            Image[] childImages = _goldenButton.transform.GetComponentsInChildren<Image>();
            foreach (var item in childImages)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, 1f);
            }
            _goldenButton.GetComponent<Button>().onClick.AddListener(GetProfit3X);
        }
    }

    private void OnEnable()
    {
        _videoButton.onClick.AddListener(_ads.ShowRewarded2XLvlUp);
    }

    private void OnDisable()
    {
        _videoButton.onClick.RemoveListener(_ads.ShowRewarded2XLvlUp);
    }

    public void GetProfit()
    {
        _closeButton.onClick.RemoveListener(GetProfit);
        _videoButton.onClick.RemoveListener(_ads.ShowRewarded2XLvlUp);
        _goldenButton.GetComponent<Button>().onClick.RemoveListener(GetProfit3X);
        _gameController.AddMoneyWithoutEvent(_profit);
        _gameController.CollectEffect.SetActive(true);
        _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_gameController._egg, "+ " + _gameController.ShowMoney(_profit));
        gameObject.SetActive(false);
    }

    public void GetProfit2X()
    {
        _closeButton.onClick.RemoveListener(GetProfit);
        _videoButton.onClick.RemoveListener(_ads.ShowRewarded2XLvlUp);
        _goldenButton.GetComponent<Button>().onClick.RemoveListener(GetProfit3X);
        _gameController.AddMoneyWithoutEvent(_profit * 2);
        _gameController._FadeBackground.gameObject.SetActive(false);
        _gameController.CollectEffect.SetActive(true);
        _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_gameController._egg, "+ " + _gameController.ShowMoney(_profit * 2));
        gameObject.SetActive(false);
    }

    public void GetProfit3X()
    {
        _closeButton.onClick.RemoveListener(GetProfit);
        _videoButton.onClick.RemoveListener(_ads.ShowRewarded2XLvlUp);
        _goldenButton.GetComponent<Button>().onClick.RemoveListener(GetProfit3X);
        _gameController.AddMoneyWithoutEvent(_profit * 3);
        _gameController.SpendCash(10);
        _gameController.CollectEffect.SetActive(true);
        _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_gameController._egg, "+ " + _gameController.ShowMoney(_profit * 3));
        gameObject.SetActive(false);
    }

    private void Update()
    {
        OpenningCard();
    }
}