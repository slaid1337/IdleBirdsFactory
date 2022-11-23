using UnityEngine;

public class GameInAppManager : Singletone<GameInAppManager>
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private BoosterObject _50xBooster;

    public void BuySpecialOffer()
    {
        _gameController.AddCash(2800);
        _gameController.BoostersContainer.Add(_50xBooster);
        _gameController.IsAdShowable = false;
        _gameController._adsAdmob.CloseBanner();
        _gameController._saveController.SaveService(new ServiceSave(true));
    }

    public void BuyCash200()
    {
        _gameController.AddCash(200);
    }

    public void BuyCash530()
    {
        _gameController.AddCash(530);
    }

    public void BuyCash1100()
    {
        _gameController.AddCash(1100);
    }

    public void BuyCash2300()
    {
        _gameController.AddCash(2300);
    }

    public void BuyCash6000()
    {
        _gameController.AddCash(6000);
    }

    public void BuyCash13000()
    {
        _gameController.AddCash(13000);
    }
}
