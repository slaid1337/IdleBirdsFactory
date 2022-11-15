using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private Animation _animation;
    private float _animationSpeed;
    [SerializeField] private Seller _seller;
    [HideInInspector]public string Money;

    private void Start()
    {
        SetSpeed(1);
    }

    public void SetSpeed(float speed)
    {
        _animationSpeed = (0.3f + 0.001f * _seller.Lvl) * speed;
        _animation = GetComponent<Animation>();
        foreach (AnimationState item in _animation)
        {
            item.speed = _animationSpeed;
        }
    }

    public void StartDisable()
    {
        StartCoroutine(Disable());
    }

    public IEnumerator Disable()
    {
        yield return new WaitForSeconds(10f);
        gameObject.SetActive(false);
    }
}
