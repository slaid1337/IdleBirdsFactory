using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_ANDROID
using Firebase;
using Firebase.Analytics;
#endif

public class GameController : ControllerBase
{
    private int _cash;
    private int _maxProfitPerHour;
    [SerializeField] private Text _cashText;
    [SerializeField] private Text _moneyText;
    [SerializeField] private Text _cashText2;
    [SerializeField] private Text _moneyText2;

    public BirdObject[] BirdsObjects;
    public List<BirdObject> _availableBirds;
    
    [SerializeField] private int _lvl;

    [SerializeField] private GameObject _cellContainer;
    public Cell[] Cells;

    public Seller _Seller;

    public Sprite _egg;
    public Sprite _goldenEgg;

    public List<BoosterObject> BoostersContainer;
    public BoosterObject ActiveBooster;
    public DateTime EndTimeEffectBosster;

    public UnityEvent<int> OnChangeMoney = new UnityEvent<int>();
    public UnityEvent<int> OnAddMoney = new UnityEvent<int>();
    public UnityEvent<int> OnAddLvl = new UnityEvent<int>();

    public GameObject UpgradeCard;
    public GameObject UnlockCard;
    public GameObject SpinCard;
    public GameObject CollectCard;
    public GameObject LevelCard;
    public GameObject SellerCard;
    public GameObject MissionCard;
    public GameObject BoosterCard;
    public GameObject OfflineProfitCard;
    public GameObject CollectEffect;
    public LevelController _levelController;

    public GameObject MainCanvas;

    public FadeBackground _FadeBackground;

    public MissionsController missionsController;

    [SerializeField] private BoosterObject[] _allBoosters;
    public SaveController _saveController;

    [SerializeField] private Tutorial _tutorial;

    public ServicesManager _servicesManager;

#if UNITY_ANDROID

    private FirebaseApp app;

    public AdmobAds _adsAdmob;

#endif

    [SerializeField] private GameObject _bottomUI;
    private bool _IsAdShowable = true;

    public BirdRout[] birdRoutes;

    [SerializeField] private GameObject _scroller;

#if UNITY_WEBGL
    private YandexSDK _yandexSDK;
#endif

    public int Cash
    {
        get
        {
            return _cash;
        }
    }

    public int Money
    {
        get
        {
            return _money;
        }
    }

    public int MaxProfitPerHour
    {
        get
        {
            return _maxProfitPerHour;
        }
    }

    public int Lvl
    {
        get
        {
            return _lvl;
        }
    }

    public bool IsAdShowable
    {
        get
        {
            return _IsAdShowable;
        }
        set
        {
            _IsAdShowable = value;
        }
    }

    private void Awake()
    {
#if UNITY_EDITOR
                Debug.Log("Unity Editor");
#endif

#if UNITY_ANDROID
                Debug.Log("Unity android");
#endif

#if UNITY_WEBGL
                Debug.Log("Unity webgl");
#endif

        Application.targetFrameRate = 60;

        _availableBirds = new List<BirdObject>();

        LoadSave();

        RefreshCashText();
        ShowMoney(_moneyText, _money);
        ShowMoney(_moneyText2, _money);
        SendChangedMoney(_money);

        ServiceSave serviceSave = _saveController.LoadService();

        StatisticSave statsSave = _saveController.LoadStatistic();

        _IsAdShowable = !serviceSave.AdBlock;

        if (statsSave.CountOfGameStarts > 1)
            StartCoroutine(LoadAndOpenPassiveCard());
        else
        {
            _tutorial._hand_unlock.SetActive(true);
            _tutorial._hand_shop.SetActive(true);
            _tutorial._isTutor = true;
        }

#if UNITY_ANDROID
        try
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);

                    Debug.Log("FireBase status - " + dependencyStatus);
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }
        catch
        {
            Debug.Log("FireBase is not loaded");
        }
#endif

        Debug.Log("--------------------1" + statsSave.CountOfGameStarts);

    }

    private void Start()
    {
        StartCoroutine(ListToHighter());

#if UNITY_WEBGL
        _yandexSDK = YandexSDK.Instance;
        OnBannerShow();
#endif
    }

    private void Update()
    {
        if (ActiveBooster != null)
        {
            if (DateTime.Now >= EndTimeEffectBosster)
            {
                EndTimeEffectBosster = DateTime.MinValue;
                ActiveBooster = null;
            }
        }
    }

    public void SpendMoney(int money)
    {
        _money -= money;
        ShowMoney(_moneyText, _money);
        ShowMoney(_moneyText2, _money);
        SendChangedMoney(_money);
    }

    public void AddMoney(int money)
    {
        int boosterForce = 1;

        if (BoostersContainer.Count > 0)
        {
            foreach (var item in BoostersContainer)
            {
                boosterForce = boosterForce * item.BoostForce;
            }
        }

        _money += money * boosterForce;
        ShowMoney(_moneyText, _money);
        ShowMoney(_moneyText2, _money);
        SendChangedMoney(_money);
        SendAddMoney(money);
        
        // ИЗМЕНЕНО: убрано создание BigInteger
        for (int i = Cells.Length - 1 ; i >= 0; i--)
        {
            if(Cells[i].BirdCount > 0)
            {
                _maxProfitPerHour = Cells[i].MoneyPerSecond * 3600;
                break;
            }
        }
    }

    public void AddMoneyWithoutEvent(int money)
    {
        _money += money;
        ShowMoney(_moneyText, _money);
        ShowMoney(_moneyText2, _money);
        SendChangedMoney(_money);
    }

