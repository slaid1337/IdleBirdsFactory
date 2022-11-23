using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class SaveController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    [SerializeField] private BoosterObject[] _allBoosters;

    private CellsSave[] _cellsSaver;

    private bool _isReset = false;
    private bool _isSaveable;

    public CellsSave[] CellsSave
    {
        get
        {
            return _cellsSaver;
        }
    }

    private void Awake()
    {

        try
        {
            _cellsSaver = LoadCells();
        }
        catch
        {

        }

        try
        {
            _gameController.OnAddLvl.AddListener(SaveOnLvlUp);
        }
        catch
        {

        }

        _isSaveable = false;
    }

    private void Start()
    {
        _isSaveable = true;
    }

    private void SaveOnLvlUp(int number)
    {
        SaveAll();
    }

    public GameControllerSave LoadGameController()
    {
        if (!File.Exists(GetSavePath() + "/GameControllerSave.json"))
        {
            GameControllerSave gameControllerSave = new GameControllerSave("120", 50, new int[0], 1, new Dictionary<int, DateTime> { });
            SaveGameController(gameControllerSave);
        }
        
        string json = File.ReadAllText(GetSavePath() + "/GameControllerSave.json");
        GameControllerSave _saveOfGameController = JsonConvert.DeserializeObject<GameControllerSave>(json);
        Debug.Log(_saveOfGameController.Money);
        return _saveOfGameController;
    }

    public CellsSave[] LoadCells()
    {
        if (!File.Exists(GetSavePath() + "/CellsSave.json"))
        {
            CellsSave[] cellsSave = new CellsSave[_gameController.Cells.Length];

            for (int i = 0; i < cellsSave.Length; i++)
            {
                string name = "";
                int count = 0;
                bool isUnlocked = false;
                int stage = _gameController.Cells[i].transform.GetSiblingIndex() + 1;

                cellsSave[i] = new CellsSave(isUnlocked, count, name, stage);
            }
            SaveCells(cellsSave);
        }
        string json = File.ReadAllText(GetSavePath() + "/CellsSave.json");
        CellsSave[] _saveOfCells = JsonConvert.DeserializeObject<CellsSave[]>(json);

        return _saveOfCells;
    }

    public SellerSave LoadSeller()
    {
        if (!File.Exists(GetSavePath() + "/SellerSave.json"))
        {
            SellerSave sellerSave = new SellerSave("0", 1);
            SaveSeller(sellerSave);
        }
        string json = File.ReadAllText(GetSavePath() + "/SellerSave.json");
        SellerSave _saveOfSeller = JsonConvert.DeserializeObject<SellerSave>(json);

        return _saveOfSeller;
    }

    public OfflineSave LoadOfflineDate()
    {
        if (!File.Exists(GetSavePath() + "/OfflineSave.json"))
        {
            OfflineSave offlineSave = new OfflineSave(DateTime.Now);
            SaveOfflineDate(offlineSave);
        }
        string json = File.ReadAllText(GetSavePath() + "/OfflineSave.json");
        OfflineSave _offlineSave = JsonConvert.DeserializeObject<OfflineSave>(json);

        return _offlineSave;
    }

    public StatisticSave LoadStatistic()
    {
        if (!File.Exists(GetSavePath() + "/StatsSave.json"))
        {
            StatisticSave statisticSave = new StatisticSave(0);
            SaveStats(statisticSave);
        }
        string json = File.ReadAllText(GetSavePath() + "/StatsSave.json");
        StatisticSave _statisticSave = JsonConvert.DeserializeObject<StatisticSave>(json);

        return _statisticSave;
    }

    public ServiceSave LoadService()
    {
        if (!File.Exists(GetSavePath() + "/ServiceSave.json"))
        {
            ServiceSave save = new ServiceSave(false);
            SaveService(save);
        }
        string json = File.ReadAllText(GetSavePath() + "/ServiceSave.json");
        ServiceSave _save = JsonConvert.DeserializeObject<ServiceSave>(json);

        return _save;
    }

    public void SaveService(ServiceSave item)
    {
        File.WriteAllText(GetSavePath() + "/ServiceSave.json", JsonConvert.SerializeObject(item));
    }

    public void SaveGameController(GameControllerSave item)
    {
        File.WriteAllText(GetSavePath() + "/GameControllerSave.json", JsonConvert.SerializeObject(item));
    }

    public void SaveCells(CellsSave[] item)
    {
        File.WriteAllText(GetSavePath() + "/CellsSave.json", JsonConvert.SerializeObject(item));
    }

    public void SaveSeller(SellerSave item)
    {
        File.WriteAllText(GetSavePath() + "/SellerSave.json", JsonConvert.SerializeObject(item));
    }

    public void SaveOfflineDate(OfflineSave item)
    {
        File.WriteAllText(GetSavePath() + "/OfflineSave.json", JsonConvert.SerializeObject(item));
    }

    public void SaveStats(StatisticSave item)
    {
        File.WriteAllText(GetSavePath() + "/StatsSave.json", JsonConvert.SerializeObject(item));
    }

    [ContextMenu("SaveGame")]
    private void OnApplicationPause()
    {
        if (_isSaveable)
        {
            SaveAll();
        }

        PushManager.Instance.SendNotification("Idle Birds Factory", "Enter and get a new Bird!", 30d);
        PushManager.Instance.SendNotification("Idle Birds Factory", "you haven't played the game for hour", 60d);
        PushManager.Instance.SendNotification("Idle Birds Factory", "you haven't played the game for 6 hour", 360d);
        PushManager.Instance.SendNotification("Idle Birds Factory", "you haven't played the game for day", 1440d);
        PushManager.Instance.SendNotification("Idle Birds Factory", "you haven't played the game for week", 10080d);
    }

    [ContextMenu("SaveGame1")]
    private void OnApplicationQuit()
    {
        SaveAll();
    }

    [ContextMenu("ResetSaves")]
    public void ResetAllSaves()
    {
        GameControllerSave gameControllerSave = new GameControllerSave("120", 50, new int[0], 1, new Dictionary<int, DateTime> { });

        CellsSave[] cellsSave = new CellsSave[90];

        for (int i = 0; i < cellsSave.Length; i++)
        {
            string name = "";
            int count = 0;
            bool isUnlocked = false;
            int stage = i + 1;

            cellsSave[i] = new CellsSave(isUnlocked, count, name, stage);
        }

        SellerSave sellerSave = new SellerSave("0", 1);

        OfflineSave offlineSave = new OfflineSave(DateTime.Now);

        StatisticSave statisticSave = new StatisticSave(0);

        SaveGameController(gameControllerSave);
        SaveCells(cellsSave);
        SaveSeller(sellerSave);
        SaveOfflineDate(offlineSave);
        SaveStats(statisticSave);

        _gameController._servicesManager.SaveToCloud(SaverType.GameController, "GameControllerSave");
        _gameController._servicesManager.SaveToCloud(SaverType.Cells, "CellsSave");
        _gameController._servicesManager.SaveToCloud(SaverType.Seller, "SellerSave");
        _gameController._servicesManager.SaveToCloud(SaverType.Stats, "StatsSave");

        _isReset = true;

        Debug.Log("reset");
    }

    public void SaveAll()
    {
        if (!_isReset && _gameController != null)
        {
            int[] nums = new int[_gameController.BoostersContainer.Count];

            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = _gameController.BoostersContainer[i].number;
            }

            GameControllerSave gameControllerSave = new GameControllerSave(_gameController.Money.ToString(),
                _gameController.Cash,
                nums,
                _gameController.Lvl,
                _gameController.ActiveBooster != null ? new Dictionary<int, DateTime> { { _gameController.ActiveBooster.number, _gameController.EndTimeEffectBosster } } : new Dictionary<int, DateTime> { });

            CellsSave[] cellsSave = new CellsSave[_gameController.Cells.Length];

            for (int i = 0; i < cellsSave.Length; i++)
            {
                string name = _gameController.Cells[i]._BirdObject == null ? "" : _gameController.Cells[i]._BirdObject.Name;
                int count = _gameController.Cells[i].BirdCount;
                bool isUnlocked = _gameController.Cells[i].IsUnlocked;
                int stage = _gameController.Cells[i].transform.GetSiblingIndex() + 1;

                cellsSave[i] = new CellsSave(isUnlocked, count, name, stage);
            }

            SellerSave sellerSave = new SellerSave(_gameController._Seller.Money.ToString(), _gameController._Seller.Lvl);

            OfflineSave offlineSave = LoadOfflineDate();
            if ((offlineSave.DateOfQuit - DateTime.Now).TotalSeconds > 10)
                offlineSave = new OfflineSave(DateTime.Now);

            StatisticSave statisticSave = new StatisticSave(LoadStatistic().CountOfGameStarts + 1);

            SaveGameController(gameControllerSave);
            SaveCells(cellsSave);
            SaveSeller(sellerSave);
            SaveOfflineDate(offlineSave);
            SaveStats(statisticSave);

            _gameController._servicesManager.SaveToCloud(SaverType.GameController, "GameControllerSave");
            _gameController._servicesManager.SaveToCloud(SaverType.Cells, "CellsSave");
            _gameController._servicesManager.SaveToCloud(SaverType.Seller, "SellerSave");
            _gameController._servicesManager.SaveToCloud(SaverType.Stats, "StatsSave");
            Debug.Log("Save");
        }
    }

    private string GetSavePath()
    {
        string saveDirectory = Application.persistentDataPath;

        return saveDirectory;
    }

    public void SaveAllFromCloud(GameControllerSave gameControllerSave, CellsSave[] cellsSave, SellerSave sellerSave, StatisticSave statisticSave, ServiceSave serviceSave)
    {

        OfflineSave offlineSave = new OfflineSave(DateTime.Now);

        SaveGameController(gameControllerSave);
        SaveCells(cellsSave);
        SaveSeller(sellerSave);
        SaveOfflineDate(offlineSave);
        SaveStats(statisticSave);
        SaveService(serviceSave);
    }
}

