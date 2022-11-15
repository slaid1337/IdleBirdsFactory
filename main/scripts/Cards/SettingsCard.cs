using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;

public class SettingsCard : BasicCard
{
    [SerializeField] private Button _serviceButton;
    [SerializeField] private Text _serviceText;
    [SerializeField] private Color[] colors;

    private void Update()
    {
        OpenningCard();
    }

    public void SelfOpen()
    {
        //bool isLogged = PlayGamesPlatform.Instance.IsAuthenticated();

        bool isLogged = false;

        //if (isLogged)
        //{
        //    _serviceButton.GetComponent<Image>().color = colors[0];
        //    _serviceText.text = "Connected";
        //    _serviceButton.onClick.AddListener(_gameController._servicesManager.LogOut);
        //}
        //else
        //{
        //    _serviceButton.GetComponent<Image>().color = colors[1];
        //    _serviceText.text = "Disconnected";
        //    _serviceButton.onClick.AddListener(_gameController._servicesManager.LogIn);
        //}
    }
}
