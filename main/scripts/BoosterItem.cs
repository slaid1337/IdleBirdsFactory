using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BoosterItem : MonoBehaviour
{
    [SerializeField] private GameObject _button;
    public GameObject ButtonFade;
    public BoosterObject _BoosterObject;
    [SerializeField] private GameController _gameController;
    [SerializeField] private Text _countText;

    private void Awake()
    {
        _button.GetComponent<Button>().onClick.AddListener(Activate);
        Refresh();
    }

    public void Activate()
    {
        _gameController.ActiveBooster = _BoosterObject;
        _gameController.BoostersContainer.Remove(_BoosterObject);
        _gameController.EndTimeEffectBosster = DateTime.Now.AddHours(_BoosterObject.Duration);
        _gameController.BoosterCard.GetComponent<BoosterCard>().Refresh();

        Refresh();
    }

    public void Refresh()
    {
        int count = 0;

        foreach (var item in _gameController.BoostersContainer)
        {
            if (item == _BoosterObject)
            {
                count++;
            }
        }

        if (count == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _countText.text = count.ToString() + " left";
        }
    }
}
