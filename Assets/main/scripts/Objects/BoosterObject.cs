using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BoosterData", menuName = "Objects/BoosterData")]
public class BoosterObject : ScriptableObject
{
    public BoosterType TypeBooster;
    public string Description;
    public int BoostForce;
    public int Duration;
    public int Cost;
    public Sprite BoostImage;
    public int number;
}