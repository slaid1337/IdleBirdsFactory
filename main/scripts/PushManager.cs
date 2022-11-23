using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class PushManager : Singletone<PushManager>
{

    private AndroidNotificationChannel _channel;

    void Start()
    {
        _channel = new AndroidNotificationChannel()
        {
            Id = "main_channel",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(_channel);
    }

    private void OnEnable()
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }

    public void SendNotification(string title, string text, double pushTimeMinutes)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddMinutes(pushTimeMinutes);

        AndroidNotificationCenter.SendNotification(notification, "main_channel");
    }

}
