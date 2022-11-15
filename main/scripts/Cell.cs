using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using System.Text;
using UnityEngine.Events;
using GooglePlayGames;
//using Firebase;
//using Firebase.Analytics;

public class Cell : ControllerBase
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private BigInteger _moneyPerSecond;
    [SerializeField] private BigInteger _upgradeCost;
    private BigInteger _moneyPerSecondForShow;
    [SerializeField] private int _birdCount = 0;
    public GameObject[] Birds;
    [SerializeField] private GameObject[] _cellsBg;
    [SerializeField] private TextMeshProUGUI _cellText;
    private int _stage;
    [SerializeField] private string _birdName;
    [SerializeField] private float _bonus;
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private GameObject _unlockButton;
    [SerializeField] private GameObject _fullImage;
    private BirdObject _birdObject;
    private bool _isUpgradeble;
    private int _birdIndex;

    private bool _isLock;
    [SerializeField] private GameObject[] _lockObjects;

    private bool _isUnlocked = false;

    [SerializeField] private ParticleSystem _particleSystem;

    public UnityEvent<int> OnUpgrade = new UnityEvent<int>();
    public UnityEvent<int> OnUnlock = new UnityEvent<int>();

    public RectTransform _rectTransform;

    public bool IsUnlocked
    {
        get
        {
            return _isUnlocked;
        }
    }

    public int Stage
    {
        get
        {
            return _stage;
        }
    }

    public int BirdIndex
    {
        get
        {
            return _birdIndex;
        }
        set
        {
            _birdIndex = value;
        }
    }

    public int BirdCount
    {
        get
        {
            return _birdCount;
        }
    }

    public BigInteger MoneyPerSecond
    {
        get
        {
            return _moneyPerSecond;
        }
    }

    public BigInteger UpgradeCost
    {
        get
        {
            return _upgradeCost;
        }
    }

    public BirdObject _BirdObject
    {
        get
        {
            return _birdObject;
        }
    }

    private void Start()
    {
        _stage = transform.GetSiblingIndex() + 1;
        _rectTransform = GetComponent<RectTransform>();
        CellsSave[] saveObject = _gameController._saveController.CellsSave;
        
        CellsSave previousSaveCell = null;
        
        int countOfPreviousBirds = 0;
        
        CellsSave save = null;
        
        foreach (var item in saveObject)
        {
            if (item.Stage == _stage)
            {
                save = item;
                break;
            }
        }
        
        foreach (var item in saveObject)
        {
            if (item.Stage == _stage - 1)
            {
                previousSaveCell = item;
                break;
            }
        }



        if (_stage == 1)
            countOfPreviousBirds = 1;
        else
        {
            countOfPreviousBirds = previousSaveCell.CountOfBirds;
            _lockObjects[0].GetComponent<Button>().onClick.RemoveListener(BreakLock);
        }

        _isUnlocked = save.IsUnlocked;
        //_isUnlocked = true;

        if (_isUnlocked)
        {
            _birdCount = save.CountOfBirds;
            _birdName = save.BirdName;

            //_birdCount = 18;
            //_birdName = "bird-bee";

            foreach (var item in _gameController.BirdsObjects)
            {
                if (item.Name == _birdName)
                {
                    _birdObject = item;
                    break;
                }
            }

            _bonus = _birdObject.Bonus;
            //_bonus = 2;

            for (int i = 0; i < Birds.Length; i++)
            {
                Birds[i].GetComponent<Bird>().Image = _birdObject.Image;
                Birds[i].GetComponent<Bird>().Image2 = _birdObject.Image2;
                Birds[i].GetComponent<Bird>().Image3 = _birdObject.Image3;
            }

            if (_stage > 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    _lockObjects[i].SetActive(false);
                }
            }

            for (int i = 0; i < _birdCount; i++)
            {
                Birds[i].transform.parent.gameObject.SetActive(true);
                _cellsBg[i].gameObject.SetActive(true);
            }

            var em = _particleSystem.emission;
            em.enabled = true;
            em.rateOverTime = _birdCount;

            ButtonSwitcher(true);

            _gameController.OnChangeMoney.AddListener(CheckOnUpgrade);

            if (_birdCount == 18)
            {
                _gameController.OnChangeMoney.RemoveListener(CheckOnUpgrade);
                _upgradeButton.SetActive(false);
                _fullImage.SetActive(true);
            }

            _moneyPerSecond = 12 * ((int)Mathf.Pow(10, _stage) / 10) * new BigInteger(_bonus);
            ButtonEnabler(true);

            

            _upgradeCost = new BigInteger(12) * (new BigInteger(Mathf.Pow(10, _stage)) / new BigInteger(10)) * new BigInteger(Mathf.Max(_birdCount, 1));
            CheckOnUpgrade(_gameController.Money);
        }
        else
        {
            if (_gameController.Lvl < (_stage - 1) * 5)
            {
                _isLock = true;
            }
            else
            {
                _isLock = false;
            }

            if (!_isLock && _isUnlocked)
            {
                foreach (var item in _lockObjects)
                {
                    item.SetActive(false);
                }

                for (int i = 0; i < _birdCount; i++)
                {
                    Birds[i].transform.parent.gameObject.SetActive(true);
                }

                _birdCount = save.CountOfBirds;
                _birdName = save.BirdName;

                foreach (var item in _gameController.BirdsObjects)
                {
                    if (item.Name == save.BirdName)
                    {
                        _birdObject = item;
                        break;
                    }
                }

                _bonus = _birdObject.Bonus;

                var em = _particleSystem.emission;
                em.enabled = true;
                em.rateOverTime = _birdCount;

                _unlockButton.GetComponent<Button>().onClick.RemoveListener(OpenUnlockCard);
                _moneyPerSecond = 12 * ((int)Mathf.Pow(10, _stage) / 10) * new BigInteger(_bonus);
                ButtonEnabler(true);

                _gameController.OnChangeMoney.AddListener(CheckOnUpgrade);

                _upgradeCost = new BigInteger(12) * (new BigInteger(Mathf.Pow(10, _stage)) / new BigInteger(10)) * new BigInteger(Mathf.Max(_birdCount, 1));

                CheckOnUpgrade(_gameController.Money);
            }
            else if (countOfPreviousBirds > 0)
            {
                _gameController.OnAddLvl.AddListener(CheckOnUnlocable);
                ButtonEnabler(false);
                AdmobAds ads = AdmobAds.Instance;
                ads._cell = this;

                if (_stage == 1)
                    CheckOnUpgrade(_gameController.Money);
                else
                {
                    _lockObjects[0].GetComponent<Button>().onClick.AddListener(ads.ShowRewardedUnlockStage);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    _lockObjects[i].SetActive(false);
                }
                _gameController.Cells[_stage - 2].OnUnlock.AddListener(AddEventOnUnlockable);

                CheckOnUpgrade(_gameController.Money);
                ButtonEnabler(false);
            }
        }
        
        RefreshInfo();
    }

    public void Unlock()
    {
        _upgradeCost = new BigInteger(12) * (new BigInteger(Mathf.Pow(10, _stage)) / new BigInteger(10)) * new BigInteger(Mathf.Max(_birdCount, 1));
        _gameController.SpendMoney(_upgradeCost);

        _gameController.UnlockCard.GetComponent<UnlockCard>().CloseCard();
        _gameController.UnlockCard.SetActive(false);

        if (_gameController._availableBirds.Count == 0)
        {
            foreach (var item in _gameController.BirdsObjects)
            {
                item.IsTaken = false;
                _gameController._availableBirds.Add(item);
            }
        }

        int randomBird = Random.Range(0, _gameController._availableBirds.Count - 1);

        _birdIndex = randomBird;

        _gameController.SpinCard.SetActive(true);
        _gameController.SpinCard.GetComponent<SpinCard>().OpenCard();
        _gameController.SpinCard.GetComponent<SpinCard>().SelfOpen(randomBird, this);
    }

    public void CollectBird()
    {
        _gameController._FadeBackground.Close();
        ButtonSwitcher(true);

        Birds[0].transform.parent.gameObject.SetActive(true);
        _cellsBg[0].gameObject.SetActive(true);
        _birdCount += 1;

        _birdObject = _gameController._availableBirds[_birdIndex];
        _birdName = _birdObject.Name;
        _bonus = _birdObject.Bonus;
        _birdObject.SetTaken();

        _gameController._availableBirds.Remove(_birdObject);

        _moneyPerSecond = 12 * ((int)Mathf.Pow(10, _stage) / 10) * new BigInteger(_bonus);

        for (int i = 0; i < Birds.Length; i++)
        {
            Birds[i].GetComponent<Bird>().Image = _birdObject.Image;
            Birds[i].GetComponent<Bird>().Image2 = _birdObject.Image2;
            Birds[i].GetComponent<Bird>().Image3 = _birdObject.Image3;
        }

        SendOnUnlock(_stage);

        RefreshInfo();
        CheckOnUpgrade(_gameController.Money);
        _gameController._FadeBackground.gameObject.SetActive(true);
        _gameController._FadeBackground.Open();
        _gameController.CollectEffect.SetActive(true);
        _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_birdObject.Image, _birdName);

        var em = _particleSystem.emission;
        em.enabled = true;
        em.rateOverTime = _birdCount;

        _isUnlocked = true;

        //try
        //{
        //    FirebaseAnalytics.LogEvent("UnlockStage" + _stage);
        //}
        //catch
        //{

        //}

        //switch (_stage)
        //{
        //    case 1:
        //        Social.ReportProgress("CgkIyIa2qoYLEAIQAg", 100.0d, x => Debug.Log("status unlocking achievement 1 + " + x));
        //        break;
        //    case 2:
        //        Social.ReportProgress("CgkIyIa2qoYLEAIQAw", 100.0d, x => Debug.Log("status unlocking achievement 1 + " + x));
        //        break;
        //    case 5:
        //        Social.ReportProgress("CgkIyIa2qoYLEAIQBA", 100.0d, x => Debug.Log("status unlocking achievement 1 + " + x));
        //        break;
        //    case 10:
        //        Social.ReportProgress("CgkIyIa2qoYLEAIQBQ", 100.0d, x => Debug.Log("status unlocking achievement 1 + " + x));
        //        break;
        //    case 15:
        //        Social.ReportProgress("CgkIyIa2qoYLEAIQBg", 100.0d, x => Debug.Log("status unlocking achievement 1 + " + x));
        //        break;
        //    case 20:
        //        Social.ReportProgress("CgkIyIa2qoYLEAIQBw", 100.0d, x => Debug.Log("status unlocking achievement 1 + " + x));
        //        break;
        //    case 30:
        //        Social.ReportProgress("CgkIyIa2qoYLEAIQCA", 100.0d, x => Debug.Log("status unlocking achievement 1 + " + x));
        //        break;
        //    case 50:
        //        Social.ReportProgress("CgkIyIa2qoYLEAIQCQ", 100.0d, x => Debug.Log("status unlocking achievement 1 + " + x));
        //        break;
        //    case 90:
        //        Social.ReportProgress("CgkIyIa2qoYLEAIQCg", 100.0d, x => Debug.Log("status unlocking achievement 1 + " + x));
        //        break;
        //}

        foreach (var item in _gameController.birdRoutes)
        {
            item._cells.Add(this);
        }

        if (_gameController.IsAdShowable)
        {
            AdmobAds.Instance.ShowInterstitial();
        }
    }

    public void Upgrade()
    {
        _upgradeCost = new BigInteger(12) * (new BigInteger(Mathf.Pow(10, _stage)) / new BigInteger(10)) * new BigInteger(Mathf.Max(_birdCount, 1));
        Birds[_birdCount].transform.parent.gameObject.SetActive(true);
        _cellsBg[_birdCount].gameObject.SetActive(true);
        _birdCount += 1;

        _gameController.SpendMoney(_upgradeCost);

        RefreshInfo();
        CheckOnUpgrade(_gameController.Money);
        _gameController.UpgradeCard.GetComponent<UpgradeCard>().Refresh();

        _moneyPerSecond = 12 * ((int)Mathf.Pow(10, _stage) / 10) * _birdCount * new BigInteger(_bonus);

        SendOnUpgrade(1);

        if (_birdCount == 18)
        {
            _gameController.OnChangeMoney.RemoveListener(CheckOnUpgrade);
            _upgradeButton.SetActive(false);
            _fullImage.SetActive(true);
            _gameController.AddCash(2);
            _gameController.UpgradeCard.GetComponent<UpgradeCard>().CloseCard();
            _gameController.UpgradeCard.SetActive(false);
            _gameController.CollectEffect.SetActive(true);
            _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_gameController._goldenEgg, "+ 2");
        }

        var em = _particleSystem.emission;
        em.enabled = true;
        em.rateOverTime = _birdCount;

        Debug.Log(_upgradeCost);
    }

    private void RefreshInfo()
    {
        if (_isLock)
        {
            _cellText.text = "Stage " + _stage + " : " + "\r\nLevel " + ((_stage - 1) * 5) + " to unlock";
        }
        else
        {
            int length = _money.ToString().Length;
            _moneyPerSecondForShow = _moneyPerSecond;

            _cellText.text = "Stage " + _stage + " : " + _birdName + "\r\n<sprite=0> " + ShowMoney(_moneyPerSecondForShow) + _chars[length / 3] + "/s " + "x" + _bonus;
        }
    }

    private void CheckOnUpgrade(BigInteger money)
    {
        _upgradeCost = new BigInteger(12) * (new BigInteger(Mathf.Pow(10, Mathf.Max(1,_stage))) / new BigInteger(10)) * new BigInteger(Mathf.Max(_birdCount, 1));
        _isUpgradeble = false;
        if (money - _upgradeCost > new BigInteger(0))
        {
            _isUpgradeble = true;
        }
        
        ButtonEnabler( _isUpgradeble);
    }

    private void ButtonEnabler(bool switcher)
    {
        Button unlock = _unlockButton.GetComponent<Button>();
        Button upgrade = _upgradeButton.GetComponent<Button>();
        unlock.interactable = switcher;
        upgrade.interactable = switcher;
    }

    private void ButtonSwitcher(bool switcher)
    {
        _upgradeButton.SetActive(switcher);
        _unlockButton.SetActive(!switcher);
    }

    public void OpenUpgradeCard()
    {
        _gameController._FadeBackground.gameObject.SetActive(true);
        _gameController._FadeBackground.Fade();
        _gameController.UpgradeCard.SetActive(true);
        _gameController.UpgradeCard.GetComponent<UpgradeCard>().OpenCard();
        _gameController.UpgradeCard.GetComponent<UpgradeCard>().SelfOpen(this);
    }

    public void OpenUnlockCard()
    {
        _gameController._FadeBackground.gameObject.SetActive(true);
        _gameController._FadeBackground.Fade();
        _gameController.UnlockCard.SetActive(true);
        _gameController.UnlockCard.GetComponent<UnlockCard>().OpenCard();
        _gameController.UnlockCard.GetComponent<UnlockCard>().SelfOpen(this);
    }

    public void SendOnUpgrade(int upgrade)
    {
        OnUpgrade.Invoke(upgrade);
    }

    public void SendOnUnlock(int unlock)
    {
        OnUnlock.Invoke(unlock);
    }

    public void BreakLock()
    {
        _isLock = false;
        RefreshInfo();

        foreach (var item in _lockObjects)
        {
            item.SetActive(false);
        }

        ButtonEnabler(true);
        _unlockButton.GetComponent<Button>().onClick.AddListener(OpenUnlockCard);
        _gameController.OnAddLvl.RemoveListener(CheckOnUnlocable);

        CheckOnUpgrade(_gameController.Money);
    }

    public void CheckOnUnlocable(int lvl)
    {
        
        if (_gameController.Lvl >= (_stage - 1) * 5)
        {
            BreakLock();
        }
        else if (_isLock)
        {
            for (int i = 0; i < 3; i++)
            {
                _lockObjects[i].SetActive(true);
            }
        }
    }

    public void SetUnlocable(int lvl)
    {
        _isLock = true;
        _gameController.Cells[_stage - 2].OnUnlock.RemoveListener(CheckOnUnlocable);
    }

    public void AddEventOnUnlockable(int lvl)
    {
        AdmobAds ads = AdmobAds.Instance;
        ads._cell = this;
        _lockObjects[0].GetComponent<Button>().onClick.AddListener(ads.ShowRewardedUnlockStage);
        _gameController.OnAddLvl.AddListener(CheckOnUnlocable);
        _gameController.Cells[_stage - 2].OnUnlock.RemoveListener(AddEventOnUnlockable);
        for (int i = 0; i < 3; i++)
        {
            _lockObjects[i].SetActive(true);
        }
    }
}