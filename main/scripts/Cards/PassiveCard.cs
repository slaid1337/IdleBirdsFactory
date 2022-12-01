using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class PassiveCard : BasicCard
{
    [SerializeField] private Text _text1;
    [SerializeField] private Text _text2;
    [SerializeField] private Text _text3;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _videoButton;
    [SerializeField] private Button _goldenButton;

    private BigInteger _profit;

#if UNITY_ANDROID
    private AdmobAds _ads = null;
#endif

    void Update()
    {
        OpenningCard();
    }

    public void SelfOpen(BigInteger profit)
    {

#if UNITY_ANDROID
        _ads = AdmobAds.Instance;
#endif

        _profit = profit;
        _text1.text = _gameController.ShowMoney(_profit);
        _text2.text = _gameController.ShowMoney(_profit * 2);
        _text3.text = _gameController.ShowMoney(_profit * 3);

        _closeButton.onClick.AddListener(GetProfit);

        if (_gameController.Cash < 15)
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

#if UNITY_ANDROID
        if (_ads._rewarded2XOfflineProfit.IsLoaded())
        {
            _videoButton.interactable = true;
            _videoButton.onClick.AddListener(_ads.ShowRewarded2XOfflineProfit);
        }
        else
        {
            _videoButton.interactable = false;
        }
#endif

#if UNITY_WEBGL
        _videoButton.onClick.AddListener(YandexSDK.Instance.ShowRewardAdvertisment);
        YandexSDK.Instance.RewardGet += GetProfit2X;
#endif
    }

    private void OnDisable()
    {
#if UNITY_WEBGL
        _videoButton.onClick.RemoveListener(YandexSDK.Instance.ShowRewardAdvertisment);
        YandexSDK.Instance.RewardGet -= GetProfit2X;
#endif
    }

    public void GetProfit()
    {
        _closeButton.onClick.RemoveListener(GetProfit);
#if UNITY_ANDROID
        _videoButton.onClick.RemoveListener(_ads.ShowRewarded2XOfflineProfit);
#endif
        _goldenButton.GetComponent<Button>().onClick.RemoveListener(GetProfit3X);
        _gameController.AddMoneyWithoutEvent(_profit);
        _gameController.CollectEffect.SetActive(true);
        _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_gameController._egg, "+ " + _gameController.ShowMoney(_profit));
        gameObject.SetActive(false);
    }

    public void GetProfit2X()
    {
        _closeButton.onClick.RemoveListener(GetProfit);
#if UNITY_ANDROID
        _videoButton.onClick.RemoveListener(_ads.ShowRewarded2XOfflineProfit);
#endif
        _goldenButton.GetComponent<Button>().onClick.RemoveListener(GetProfit3X);
        _gameController.AddMoneyWithoutEvent(_profit * 2);
        _gameController.CollectEffect.SetActive(true);
        _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_gameController._egg, "+ " + _gameController.ShowMoney(_profit * 2));
        _gameController._FadeBackground.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void GetProfit3X()
    {
        _closeButton.onClick.RemoveListener(GetProfit);
#if UNITY_ANDROID
        _videoButton.onClick.RemoveListener(_ads.ShowRewarded2XOfflineProfit);
#endif
        _goldenButton.GetComponent<Button>().onClick.RemoveListener(GetProfit3X);
        _gameController.AddMoneyWithoutEvent(_profit * 3);
        _gameController.SpendCash(15);
        _gameController.CollectEffect.SetActive(true);
        _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_gameController._egg, "+ " + _gameController.ShowMoney(_profit * 3));
        gameObject.SetActive(false);
    }
}
