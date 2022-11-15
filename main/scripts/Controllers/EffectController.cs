using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    [SerializeField] private Text _itemText;
    [SerializeField] private GameObject _item;
    [SerializeField] private FadeBackground _fadeBackground;
    [SerializeField] private Animation _animation;

    public void OpenItem(Sprite sprite, string description)
    {
        _item.GetComponent<Image>().sprite = sprite;
        _itemText.text = description;
        _animation.Play();
        StartCoroutine(CloseCoroutine());
    }

    private IEnumerator CloseCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _fadeBackground.Close();
        gameObject.SetActive(false);
    }
}
