using System.Collections;
using UnityEngine;
using TMPro;

public class SellerEggs : MonoBehaviour
{
    [SerializeField] private GameObject _eggText;
    private Animation _textAnimation;
    private TextMeshProUGUI _text;

    private void Start()
    {
        _textAnimation = _eggText.GetComponent<Animation>();
        _text = _eggText.GetComponent<TextMeshProUGUI>();
    }

    public void ShowProfit()
    {
        _eggText.SetActive(true);
        _text.text = "<sprite=0>+" + GetComponent<Road>().Money;
        
        _textAnimation.Play();

        StartCoroutine(DisableText());
    }

    private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(1f);

        _eggText.SetActive(false);
    }
}