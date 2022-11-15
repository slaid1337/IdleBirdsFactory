using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBackground : MonoBehaviour
{
    private float _fadeStep;
    private bool _isFade;
    private Image _image;
    [SerializeField] private float _fadeSpeed;

    private void Awake()
    {
        _image = gameObject.GetComponent<Image>();
        _isFade = false;
    }

    private void Update()
    {
        if (_isFade)
        {
            _fadeStep += Time.deltaTime * _fadeSpeed;
            _image.color = new Color(0, 0, 0, Mathf.Lerp(0f, 0.6f, _fadeStep));
            if (_fadeStep >= 1f)
            {
                _fadeStep = 1f;
                _isFade = false;
            }
        }
    }

    public void Fade()
    {
        _fadeStep = 0f;
        _isFade = true;
    }

    public void Close()
    {
        _fadeStep = 0f;
        _image.color = new Color(0, 0, 0, 0);
        gameObject.SetActive(false);
    }

    public void Open()
    {
        _fadeStep = 0f;
        _image.color = new Color(0, 0, 0, 0.6f);
    }
}