[Serializable]
public class GameControllerSave : SaveBase
{
    public string Money;
    public int Cash;
    public int[] BoostersNumbers;
    public int Level;
    public Dictionary<int, DateTime> ActiveBooster;

    public GameControllerSave(string money, int cash, int[] numbers, int level, Dictionary<int, DateTime> activeBooster)
    {
        Money = money;
        Cash = cash;
        BoostersNumbers = numbers;
        Level = level;
        ActiveBooster = activeBooster;
    }
}

[Serializable]
public class CellsSave : SaveBase
{
    public string BirdName;
    public int CountOfBirds;
    public bool IsUnlocked;
    public int Stage;

    public CellsSave(bool isUnlocked, int birdCount, string nameBird, int stage)
    {
        BirdName = nameBird;
        CountOfBirds = birdCount;
        IsUnlocked = isUnlocked;
        Stage = stage;
    }
}

[Serializable]
public class SellerSave : SaveBase
{
    public string Money;
    public int Lvl;

    public SellerSave(string money, int lvl)
    {
        Money = money;
        Lvl = lvl;
    }
}

[Serializable]
public class OfflineSave : SaveBase
{
    public DateTime DateOfQuit;

    public OfflineSave(DateTime date)
    {
        DateOfQuit = date;
    }
}

[Serializable]
public class StatisticSave : SaveBase
{
    public int CountOfGameStarts;

    public StatisticSave(int starts)
    {
        CountOfGameStarts = starts;
    }
}

[Serializable]
public class ServiceSave : SaveBase
{
    public bool AdBlock;

    public ServiceSave(bool block)
    {
        AdBlock = block;
    }
}

public abstract class SaveBase
{
    public virtual void Print()
    {
        Debug.Log("ass");
    }
}