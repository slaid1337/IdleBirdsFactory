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

    private float _tParam;
    private bool _coroutineAllowed;

    private int _routToGo;
    private bool _isMovingLeft;
    private float _scaleStep;
    private bool _isCharging;
    [SerializeField] private float _scalingSpeed;
    [SerializeField] private float _speedModifire;
    private UnityEngine.Vector3 _birdPosition;

    private Camera _mainCamera;

    public int BirdIndex;

    private Transform[] routes;

    private void Start()
    {
        _gameController = _seller._gameController;

        _cells = new List<Cell>();

        foreach (var cell in _gameController.Cells)
        {
            if (cell.BirdCount > 0)
            {
                _cells.Add(cell);
            }
        }

        _tParam = 0f;
        _coroutineAllowed = true;
        _speedModifire = Random.Range(0.8f,1f);

        _scaleStep = 0f;
        _isCharging = true;

        routes = _cells[0].Birds[BirdIndex - 1].GetComponent<Bird>().routes;

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute());
            _coroutineAllowed = false;
        }

        if (_isCharging)
        {
            _scaleStep += Time.deltaTime * _scalingSpeed;
        }
        else
        {
            _scaleStep -= Time.deltaTime * _scalingSpeed;
        }

        if (_scaleStep >= 1f)
        {
            _isCharging = false;
        }
        else if (_scaleStep <= 0)
        {
            _isCharging = true;
        }

        if (_isMovingLeft)
        {
            foreach (var cell in _cells)
            {
                if (cell._rectTransform.IsVisibleFrom(_mainCamera) && cell.BirdCount >= BirdIndex)
                {
                    cell.Birds[BirdIndex - 1].GetComponent<Bird>()._rectTransform.localScale = new UnityEngine.Vector3(Mathf.Lerp(1f, 0.6f, _scaleStep), Mathf.Lerp(1f, 0.6f, _scaleStep), 1f);
                }
            }
        }
        else
        {

            foreach (var cell in _cells)
            {
                if (cell._rectTransform.IsVisibleFrom(_mainCamera) && cell.BirdCount >= BirdIndex)
                {
                    cell.Birds[BirdIndex - 1].GetComponent<Bird>()._rectTransform.localScale = new UnityEngine.Vector3(Mathf.Lerp(-1f, -0.6f, _scaleStep), Mathf.Lerp(1f, 0.6f, _scaleStep), 1f);
                }
            }
        }
    }

    private IEnumerator GoByTheRoute()
    {
        int routeNumber = _routToGo;
        _coroutineAllowed = false;

        Transform g0 = routes[routeNumber].GetChild(0);
        Transform g1 = routes[routeNumber].GetChild(1);
        Transform g2 = routes[routeNumber].GetChild(2);
        Transform g3 = routes[routeNumber].GetChild(3);

        UnityEngine.Vector3 p0 = UnityEngine.Vector3.zero;
        UnityEngine.Vector3 p1 = UnityEngine.Vector3.zero;
        UnityEngine.Vector3 p2 = UnityEngine.Vector3.zero;
        UnityEngine.Vector3 p3 = UnityEngine.Vector3.zero;

        if (routeNumber == 0)
        {
            _isMovingLeft = false;
        }
        else
        {
            _isMovingLeft = true;
        }

        while (_tParam < 1)
        {
            p0 = g0.GetComponent<RectTransform>().anchoredPosition;
            p1 = g1.GetComponent<RectTransform>().anchoredPosition;
            p2 = g2.GetComponent<RectTransform>().anchoredPosition;
            p3 = g3.GetComponent<RectTransform>().anchoredPosition;

            _tParam += Time.deltaTime * _speedModifire;

            _birdPosition = Mathf.Pow(1 - _tParam, 3) * p0 +
                3 * Mathf.Pow(1 - _tParam, 2) * _tParam * p1 +
                3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * p2 +
                Mathf.Pow(_tParam, 3) * p3;

            foreach (var cell in _cells)
            {
                if (cell._rectTransform.IsVisibleFrom(_mainCamera) && cell.BirdCount >= BirdIndex)
                {
                    cell.Birds[BirdIndex - 1].GetComponent<RectTransform>().anchoredPosition = _birdPosition;
                }
            }

            yield return new WaitForEndOfFrame();
        }

        _tParam = 0f;
        _routToGo += 1;

        if (_routToGo > routes.Length - 1)
        {
            _routToGo = 0;
            foreach (var cell in _cells)
            {
                if (cell._rectTransform.IsVisibleFrom(_mainCamera) && cell.BirdCount >= BirdIndex)
                {
                    cell.Birds[BirdIndex - 1].GetComponent<Bird>().OpenPopUp();
                }
            }
        }

        _coroutineAllowed = true;
    }
}
