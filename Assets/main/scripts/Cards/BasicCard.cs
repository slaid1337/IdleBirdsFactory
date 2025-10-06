using UnityEngine;

public abstract class BasicCard : MonoBehaviour
{
    [SerializeField] protected GameController _gameController;

    protected float _height;
    protected float _switchValue;
    protected bool _switcher;
    protected RectTransform _rectTransform;

    public void OpenCard()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _height = (_gameController.MainCanvas.GetComponent<RectTransform>().sizeDelta.y / 2) + (_rectTransform.sizeDelta.y / 2);
        _switchValue = 0;
        _switcher = true;
        _rectTransform.anchoredPosition = new Vector3(0, -_height, 0);
    }

    protected void OpenningCard()
    {
        if (_switcher)
        {
            _switchValue += Time.deltaTime * 1.75f;
            _rectTransform.anchoredPosition = Vector3.Lerp(new Vector3(0, -_height, 0), Vector3.zero, Mathf.SmoothStep(0.0f, 1.0f, Mathf.Min(_switchValue, 1f)));
            if (_switchValue >= 1f)
            {
                _switcher = false;
                _switchValue = 0;
            }
        }
    }
}