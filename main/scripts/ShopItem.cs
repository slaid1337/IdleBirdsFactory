using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _header;
    public BoosterObject _boosterObject;

    public void RefreshHeader(string text)
    {
        _header.text = text;
    }
}