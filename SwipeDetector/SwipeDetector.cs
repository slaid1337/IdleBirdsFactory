using System;
using UnityEngine;
using UnityEditor;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _place;

    [SerializeField] private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField] private float minDistanceForSwipe = 20f;

    private Rect _rect;

    private Vector3 _positionRect;

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Start()
    {
        GetComponent<RectTransform>().position = _place.GetComponent<RectTransform>().position;
        _positionRect = _camera.WorldToScreenPoint(GetComponent<RectTransform>().position);
    }

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        Rect rect = new Rect(new Vector2(_positionRect.x, _positionRect.y), new Vector2(10000,10000));
        
        _rect = rect;
        if (SwipeDistanceCheckMet() && rect.Contains(fingerDownPosition))
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
                Debug.Log(direction);
            }
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
                Debug.Log(direction);
            }
            fingerUpPosition = fingerDownPosition;
        }
        
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
        OnSwipe(swipeData);
    }

    void OnDrawGizmos()
    {
        
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
        DrawRect(_rect);

    }

    void DrawRect(Rect rect)
    {
        Gizmos.DrawWireCube(new Vector3(rect.center.x, rect.center.y, 0.01f), new Vector3(rect.size.x, rect.size.y, 0.01f));
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

