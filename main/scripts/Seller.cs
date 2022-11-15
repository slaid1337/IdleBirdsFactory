using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Seller : ControllerBase
{
    [SerializeField] private Text _moneyText;
    [SerializeField] private int _lvl;
    [SerializeField] private BigInteger _sellPerTime;
    [SerializeField] private BigInteger _upgradeCost;
    [SerializeField] private GameObject _upgradeButton;
    public GameController _gameController;
    [SerializeField] private Road[] _roads;
    [SerializeField] private Road[] _eggs;
    private bool _isUpgradeble;
    private float _speed;

    private bool _isBoost;
    private float _boostTime;

    public UnityEvent<int> OnUpgrade = new UnityEvent<int>();

    private int _eggIndex;

    public int Lvl
    {
        get
        {
            return _lvl;
        }
    }

    public BigInteger Money
    {
        get
        {
            return _money;
        }
    }

    private void Start()
    {
        SellerSave save = _gameController._saveController.LoadSeller();

        _lvl = save.Lvl;
        _money = BigInteger.Parse(save.Money);

        _speed = 3f - (0.01f * _lvl);

        ShowMoney(_moneyText, _money);
        StartCoroutine(SendMoney());

        SwipeDetector.OnSwipe += BoostSpeedBySwipe;

        if (_lvl % 10 == 0)
        {
            _sellPerTime = new BigInteger(14 * Mathf.Pow(10, (_lvl + 1) / 10));
            _upgradeCost = new BigInteger(60 * Mathf.Pow(10, (_lvl + 1) / 10));
        }
        else
        {
            _sellPerTime = new BigInteger(14 * Mathf.Max(_lvl % 10, 1) * Mathf.Pow(10, _lvl / 10));
            _upgradeCost = new BigInteger(60 * Mathf.Max(_lvl % 10, 1) * Mathf.Pow(10, _lvl / 10));
        }

        _gameController.OnChangeMoney.AddListener(CheckOnUpgrade);
        CheckOnUpgrade(_gameController.Money);

        _boostTime = 0f;

        _eggIndex = 0;
    }

    public void AddMoney(BigInteger money)
    {
        _money += money;
        ShowMoney(_moneyText, _money);
    }

    public void Upgrade()
    {
        if (_lvl % 10 == 0)
        {
            _sellPerTime = new BigInteger(14 * Mathf.Pow(10, (_lvl + 1) / 10));
            _upgradeCost = new BigInteger(60 * Mathf.Pow(10, (_lvl + 1) / 10));
        }
        else
        {
            _sellPerTime = new BigInteger(14 * Mathf.Max(_lvl % 10, 1) * Mathf.Pow(10, _lvl / 10));
            _upgradeCost = new BigInteger(60 * Mathf.Max(_lvl % 10, 1) * Mathf.Pow(10, _lvl / 10));
        }

        _lvl += 1;

        _gameController.SpendMoney(_upgradeCost);
        ShowMoney(_moneyText, _money);
        CheckOnUpgrade(_gameController.Money);
        _gameController.SellerCard.GetComponent<SellerCard>().RefreshInfo();

        _speed = 3f - (0.01f * _lvl);

        foreach (Road item in _roads)
        {
            item.SetSpeed(1);
        }
        foreach (Road item in _eggs)
        {
            item.SetSpeed(1);
        }

        SendOnUpgrade(1);
    }

    private void CheckOnUpgrade(BigInteger money)
    {
        if (_lvl % 10 == 0)
        {
            _sellPerTime = new BigInteger(14 * Mathf.Pow(10, (_lvl + 1) / 10));
            _upgradeCost = new BigInteger(60 * Mathf.Pow(10, (_lvl + 1) / 10));
        }
        else
        {
            _sellPerTime = new BigInteger(14 * Mathf.Max(_lvl % 10, 1) * Mathf.Pow(10, _lvl / 10));
            _upgradeCost = new BigInteger(60 * Mathf.Max(_lvl % 10, 1) * Mathf.Pow(10, _lvl / 10));
        }

        if (money - _upgradeCost < new BigInteger(0))
        {
            _isUpgradeble = false;
        }
        else
        {
            _isUpgradeble = true;
        }
        _upgradeButton.GetComponent<Button>().enabled = _isUpgradeble;
    }

    private IEnumerator SendMoney()
    {
        yield return new WaitForSeconds(_speed);

        int boost = 1;

        if (_gameController.ActiveBooster != null)
        {
            boost = _gameController.ActiveBooster.BoostForce;
        }

        if (_money - _sellPerTime >= 0)
        {
            _gameController.AddMoney(_sellPerTime * boost);
            _money -= _sellPerTime;
            
            _eggs[_eggIndex].GetComponent<Road>().Money = ShowMoney(_sellPerTime * boost);
            _eggs[_eggIndex].gameObject.SetActive(true);
            _eggs[_eggIndex].GetComponent<Animation>().Play();
            _eggs[_eggIndex].StartDisable();
            _eggIndex++;
        }
        else if (_money > 0)
        {
            _gameController.AddMoney(_money * boost);

            _eggs[_eggIndex].GetComponent<Road>().Money = ShowMoney(_money * boost);
            _eggs[_eggIndex].gameObject.SetActive(true);
            _eggs[_eggIndex].GetComponent<Animation>().Play();
            _eggs[_eggIndex].StartDisable();
            _eggIndex++;
            _money = 0;
        }
        StartCoroutine(SendMoney());

        if (_eggIndex >= _eggs.Length)
        {
            _eggIndex = 0;
        }
    }

    public void OpenUpgradeCard()
    {
        SellerCard sellerCard = _gameController.SellerCard.GetComponent<SellerCard>();

        _gameController._FadeBackground.gameObject.SetActive(true);
        _gameController._FadeBackground.Fade();
        sellerCard.gameObject.SetActive(true);
        sellerCard.OpenCard();
        sellerCard.SelfOpen();
    }

    private void Update()
    {
        if (_isBoost)
        {
            _boostTime += Time.deltaTime;
            if (_boostTime >= 1f)
            {
                _isBoost = false;
                _speed = 3f - (0.01f * _lvl);
                _boostTime = 0;
                foreach (Road item in _roads)
                {
                    item.SetSpeed(1);
                }
                foreach (Road item in _eggs)
                {
                    item.SetSpeed(1);
                }
            }
        }
    }

    public void BoostSpeedBySwipe(SwipeData swipeData)
    {
        if (swipeData.Direction == SwipeDirection.Right)
        {
            _speed = (3f - (0.01f * _lvl)) / 3;
            _isBoost = true;
            _boostTime = 0;
            foreach (Road item in _roads)
            {
                item.SetSpeed(3);
            }
            foreach (Road item in _eggs)
            {
                item.SetSpeed(3);
            }
        }
        
    }

    public void SendOnUpgrade(int upgrade)
    {
        OnUpgrade.Invoke(upgrade);
    }
}