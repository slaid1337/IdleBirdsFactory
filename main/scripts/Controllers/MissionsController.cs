using System.Collections;
using UnityEngine;

public class MissionsController : ControllerBase
{
    [SerializeField] private MissionObject[] _missionObjects;
    [SerializeField] private MissionCellController[] _missionCellControllers;

    [SerializeField] private GameController _gameController;

    [SerializeField] private GameObject _plusIcon;

    private bool _isTimeSending;
    private float _time;

    private int _currentCompliteQuests;

    public int CurrentCompliteQuestes
    {
        get
        {
            return _currentCompliteQuests;
        }
        set
        {
            _currentCompliteQuests = value;
        }
    }

    private void Start()
    {
        _currentCompliteQuests = 0;

        foreach (MissionObject item in _missionObjects)
        {
            if (item.CellIndex > 0)
            {
                _missionCellControllers[item.CellIndex - 1].Quest = item;
                
                if (item.currentValue >= item.Goal)
                {
                    _currentCompliteQuests++;
                    _missionCellControllers[item.CellIndex - 1].SetCollectable();
                }
                else
                {

                    Cell[] cells = _gameController.Cells;

                    switch (item.Missiontype)
                    {

                        case EnumMissions.tapBirds:
                            foreach (Cell cell in cells)
                            {
                                foreach (GameObject bird in cell.Birds)
                                {
                                    bird.GetComponent<Bird>().OnTap.AddListener(AddTap);
                                }
                            }
                            break;

                        case EnumMissions.UpgradeBirds:
                            foreach (Cell cell in cells)
                            {
                                cell.OnUpgrade.AddListener(AddUpgradeBirds);
                            }
                            break;

                        case EnumMissions.UpgradeSellingMachine:
                            _gameController._Seller.OnUpgrade.AddListener(AddUpgradeSeller);
                            break;

                        case EnumMissions.DoUpgrades:
                            _gameController._Seller.OnUpgrade.AddListener(AddUpgradeAll);
                            foreach (Cell cell in cells)
                            {
                                cell.OnUpgrade.AddListener(AddUpgradeAll);
                            }
                            break;

                        case EnumMissions.ReachLevel:
                            if (item.Goal <= _gameController.Lvl)
                            {
                                item.currentValue = item.Goal;
                                item.PrivateGoal = 1;
                            }
                            else
                                _gameController.OnAddLvl.AddListener(AddLvl);
                            break;

                        case EnumMissions.UnlockStage:
                            if (cells[item.Goal - 1].IsUnlocked)
                            {
                                item.currentValue = item.Goal;
                                item.PrivateGoal = 1;
                            }
                            else
                            {
                                foreach (Cell cell in cells)
                                {
                                    cell.OnUnlock.AddListener(AddStage);
                                }
                            }
                            break;

                        case EnumMissions.PlayMinutes:
                            _isTimeSending = true;
                            StartCoroutine(AddTime());
                            break;
                    }
                }
            }
        }

        if (CurrentCompliteQuestes > 0)
        {
            _plusIcon.SetActive(true);
        }
        else
        {
            _plusIcon.SetActive(false);
        }
    }

    private void Update()
    {
        if (_isTimeSending)
        {
            _time += Time.deltaTime;
        }
    }

