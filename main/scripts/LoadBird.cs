using UnityEngine;
using UnityEngine.UI;

public class LoadBird : MonoBehaviour
{
    [SerializeField] private Image _birdImage;

    private float _animationTimer;
    [SerializeField] private float _animationSpeed;
    private bool _animationSwitcher;

    [SerializeField] private Sprite _image;
    [SerializeField] private Sprite _image2;
    [SerializeField] private Sprite _image3;
    
    void Update()
    {
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
}