#if UNITY_EDITOR
    [ContextMenu("Add Test Money")]
    public void AddTestMoney()
    {
        AddMoney(1000000);
        Debug.Log("Test money added!");
    }

    [ContextMenu("Add Test Cash")]
    public void AddTestCash()
    {
        AddCash(10000);
        Debug.Log("Test cash added!");
    }

    [ContextMenu("Level Up")]
    public void TestLevelUp()
    {
        LvlUp();
        Debug.Log("Level up!");
    }
#endif

    private void SendChangedMoney(int money)
    {
        OnChangeMoney.Invoke(money);
    }

    private void SendAddMoney(int money)
    {
        OnAddMoney.Invoke(money);
    }

    private void RefreshCashText()
    {
        _cashText.text = _cash.ToString();
        _cashText2.text = _cash.ToString();
    }

    public void SpendCash(int cash)
    {
        _cash -= cash;
        RefreshCashText();
    }

    public void AddCash(int cash)
    {
        _cash += cash;
        RefreshCashText();
    }

    public void LvlUp()
    {
        _lvl++;
        SendOnLvlUp(_lvl);
    }

    public void SendOnLvlUp(int lvl)
    {
        OnAddLvl.Invoke(lvl);
    }

    private void LoadSave()
    {
        GameControllerSave save = _saveController.LoadGameController();

        _cash = save.Cash;
        _money = save.Money;

        _lvl = save.Level;

        if (save.BoostersNumbers.Length > 0)
        {
            foreach (var item in save.BoostersNumbers)
            {
                foreach (var booster in _allBoosters)
                {
                    if (booster.number == item)
                    {
                        BoostersContainer.Add(booster);
                    }
                }
            }
        }

        if (save.ActiveBooster.Count > 0)
        {
            Dictionary<int, DateTime>.KeyCollection tmpDict = save.ActiveBooster.Keys;

            foreach (var item in _allBoosters)
            {
                foreach (var tmp in tmpDict)
                {
                    if (item.number == tmp)
                    {
                        if (save.ActiveBooster[item.number] > DateTime.Now)
                        {
                            ActiveBooster = item;
                            EndTimeEffectBosster = save.ActiveBooster[ActiveBooster.number];
                        }
                    }
                }
            }

            
        }

        foreach (var item in BirdsObjects)
        {
            if (!item.IsTaken)
            {
                _availableBirds.Add(item);
            }
        }
    }

    private IEnumerator LoadAndOpenPassiveCard()
    {
        yield return new WaitForSeconds(1f);

        OfflineSave save = _saveController.LoadOfflineDate();

        for (int i = Cells.Length - 1; i >= 0; i--)
        {
            if (Cells[i].BirdCount > 0)
            {
                _maxProfitPerHour = Cells[i].MoneyPerSecond * 3600;
                break;
            }
        }

        if (MaxProfitPerHour > 0)
        {

            double totalSeconds = (DateTime.Now - save.DateOfQuit).TotalSeconds;

            // Расчет офлайн прибыли
            int money = (int)(_maxProfitPerHour * totalSeconds / 3600.0);
            money = Mathf.Min(money, 100000); // Ограничиваем офлайн прибыль
            
            Debug.Log("Offline profit: " + money);
            PassiveCard card = OfflineProfitCard.GetComponent<PassiveCard>();

            OfflineProfitCard.SetActive(true);
            card.OpenCard();
            card.SelfOpen(money);

            _FadeBackground.gameObject.SetActive(true);
            _FadeBackground.Fade();
        }
    }

    public void OnBannerShow()
    {
        _bottomUI.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector2(0f, -326f);
    }

    public void OnBannerClose()
    {
        _bottomUI.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector2(0f, -380f);
    }

    private IEnumerator ListToHighter()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = Cells.Length - 1; i >= 0; i--)
        {
            if (Cells[i].BirdCount > 0)
            {
                _scroller.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector2(0f, -Cells[i].GetComponent<RectTransform>().anchoredPosition.y);
                break;
            }
        }
    }
}