    public void NewQuest(MissionObject missionObject, int cellIndex, int lvl)
    {
        _currentCompliteQuests--;

        int index = Random.Range(0,_missionObjects.Length - 1);

        for (int i = 0; _missionObjects[index].CellIndex != 0; i++)
        {
            index = Random.Range(0, _missionObjects.Length - 1);
        }

        missionObject.CellIndex = 0;
        _missionObjects[index].CellIndex = cellIndex;
        int isSkip = Random.Range(0, 10);

        _missionObjects[index].currentValue = 0;

        if (isSkip > 5)
        {
            _missionObjects[index].Skippable = true;
        }
        else
        {
            _missionObjects[index].Skippable = false;
        }

        Cell[] cells = _gameController.Cells;

        switch (_missionObjects[index].Missiontype)
        {
            case EnumMissions.DoUpgrades:
                _missionObjects[index].Goal = Random.Range(6, 12);
                _gameController._Seller.OnUpgrade.AddListener(AddUpgradeAll);
                foreach (Cell cell in cells)
                {
                    cell.OnUpgrade.AddListener(AddUpgradeAll);
                }
                break;

            case EnumMissions.PlayMinutes:
                _missionObjects[index].Goal = 15 * Mathf.Max(1, lvl % 10);
                _missionObjects[index].Skippable = true;
                _isTimeSending = true;
                StartCoroutine(AddTime());
                break;

            case EnumMissions.ReachLevel:
                _missionObjects[index].Goal = lvl + 3;
                _gameController.OnAddLvl.AddListener(AddLvl);
                break;

            case EnumMissions.tapBirds:
                _missionObjects[index].Goal = Random.Range(50, 200);
                foreach (Cell cell in cells)
                {
                    foreach (GameObject bird in cell.Birds)
                    {
                        bird.GetComponent<Bird>().OnTap.AddListener(AddTap);
                    }
                }
                break;

            case EnumMissions.UnlockStage:
                _missionObjects[index].Goal = (lvl / 5) + 2;
                foreach (Cell cell in cells)
                {
                    cell.OnUnlock.AddListener(AddStage);
                }
                break;

            case EnumMissions.UpgradeBirds:
                _missionObjects[index].Goal = Random.Range(6, 12);
                foreach (Cell cell in cells)
                {
                    cell.OnUpgrade.AddListener(AddUpgradeBirds);
                }
                break;

            case EnumMissions.UpgradeSellingMachine:
                _missionObjects[index].Goal = Random.Range(10, 20);
                _gameController._Seller.OnUpgrade.AddListener(AddUpgradeSeller);
                break;

            case EnumMissions.WatchVideo:
                _missionObjects[index].Goal = Random.Range(1, 3);
                break;
        }

        _missionObjects[index].RefreshDescription();

        _missionCellControllers[cellIndex - 1].Quest = _missionObjects[index];

        if (CurrentCompliteQuestes > 0)
        {
            _plusIcon.SetActive(true);
        }
        else
        {
            _plusIcon.SetActive(false);
        }
    }

    public void NewQuest(int cellIndex, int lvl)
    {
        _currentCompliteQuests--;

        int index = Random.Range(0, _missionObjects.Length - 1);

        for (int i = 0; _missionObjects[index].CellIndex != 0; i++)
        {
            index = Random.Range(0, _missionObjects.Length - 1);
        }

        _missionObjects[index].CellIndex = cellIndex;
        int isSkip = Random.Range(0, 10);

        _missionObjects[index].currentValue = 0;

        if (isSkip > 5)
        {
            _missionObjects[index].Skippable = true;
        }
        else
        {
            _missionObjects[index].Skippable = false;
        }

        Cell[] cells = _gameController.Cells;

        switch (_missionObjects[index].Missiontype)
        {
            case EnumMissions.DoUpgrades:
                _missionObjects[index].Goal = Random.Range(6, 12);
                _gameController._Seller.OnUpgrade.AddListener(AddUpgradeAll);
                foreach (Cell cell in cells)
                {
                    cell.OnUpgrade.AddListener(AddUpgradeAll);
                }
                break;

            case EnumMissions.PlayMinutes:
                _missionObjects[index].Goal = 15 * Mathf.Max(1, lvl % 10);
                _missionObjects[index].Skippable = true;

                StartCoroutine(AddTime());
                break;

            case EnumMissions.ReachLevel:
                _missionObjects[index].Goal = lvl + 3;
                _gameController.OnAddLvl.AddListener(AddLvl);
                break;

            case EnumMissions.tapBirds:
                _missionObjects[index].Goal = Random.Range(50, 200);
                foreach (Cell cell in cells)
                {
                    foreach (GameObject bird in cell.Birds)
                    {
                        bird.GetComponent<Bird>().OnTap.AddListener(AddTap);
                    }
                }
                break;

            case EnumMissions.UnlockStage:
                _missionObjects[index].Goal = (lvl / 5) + 2;
                foreach (Cell cell in cells)
                {
                    cell.OnUnlock.AddListener(AddStage);
                }
                break;

            case EnumMissions.UpgradeBirds:
                _missionObjects[index].Goal = Random.Range(6, 12);
                foreach (Cell cell in cells)
                {
                    cell.OnUpgrade.AddListener(AddUpgradeBirds);
                }
                break;

            case EnumMissions.UpgradeSellingMachine:
                _missionObjects[index].Goal = Random.Range(10, 20);
                _gameController._Seller.OnUpgrade.AddListener(AddUpgradeSeller);
                break;

            case EnumMissions.WatchVideo:
                _missionObjects[index].Goal = Random.Range(1, 3);
                break;
        }

        _missionObjects[index].RefreshDescription();

        _missionCellControllers[cellIndex - 1].Quest = _missionObjects[index];

        if (CurrentCompliteQuestes > 0)
        {
            _plusIcon.SetActive(true);
        }
        else
        {
            _plusIcon.SetActive(false);
        }
    }

