using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Objects/QuestData")]
public class MissionObject : ScriptableObject
{
    public string Description1;
    public string Description2;
    public string Description;
    public EnumMissions Missiontype;
    public int Profit;
    public int Goal;
    public int currentValue;
    public bool Skippable;
    public int CellIndex;
    public int PrivateGoal;

    private void Awake()
    {
        RefreshDescription();
    }

    public void RefreshDescription()
    {
        if (Missiontype == EnumMissions.UnlockStage || Missiontype == EnumMissions.ReachLevel)
        {
            PrivateGoal = 1;
        }
        else
        {
            PrivateGoal = Goal;
        }
        
        Description = Description1 + " " + Goal + " " + Description2;
    }
}