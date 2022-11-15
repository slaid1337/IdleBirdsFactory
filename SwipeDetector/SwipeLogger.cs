using UnityEngine;

public class SwipeLogger : MonoBehaviour
{
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private GameObject _hand_shop;
    private void Awake()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }

    private void Start()
    {
        if (_tutorial._isTutor)
        {
            _tutorial._hand_swipe.SetActive(true);
        }
    }

    private void SwipeDetector_OnSwipe(SwipeData data)
    {
        if (_tutorial._isTutor)
        {
            _tutorial._hand_swipe.SetActive(false);
            _hand_shop.SetActive(true);
        }
        SwipeDetector.OnSwipe -= SwipeDetector_OnSwipe;
    }
}