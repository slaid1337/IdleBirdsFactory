using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildMarketSettings", menuName = "Objects/BuildMarketSettings")]
public class BuildMarketSettings : ScriptableObject
{
    public MarketType marketType;
}

public enum MarketType
{
    PlayMarket,
    AppGallery,
    YandexGames
}
