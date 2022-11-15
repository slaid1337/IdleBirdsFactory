using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Newtonsoft.Json;
using System;

public class GPManager : ServiceBase
{
    public GPManager(GameController gameController)
    {
        _gameController = gameController;
    }

    public override void Initialize()
    {
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        //      .RequestServerAuthCode(false /* Don't force refresh */)
        //      .EnableSavedGames()
        //      .Build();

        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.Activate();
    }

    public override void LogIn()
    {
        //PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, SignInResult);
        //Debug.Log("Player Name ----------" + PlayGamesPlatform.Instance.GetUserDisplayName());
    }

    public override void LogOut()
    {
        //PlayGamesPlatform.Instance.SignOut();
    }

    private void SignInResult(SignInStatus status)
    {
        Debug.Log("Status -------------" + status.ToString());
    }

    public bool IsLogged()
    {
        //return PlayGamesPlatform.Instance.IsAuthenticated();
        return false;
    }

    private string saveGameController = "GameControllerSave";
    private string saveCells = "CellsSave";
    private string saveSeller = "SellerSave";
    private string saveStats = "StatsSave";

    #region GameController

    public override void SaveGameContollerSaveCloud()
    {
        if (Social.localUser.authenticated)
        {
            //((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveGameController, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenToSaveGameControllerSaveCloud);
        }
    }

    public override void LoadGameContollerSaveCloud()
    {
        if (Social.localUser.authenticated)
        {
            //((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveGameController, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenGameControllerSaveCloud);
        }
    }

    public void OpenGameControllerSaveCloud(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        //((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, ReadGameControllerDataFromCloud);
    }

    public void OpenToSaveGameControllerSaveCloud(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(GetDataGameControllerToStoreInCloud());
        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
        //((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(meta, update, data, SaveUpdate);
    }

    private void ReadGameControllerDataFromCloud(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string saveData = System.Text.ASCIIEncoding.ASCII.GetString(data);
        }
    }

    #endregion

    #region Cells

    public override void SaveCellsSaveCloud()
    {
        if (Social.localUser.authenticated)
        {
            //((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveCells, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenToCellsSaveCloud);
        }
    }

    public override void LoadCellsSaveCloud()
    {
        if (Social.localUser.authenticated)
        {
            //((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveCells, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenCellsSaveCloud);
        }
    }

    public void OpenCellsSaveCloud(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        //((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, ReadDataFromCloud);
    }

    public void OpenToCellsSaveCloud(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(GetDataCellsToStoreInCloud());
        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
        //((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(meta, update, data, SaveUpdate);
    }

    #endregion

    #region Seller

    public override void SaveSellerSaveCloud()
    {
        if (Social.localUser.authenticated)
        {
            //((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveSeller, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenToSellerSaveCloud);
        }
    }

    public override void LoadSellerSaveCloud()
    {
        if (Social.localUser.authenticated)
        {
            //((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveSeller, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenSellerSaveCloud);
        }
    }

    public void OpenSellerSaveCloud(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        //((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, ReadDataFromCloud);
    }

    public void OpenToSellerSaveCloud(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(GetDataSellerToStoreInCloud());
        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
        //((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(meta, update, data, SaveUpdate);
    }

    #endregion


    #region Stats

    public override void SaveStatsSaveCloud()
    {
        if (Social.localUser.authenticated)
        {
            //((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveStats, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenToStatsSaveCloud);
        }
    }

    public override void LoadStatsSaveCloud()
    {
        if (Social.localUser.authenticated)
        {
            //((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveStats, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenStatsSaveCloud);
        }
    }

    public void OpenStatsSaveCloud(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        //((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, ReadDataFromCloud);
    }

    public void OpenToStatsSaveCloud(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(GetDataSellerToStoreInCloud());
        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
        //((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(meta, update, data, SaveUpdate);
    }

    #endregion

    private void ReadDataFromCloud(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string saveData = System.Text.ASCIIEncoding.ASCII.GetString(data);
            LoadDataFromCloudToGame(saveData);
        }
    }

    private void LoadDataFromCloudToGame(string saveData)
    {
        Debug.Log(saveData);
    }

    private void SaveUpdate(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        Debug.Log("successfully adding data to server");
    }

    private string GetDataGameControllerToStoreInCloud()
    {
        GameControllerSave save = _gameController._saveController.LoadGameController();

        string data = JsonConvert.SerializeObject(save);

        return data;
    }

    private string GetDataCellsToStoreInCloud()
    {
        CellsSave[] save = _gameController._saveController.LoadCells();

        string data = JsonConvert.SerializeObject(save);

        return data;
    }

    private string GetDataSellerToStoreInCloud()
    {
        SellerSave save = _gameController._saveController.LoadSeller();

        string data = JsonConvert.SerializeObject(save);

        return data;
    }

    private string GetDataStatsToStoreInCloud()
    {
        StatisticSave save = _gameController._saveController.LoadStatistic();

        string data = JsonConvert.SerializeObject(save);

        return data;
    }
}