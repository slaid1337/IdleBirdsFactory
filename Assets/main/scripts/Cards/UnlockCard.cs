using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockCard : BasicCard
{

    private bool _isUpgradeble;
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private Image _birdImage;
    [SerializeField] private TextMeshProUGUI _buttonText;

    private float _animationTimer;
    [SerializeField] private float _animationSpeed;
    private bool _animationSwitcher;

    [SerializeField] private Sprite _image;
    [SerializeField] private Sprite _image2;
    [SerializeField] private Sprite _image3;

    private Cell _cell;

    public void Update()
    {
        OpenningCard();

        _animationTimer += Time.deltaTime;
        if (_animationTimer <= _animationSpeed && _animationSwitcher)
        {
            _birdImage.sprite = _image;
            _animationSwitcher = false;
        }
        else if (_animationTimer >= _animationSpeed && _animationTimer <= _animationSpeed * 2 && !_animationSwitcher)
        {
            _birdImage.sprite = _image2;
            _animationSwitcher = true;
        }
        else if (_animationTimer >= _animationSpeed * 2 && _animationTimer <= _animationSpeed * 3 && _animationSwitcher)
        {
            _birdImage.sprite = _image3;
            _animationSwitcher = false;
        }
        else if (_animationTimer >= _animationSpeed * 3 && _animationTimer <= _animationSpeed * 4 && !_animationSwitcher)
        {
            _birdImage.sprite = _image2;
            _animationTimer = 0;
            _animationSwitcher = true;
        }
    }

    public void SelfOpen(Cell cell)
    {
        _cell = cell;

        _gameController.OnChangeMoney.AddListener(CheckOnUpgradeCard);

        _buttonText.text = "<sprite=0>" + _gameController.ShowMoney(cell.UpgradeCost);

        _upgradeButton.GetComponent<Button>().onClick.AddListener(cell.Unlock);
    }
    public void CloseCard()
    {
        _upgradeButton.GetComponent<Button>().onClick.RemoveListener(_cell.Unlock);
        _gameController.OnChangeMoney.RemoveListener(CheckOnUpgradeCard);
    }

    private void CheckOnUpgradeCard(int money)
    {
        _isUpgradeble = (money - _cell.UpgradeCost) >= 0;
        _upgradeButton.GetComponent<Button>().enabled = _isUpgradeble;
    }
}