using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_ANDROID
using GooglePlayGames.BasicApi;
using Firebase;
using Firebase.Analytics;
#endif

public class LoadGameManager : MonoBehaviour
{
    [SerializeField] private ServicesManager _servicesManager;
    [SerializeField] private SaveController _saveController;
    //private YandexSDK _yandexSDK;

    private void Start()
    {
#if UNITY_ANDROID
        _servicesManager.Initialize();
        _servicesManager.LogInning();
#endif

        //try
        //{
        //    _yandexSDK = YandexSDK.Instance;
        //    _yandexSDK.Authenticate();
        //}
        //catch
        //{

        //}


        StartCoroutine(UntilLogin());

#if UNITY_ANDROID
        try
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        }
        catch
        {
            Debug.Log("Firebase not loaded");
        }
#endif
    }

    private IEnumerator UntilLogin()
    {
#if UNITY_ANDROID
        yield return new WaitUntil(() => _servicesManager._statutsOfLoginning != SignInStatus.NotAuthenticated && _servicesManager._statutsOfLoginning != SignInStatus.AlreadyInProgress);
#else
        yield return new WaitForSeconds(0.1f);
#endif

#if UNITY_ANDROID
        Debug.Log("try loading");
        if (_servicesManager._statutsOfLoginning == SignInStatus.Success)
        {

            Debug.Log("start load cloud");
            yield return new WaitUntil(() => _servicesManager.IsLogged() == true);

            if (_servicesManager.IsLogged())
            {
                StartCoroutine(LoadSceneWhenLoad());
            }

        }
        else
        {
            SceneManager.LoadScene(1);
            Debug.Log("--------Load_local_save--------");
        }
#endif

        SceneManager.LoadScene(1);
        Debug.Log("--------Load_local_save--------");
    }

#if UNITY_ANDROID
    private IEnumerator LoadSceneWhenLoad()
    {
        GameControllerSave self = _saveController.LoadGameController();
        _servicesManager.GetGameControllerSaveCloud();

        yield return new WaitUntil(() => _servicesManager._countOfLoading > 0);

        if (_servicesManager.DataTmp.Length > 1)
        {
            GameControllerSave cloud = _servicesManager._gameControllerSave;

            if (cloud.Level > self.Level)
            {
                _servicesManager.GetServiceSaveCloud();
                _servicesManager.GetCellsSaveCloud();
                _servicesManager.GetSellerSaveCloud();
                _servicesManager.GetStatsSaveCloud();

                yield return new WaitUntil(() => _servicesManager._cellsSave.Length > 4 && _servicesManager._sellerSave.Lvl > 0 && _servicesManager._statisticSave.CountOfGameStarts > 0);

                CellsSave[] cells = _servicesManager._cellsSave;
                SellerSave seller = _servicesManager._sellerSave;
                StatisticSave stats = _servicesManager._statisticSave;
                ServiceSave service = _servicesManager._serviceSave;

                _saveController.SaveAllFromCloud(cloud, cells, seller, stats, service);

                yield return new WaitForSeconds(1f);
            }

            Debug.Log("PRELOADDDDDDDDD_____" + cloud.Level);
        }

        SceneManager.LoadScene(1);
    }
#endif
}
