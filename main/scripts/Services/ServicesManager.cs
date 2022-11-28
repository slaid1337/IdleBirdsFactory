using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
#endif

public class ServicesManager : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    public GameControllerSave _gameControllerSave;
    public CellsSave[] _cellsSave;
    public SellerSave _sellerSave;
    public StatisticSave _statisticSave;
    public ServiceSave _serviceSave;



    public SignInStatus _statutsOfLoginning = SignInStatus.NotAuthenticated;

#if UNITY_ANDROID
    public int _countOfLoading;

    public string DataTmp;

    private string saveGameController = "GameControllerSave";
    private string saveCells = "CellsSave";
    private string saveSeller = "SellerSave";
    private string saveStats = "StatsSave";
    private string saveService = "ServiceSave";

    public void Initialize()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
              .RequestServerAuthCode(false /* Don't force refresh */)
              .EnableSavedGames()
              .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    public void LogInning()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, OnEndLogin);
    }

    private void OnEndLogin(SignInStatus status)
    {
        Debug.Log(status);
        _statutsOfLoginning = status;
    }

    public bool IsLogged()
    {
        return PlayGamesPlatform.Instance.IsAuthenticated();
    }

    public void LogIn()
    {
        LogInning();
        SceneManager.LoadScene(0);
    }

    public void LogOut()
    {
        PlayGamesPlatform.Instance.SignOut();
        SceneManager.LoadScene(0);
    }

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
        Debug.Log("achievements");
    }

    public void LoadSaveCloud(SaverType saverType, string name)
    {
        if (IsLogged())
        {
            PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(name, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime,
                (status, meta) => ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta,
                (status, data) =>
                {
                    if (status == SavedGameRequestStatus.Success)
                    {
                        string saveData = System.Text.ASCIIEncoding.ASCII.GetString(data);
                        switch (saverType)
                        {
                            case SaverType.GameController:
                                _gameControllerSave = JsonConvert.DeserializeObject<GameControllerSave>(saveData);
                                break;
                            case SaverType.Cells:
                                _cellsSave = JsonConvert.DeserializeObject<CellsSave[]>(saveData);
                                break;
                            case SaverType.Seller:
                                _sellerSave = JsonConvert.DeserializeObject<SellerSave>(saveData);
                                break;
                            case SaverType.Stats:
                                _statisticSave = JsonConvert.DeserializeObject<StatisticSave>(saveData);
                                break;
                            case SaverType.Service:
                                _serviceSave = JsonConvert.DeserializeObject<ServiceSave>(saveData);
                                break;
                        }
                        DataTmp = saveData;
                        _countOfLoading++;
                        Debug.Log("saveData -------- " + saveData);
                    }
                }));
        }
    }

    public void GetServiceSaveCloud()
    {
        LoadSaveCloud(SaverType.Service, saveService);
    }

    public void GetGameControllerSaveCloud()
    {
        LoadSaveCloud(SaverType.GameController, saveGameController);
    }

    public void GetCellsSaveCloud()
    {
        LoadSaveCloud(SaverType.Cells, saveCells);
    }

    public void GetSellerSaveCloud()
    {
        LoadSaveCloud(SaverType.Seller, saveSeller);
    }

    public void GetStatsSaveCloud()
    {
        LoadSaveCloud(SaverType.Stats, saveStats);
    }

    public void SaveToCloud(SaverType saverType, string name)
    {
        Debug.Log("try saving cloud");
        if (IsLogged())
        {
            Debug.Log("start saving");
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            Debug.Log(savedGameClient);
            savedGameClient.OpenWithAutomaticConflictResolution(name, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime,
                (status, meta) =>
                {
                    Debug.Log("start loading to cloud");
                    byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(GetDataToCloud(saverType));
                    SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
                    PlayGamesPlatform.Instance.SavedGame.CommitUpdate(meta, update, data, (status, meta) => Debug.Log("succes load to cloud"));
                });
        }
        Debug.Log("saving to cloud ends with --- " + IsLogged());
    }

    public string GetDataToCloud(SaverType saverType)
    {
        string data = "";

        switch (saverType)
        {
            case SaverType.GameController:
                GameControllerSave save = _gameController._saveController.LoadGameController();
                data = JsonConvert.SerializeObject(save);
                break;
            case SaverType.Cells:
                CellsSave[] save1 = _gameController._saveController.LoadCells();
                data = JsonConvert.SerializeObject(save1);
                break;
            case SaverType.Seller:
                SellerSave save2 = _gameController._saveController.LoadSeller();
                data = JsonConvert.SerializeObject(save2);
                break;
            case SaverType.Stats:
                StatisticSave save3 = _gameController._saveController.LoadStatistic();
                data = JsonConvert.SerializeObject(save3);
                break;
            case SaverType.Service:
                ServiceSave save4 = _gameController._saveController.LoadService();
                data = JsonConvert.SerializeObject(save4);
                break;
        }
        Debug.Log("12312312312312312312213 " + data);
        return data;
    }

#endif
}
public enum SaverType
{
    GameController,
    Cells,
    Seller,
    Stats,
    Service
}