using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Get2XBooster : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();

#if UNITY_ANDROID
        _button.onClick.AddListener(AdmobAds.Instance.ShowRewarded2XBooster);
#endif

#if UNITY_WEBGL
        _button.onClick.AddListener(YandexSDK.Instance.ShowRewardAdvertismentBooster);
#endif
    }
}
