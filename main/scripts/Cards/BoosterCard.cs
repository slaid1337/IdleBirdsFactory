using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterCard : BasicCard
{
    [SerializeField] private Image _mainImage;
    [SerializeField] private Text _boostForceText;
    [SerializeField] private Text _timeText;
    [SerializeField] private Sprite _starImage;
    [SerializeField] private BoosterItem[] _boosterItems;
    [SerializeField] private Tutorial _tutorial;

    private void Update()
    {
        OpenningCard();
    }

    public void SelfOpen()
    {
        Refresh();

        foreach (var item in _boosterItems)
        {
            if (_gameController.BoostersContainer.Contains(item._BoosterObject))
            {
                item.gameObject.SetActive(true);
                item.Refresh();
            }
        }

        if (_tutorial._isTutor)
        {
            _tutorial._hand_card_booster.SetActive(true);
        }
    }

    public void Refresh()
    {
        if (_gameController.ActiveBooster != null)
        {
            _mainImage.sprite = _gameController.ActiveBooster.BoostImage;
            _boostForceText.text = "x" + _gameController.ActiveBooster.BoostForce.ToString();

            if ((_gameController.EndTimeEffectBosster - DateTime.Now).Days > 0)
            {
                _timeText.text = (_gameController.EndTimeEffectBosster - DateTime.Now).Days + "d";
            }
            else if ((_gameController.EndTimeEffectBosster - DateTime.Now).Hours > 0)
            {
                _timeText.text = (_gameController.EndTimeEffectBosster - DateTime.Now).Hours + "h";
            }
            else
            {
                _timeText.text = (_gameController.EndTimeEffectBosster - DateTime.Now).Minutes + "m";
            }

            foreach (var item in _boosterItems)
            {
                item.ButtonFade.SetActive(true);
            }
        }
        else
        {
            _mainImage.sprite = _starImage;
            _boostForceText.text = "x--";
            _timeText.text = "--";

            foreach (var item in _boosterItems)
            {
                item.ButtonFade.SetActive(false);
            }
        }
    }
}
