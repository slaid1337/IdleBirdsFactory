using UnityEngine;

public abstract class ServiceBase : MonoBehaviour
{
    public GameController _gameController;

    public abstract void Initialize();

    public abstract void LogIn();

    public abstract void LogOut();

    public abstract void SaveGameContollerSaveCloud();
    public abstract void LoadGameContollerSaveCloud();

    public abstract void SaveCellsSaveCloud();
    public abstract void LoadCellsSaveCloud();

    public abstract void SaveSellerSaveCloud();
    public abstract void LoadSellerSaveCloud();

    public abstract void SaveStatsSaveCloud();
    public abstract void LoadStatsSaveCloud();
}
