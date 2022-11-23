using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;
using UnityEngine.Events;

public class BirdRout : MonoBehaviour
{
    [SerializeField] private Seller _seller;
    private GameController _gameController;
    public List<Cell> _cells;

    private float _scaleStep;
    private bool _isCharging;
    [SerializeField] private float _scalingSpeed;
    [SerializeField] private float _speedModifire;
    private UnityEngine.Vector3 _birdPosition;

    private Camera _mainCamera;

    public int BirdIndex;

    private Transform[] routes;

    public RectTransform pointA;
    public RectTransform pointB;

    [SerializeField] private float YSpeed;
    [SerializeField] private float YAmplitude;
    [SerializeField] private float Speed;
    [SerializeField] private float AmplitudeOffset;
    [SerializeField] private float Amplitude;
    private UnityEngine.Vector3 _newPosition;
    private UnityEngine.Vector2 _newOffset;
    private float _sinStep;
    private bool _isRight;

    private void Start()
    {

        YSpeed = 7.84f;
        YAmplitude = 31.3f;
        Speed = Random.Range(1.01f, 0.8f);
        AmplitudeOffset = 0.5f;
        Amplitude = 0.5f;

        _gameController = _seller._gameController;

        _cells = new List<Cell>();

        foreach (var cell in _gameController.Cells)
        {
            if (cell.BirdCount > 0)
            {
                _cells.Add(cell);
            }
        }

        _scaleStep = 0f;
        _isCharging = true;

        routes = _cells[0].Birds[BirdIndex - 1].GetComponent<Bird>().routes;

        _mainCamera = Camera.main;

        RectTransform g0 = routes[0].GetChild(0).GetComponent<RectTransform>();
        RectTransform g1 = routes[0].GetChild(1).GetComponent<RectTransform>();

        pointA = g0;
        pointB = g1;

    }

    private void Update()
    {
        float currentStep = _sinStep;
        _sinStep = Mathf.Sin(Time.time * Speed) * Amplitude + AmplitudeOffset;
        _newOffset = new UnityEngine.Vector2(0, Mathf.Sin(Time.time * YSpeed) * YAmplitude + YAmplitude);
        _newPosition = UnityEngine.Vector3.Lerp(pointA.anchoredPosition + _newOffset, pointB.anchoredPosition + _newOffset, _sinStep);

        bool currentDirecion = _isRight;

        if (currentStep > _sinStep)
        {
            _isRight = false;
        }
        else
        {
            _isRight = true;
        }

        if (currentDirecion != _isRight)
        {
            foreach (var cell in _cells)
            {
                if (cell._rectTransform.IsVisibleFrom(_mainCamera) && cell.BirdCount >= BirdIndex)
                {
                    Bird bird = cell.Birds[BirdIndex - 1].GetComponent<Bird>();
                    bird._rectTransform.localScale = new UnityEngine.Vector3(Mathf.Lerp( _isRight ? -1f : 1f , -0.6f, _scaleStep), Mathf.Lerp(1f, 0.6f, _scaleStep), 1f);
                    if (_isRight)
                        bird.OpenPopUp();
                }
            }
        }

        foreach (var cell in _cells)
        {
            if (cell._rectTransform.IsVisibleFrom(_mainCamera) && cell.BirdCount >= BirdIndex)
            { 
                cell.Birds[BirdIndex - 1].GetComponent<RectTransform>().anchoredPosition = _newPosition;
            }
        }
    }
}
