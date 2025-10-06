using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class Bird : ControllerBase
{
    private readonly WaitForSeconds _waitOneSecond = new WaitForSeconds(1f);
    private readonly WaitForSeconds _waitTextPopup = new WaitForSeconds(1f);
    
    [SerializeField] private Seller _seller;
    [SerializeField] private Cell _cell;
    private int _moneyPerSecond;
    private Sprite _image;
    private Sprite _image2;
    private Sprite _image3;
    private float _animationTimer;
    [SerializeField] private float _animationSpeed;
    private bool _animationSwitcher;

    public Transform[] routes;

    private int _routToGo;
    private float _tParam;
    public UnityEngine.Vector3 _birdPosition;
    [SerializeField] private float _speedModifire;
    private bool _coroutineAllowed;

    public RectTransform _rectTransform;
    private bool _isMovingLeft;
    private float _scaleStep;
    private bool _isCharging;
    [SerializeField] private float _scalingSpeed;

    [SerializeField] private Animation _animationColor;

    [SerializeField] private GameObject _popupText;

    public UnityEvent<int> OnTap = new UnityEvent<int>();

    private Image _sprite;
    private Camera _mainCamera;

    public Sprite Image
    {
        set
        {
            _image = value;
        }
    }

    public Sprite Image2
    {
        set
        {
            _image2 = value;
        }
    }

    public Sprite Image3
    {
        set
        {
            _image3 = value;
        }
    }

    private void Update()
    {
        if (_rectTransform.IsVisibleFrom(_mainCamera))
        {
            _animationTimer += Time.deltaTime;
            if (_animationTimer <= _animationSpeed && _animationSwitcher)
            {
                _sprite.sprite = _image;
                _animationSwitcher = false;
            }
            else if (_animationTimer >= _animationSpeed && _animationTimer <= _animationSpeed * 2 && !_animationSwitcher)
            {
                _sprite.sprite = _image2;
                _animationSwitcher = true;
            }
            else if (_animationTimer >= _animationSpeed * 2 && _animationTimer <= _animationSpeed * 3 && _animationSwitcher)
            {
                _sprite.sprite = _image3;
                _animationSwitcher = false;
            }
            else if (_animationTimer >= _animationSpeed * 3 && _animationTimer <= _animationSpeed * 4 && !_animationSwitcher)
            {
                _sprite.sprite = _image2;
                _animationTimer = 0;
                _animationSwitcher = true;
            }
        }
    }

    private void Start()
    {
        
        GetComponent<Image>().sprite = _image;
        _moneyPerSecond = 12 * ((int)Mathf.Pow(10, _cell.Stage) / 10);
        StartCoroutine(MakeMoney(Random.Range(0.2f , 1.0f)));
        _animationTimer = 0;
        _animationSwitcher = true;

        _routToGo = 0;
        _tParam = 0f;
        _coroutineAllowed = true;

        _rectTransform = GetComponent<RectTransform>();
        _scaleStep = 0f;
        _isCharging = true;
        _sprite = GetComponent<Image>();
        _mainCamera = Camera.main;
    }

    private IEnumerator MakeMoney(float waitTime)
    {
        yield return _waitOneSecond;
        _seller.AddMoney(_moneyPerSecond);
        StartCoroutine(MakeMoney(1f));
    }

    public void FastProduce()
    {
        _seller.AddMoney(_moneyPerSecond);
        _animationColor.Play();
        SendTap(1);
    }

    public void SendTap(int tap)
    {
        OnTap.Invoke(tap);
    }

    public void OpenPopUp()
    {
        _popupText.SetActive(true);
        _popupText.GetComponent<Text>().text = "+ " + _seller.ShowMoney(_moneyPerSecond);
        _popupText.GetComponent<Animation>().Play();
        StartCoroutine(CloseTextPopup());
    }

    public IEnumerator CloseTextPopup()
    {
        yield return _waitTextPopup;
        _popupText.SetActive(false);
    }
}