    public void AddTap(int tap)
    {
        foreach (var item in _missionObjects)
        {
            if (item.Missiontype == EnumMissions.tapBirds)
            {
                item.currentValue += tap;
                if (item.currentValue >= item.Goal)
                {
                    Cell[] cells = _gameController.Cells;
                    foreach (Cell cell in cells)
                    {
                        foreach (GameObject bird in cell.Birds)
                        {
                            bird.GetComponent<Bird>().OnTap.RemoveListener(AddTap);
                        }
                    }
                    _missionCellControllers[item.CellIndex - 1].SetCollectable();

                    _currentCompliteQuests++;

                    
                    _plusIcon.SetActive(true);
                }
                break;
            }
        }
    }

    public void AddUpgradeBirds(int upgrade)
    {
        foreach (var item in _missionObjects)
        {
            if (item.Missiontype == EnumMissions.UpgradeBirds)
            {
                item.currentValue += upgrade;
                if (item.currentValue >= item.Goal)
                {
                    _missionCellControllers[item.CellIndex - 1].SetCollectable();

                    _currentCompliteQuests++;

                    Cell[] cells = _gameController.Cells;
                    foreach (Cell cell in cells)
                    {
                        cell.OnUpgrade.RemoveListener(AddUpgradeBirds);
                    }
                    _plusIcon.SetActive(true);
                }
            }
        }
    }

    public void AddUpgradeSeller(int upgrade)
    {
        foreach (var item in _missionObjects)
        {
            if (item.Missiontype == EnumMissions.UpgradeSellingMachine)
            {
                item.currentValue += upgrade;
                if (item.currentValue >= item.Goal)
                {
                    _missionCellControllers[item.CellIndex - 1].SetCollectable();

                    _currentCompliteQuests++;

                    _gameController._Seller.OnUpgrade.RemoveListener(AddUpgradeSeller);
                    _plusIcon.SetActive(true);
                }
            }
        }
    }

    public void AddUpgradeAll(int upgrade)
    {
        foreach (var item in _missionObjects)
        {
            if (item.Missiontype == EnumMissions.DoUpgrades)
            {
                item.currentValue += upgrade;
                if (item.currentValue >= item.Goal)
                {
                    _missionCellControllers[item.CellIndex - 1].SetCollectable();

                    _currentCompliteQuests++;

                    _gameController._Seller.OnUpgrade.RemoveListener(AddUpgradeAll);

                    Cell[] cells = _gameController.Cells;
                    foreach (Cell cell in cells)
                    {
                        cell.OnUpgrade.RemoveListener(AddUpgradeAll);
                    }
                    _plusIcon.SetActive(true);
                }
            }
        }
    }

    public void AddLvl(int lvl)
    {
        foreach (var item in _missionObjects)
        {
            if (item.Missiontype == EnumMissions.ReachLevel)
            {
                item.currentValue = lvl;

                if (item.currentValue >= item.Goal)
                {
                    _missionCellControllers[item.CellIndex - 1].SetCollectable();

                    _currentCompliteQuests++;

                    _gameController._Seller.OnUpgrade.RemoveListener(AddLvl);
                    _plusIcon.SetActive(true);
                }
            }
        }
    }

    public void AddStage(int stage)
    {
        foreach (var item in _missionObjects)
        {
            if (item.Missiontype == EnumMissions.UnlockStage)
            {
                item.currentValue = stage;

                if (item.currentValue >= item.Goal)
                {
                    _missionCellControllers[item.CellIndex - 1].SetCollectable();

                    _currentCompliteQuests++;

                    Cell[] cells = _gameController.Cells;
                    foreach (Cell cell in cells)
                    {
                        cell.OnUpgrade.RemoveListener(AddStage);
                    }
                    _plusIcon.SetActive(true);
                }
            }
        }
    }

    public IEnumerator AddTime()
    {
        yield return new WaitForSeconds(60f);

        if (_isTimeSending)
        {
            foreach (var item in _missionObjects)
            {
                if (item.Missiontype == EnumMissions.PlayMinutes)
                {
                    item.currentValue += (int) _time / 60;
                    _time = 0;
                    if (item.currentValue >= item.Goal)
                    {
                        _isTimeSending = false;
                        _plusIcon.SetActive(true);
                    }
                }
            }

            StartCoroutine(AddTime());
        }
    }
}