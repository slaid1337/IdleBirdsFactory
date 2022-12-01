using UnityEngine;
using UnityEngine.UI;

public class MissionCellController : MonoBehaviour
{
    public MissionObject Quest;

    [SerializeField] private GameController _gameController;
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private Text _progressText;
    [SerializeField] private Text _moneyText;
    [SerializeField] private Text _descriptoin;
    [SerializeField] private GameObject _collectBtn;
    [SerializeField] private GameObject _adsBtn;
    private bool _IsCollectable;
    [SerializeField] private int _cellIndex;
    [SerializeField] private Vector2 _startSizeBar;
    private int _goal;
    private int _currentValue;

#if UNITY_ANDROID
    private AdmobAds _ads = null;
#endif

    public void Refresh()
    {
        if (Quest == null)
        {
            CreateNewQuest();
        }

        if (Quest.PrivateGoal == Quest.currentValue)
        {
            SetCollectable();
            return;
        }

        _descriptoin.text = Quest.Description;
        _moneyText.text = Quest.Profit.ToString();
        float barIndex = 0f;
        if (Quest.Missiontype == EnumMissions.UnlockStage || Quest.Missiontype == EnumMissions.ReachLevel)
        {
            _goal = 1;
            if (Quest.currentValue >= Quest.Goal)
            {
                _currentValue = 1;
            }
            else
            {
                _currentValue = 0;
            }
            barIndex = Mathf.Min((float)Normalize((double)_currentValue, 0, (double)_goal, 0, 1), 1f);
        }
        else
        {
            _goal = Quest.PrivateGoal;
            _currentValue = Quest.currentValue;
            barIndex = Mathf.Min((float)Normalize((double)_currentValue, 0, (double)Quest.Goal, 0, 1), 1f);
        }
        _progressText.text = _currentValue + "/" + _goal;

        _progressBar.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, _startSizeBar.y), _startSizeBar, barIndex);

        if (_IsCollectable)
        {
            _collectBtn.GetComponent<Button>().interactable = true;
            _adsBtn.SetActive(false);
        }
        else
        {
            _collectBtn.GetComponent<Button>().interactable = false;

            if (!Quest.Skippable)
            {
                _adsBtn.SetActive(false);
            }
            else
            {
                _adsBtn.SetActive(true);
            }
        }
    }

    public void SetCellToAds()
    {
#if UNITY_ANDROID
        _ads = AdmobAds.Instance;
        _ads._missionCellController = this;
#endif
    }

    double Normalize(double val, double valmin, double valmax, double min, double max)
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }

    public void Collect()
    {
        _gameController.AddCash(Quest.Profit);

        _collectBtn.GetComponent<Button>().onClick.RemoveListener(Collect);
        _collectBtn.GetComponent<Button>().interactable = false;

        _gameController.missionsController.NewQuest(Quest, _cellIndex, _gameController.Lvl);

        _gameController.missionsController.CurrentCompliteQuestes--;
        _IsCollectable = false;

        _gameController.MissionCard.SetActive(false);

        _gameController.CollectEffect.SetActive(true);
        _gameController.CollectEffect.GetComponent<EffectController>().OpenItem(_gameController._goldenEgg, "+ " + Quest.Profit);
    }

    public void SetCollectable()
    {
        _collectBtn.GetComponent<Button>().onClick.AddListener(Collect);
        _IsCollectable = true;
        Refresh();
    }

    private void CreateNewQuest()
    {
        _gameController.missionsController.NewQuest(_cellIndex, _gameController.Lvl);
    }
}