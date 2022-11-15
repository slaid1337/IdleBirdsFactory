using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public void Checker()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (rectTransform.anchoredPosition.y < 0)
        {
            rectTransform.anchoredPosition = new Vector2(0,0);
        }
    }
}
