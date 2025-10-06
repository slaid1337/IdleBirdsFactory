using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID
using Firebase.Analytics;
#endif

public class LevelController : ControllerBase
{
    [SerializeField] private int _lvl;
    [SerializeField] private Text _lvlText;
    [SerializeField] private GameObject _barObject;
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private GameObject _lvlUpButton;
    [SerializeField] private GameController _gameController;
    private int _costToUp;
    private Vector2 _startSizeBar;

    private void Start()
    {
        _startSizeBar = _progressBar.GetComponent<RectTransform>().sizeDelta;
        _lvl = _gameController.Lvl;
        _gameController.OnAddMoney.AddListener(CheckOnUpgrade);
        CheckOnUpgrade(0);
        Debug.Log(_costToUp);
    }

    public void LevelUp()
    {
        _lvl++;
        _gameController.LvlUp();
        _money = 0;
        _barObject.SetActive(true);
        _lvlUpButton.SetActive(false);
        CheckOnUpgrade(0);
        RefreshText();

        _gameController._FadeBackground.gameObject.SetActive(true);
        _gameController._FadeBackground.Fade();
        _gameController.LevelCard.SetActive(true);
        _gameController.LevelCard.GetComponent<LevelCard>().OpenCard();
        _gameController.LevelCard.GetComponent<LevelCard>().SelfOpen(_gameController.MaxProfitPerHour);

#if UNITY_ANDROID
        FirebaseAnalytics.LogEvent("lvl_UP");
#endif
    }

    private void RefreshText()
    {
        _lvlText.text = "Level " + _lvl;
    }
    
    private void CheckOnUpgrade(int money)
    {
        _costToUp = (int)(25 * Mathf.Pow(1.5f, _lvl / 3f));
        _money += money;
        float barIndex = Mathf.Min((float)_money / _costToUp, 1f);
        _progressBar.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, _startSizeBar.y), _startSizeBar, barIndex);
        if (barIndex == 1f)
        {
            _barObject.SetActive(false);
            _lvlUpButton.SetActive(true);
        }
        RefreshText();
    }
}