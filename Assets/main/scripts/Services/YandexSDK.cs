using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexSDK : Singletone<YandexSDK>
{
#if UNITY_WEBGL
    UserGameData UGD;
    private UserData UD;

    public UserGameData GetUserGameData => UGD;

    public UserData GetUserData => UD;
    
    [DllImport("__Internal")]
    private static extern void Auth();    // Авторизация - внешняя функция для связи с плагином
    [DllImport("__Internal")]
    private static extern void ShowCommonADV();    // Показ обычной рекламы - внешняя функция для связи с плагином
    [DllImport("__Internal")]
    private static extern void GetData();    // Получение данных - внешняя функция для связи с плагином
    [DllImport("__Internal")]
    private static extern void SetData(string data);    // Отправка данных - внешняя функция для связи с плагином
    [DllImport("__Internal")]
    private static extern void ShowRewardADV();    // Показ рекламы с наградой - внешняя функция для связи с плагином
    [DllImport("__Internal")]
    private static extern void ShowRewardADVUnlock();    // Показ рекламы с наградой - внешняя функция для связи с плагином
    [DllImport("__Internal")]
    private static extern void ShowRewardADVBooster();    // Показ рекламы с наградой - внешняя функция для связи с плагином
    [DllImport("__Internal")]
    private static extern void GetLeaderBoardEntries();
    [DllImport("__Internal")]
    private static extern void SetLeaderBoard(int score); 
    
    public event Action AuthSuccess;    //События
    public event Action DataGet;    //События
    public event Action RewardGet;  //События
    public event Action RewardGetUnlockStage;  //События
    public event Action RewardGetBooster;  //События
    public event Action<string> LeaderBoardReady;

    public void Authenticate()    //    Авторизация
    {
        Auth();
    }

    public void GettingData()    // Получение данных
    {
        GetData();
    }

    public void SettingData(string data)    // Сохранение данных
    {
        SetData(data);
    }

    public void getLeaderEntries()
    {
        GetLeaderBoardEntries();
    }

    public void setLeaderScore(int score)
    {
        SetLeaderBoard(score);
    }

    public void BoardEntriesReady(string _data)
    {
        LeaderBoardReady?.Invoke(_data);
    }

    public void ShowCommonAdvertisment()    // Показ обычной рекламы
    {
        ShowCommonADV();
    }

    public void ShowRewardAdvertisment()    // Показ рекламы с наградой
    {
        ShowRewardADV();
    }

    public void ShowRewardAdvertismentUnlock()    // Показ рекламы с наградой
    {
        ShowRewardADVUnlock();
    }

    public void ShowRewardAdvertismentBooster()    // Показ рекламы с наградой
    {
        ShowRewardADVBooster();
    }

    public void AuthenticateSuccess(string data)    // Авторизация успешно пройдена
    {
        UD.Name = data;
        AuthSuccess?.Invoke();
    }
    
    public void DataGetting(string data) // Данные получены
    {
        UserDataSaving UDS = new UserDataSaving();
        UDS = JsonUtility.FromJson<UserDataSaving>(data);
        UGD = JsonUtility.FromJson<UserGameData>(UDS.data);
        DataGet?.Invoke();
    }

    public void RewardGettingBooster()
    {
        RewardGetBooster?.Invoke();
    }

    public void RewardGettingUnlockStage()
    {
        RewardGetUnlockStage?.Invoke();
    }
    
    public void RewardGetting() // Реклама просмотрена
    {
        RewardGet?.Invoke();
    }
#endif
}

[Serializable]
public class UserData
{
    public string Name;
    public string image;
}

[Serializable]
public class UserGameData
{
    public UserGameData(int coin)
    {
        Coin = coin;
    }
    public int Coin;
}
[Serializable]
public class UserDataSaving
{
    public string data;
}

