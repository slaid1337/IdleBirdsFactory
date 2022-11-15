using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject _hand_unlock;
    public GameObject _hand_swipe;
    public GameObject _hand_shop;
    public GameObject _hand_buy_booster;
    public GameObject _hand_booster;
    public GameObject _hand_card_buy;
    public GameObject _hand_card_booster;
    public bool _isTutor;
    [SerializeField] private SaveController _saveController;

    public void OnEnd()
    {
        StatisticSave statisticSave = _saveController.LoadStatistic();
        if (statisticSave.CountOfGameStarts < 1)
        {
            _saveController.SaveStats(new StatisticSave(2));
            _isTutor = false;
        }
    }
}
