using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BirdData", menuName = "Objects/BirdData")]
public class BirdObject : ScriptableObject
{
    public string Name;
    public float Bonus;
    public Sprite Image;
    public Sprite Image2;
    public Sprite Image3;

    public bool IsTaken;

    public void SetTaken()
    {
        IsTaken = true;
    }
}
