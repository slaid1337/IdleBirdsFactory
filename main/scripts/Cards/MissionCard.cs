using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCard : BasicCard
{
    [SerializeField] private MissionCellController[] _missionCellControllers;

    private void Update()
    {
        OpenningCard();
    }

    public void SelfOpen()
    {
        foreach (var item in _missionCellControllers)
        {
            item.Refresh();
        }
    }
}
