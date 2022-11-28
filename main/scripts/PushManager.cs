using Unity.Notifications.Android;

public class PushManager : Singletone<PushManager>
{
#if UNITY_ANDROID
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
        
        AndroidNotificationCenter.CancelAllScheduledNotifications();
    }

    public void SendNotification(string title, string text, double pushTimeMinutes)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddMinutes(pushTimeMinutes);

        AndroidNotificationCenter.SendNotification(notification, "main_channel");
    }
#endif
}