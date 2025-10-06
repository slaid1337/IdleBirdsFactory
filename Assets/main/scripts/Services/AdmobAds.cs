using UnityEngine;
using System;

#if UNITY_ANDROID
using GoogleMobileAds.Api;
#endif

public class AdmobAds : Singletone<AdmobAds>
{
    [SerializeField] private GameController _gameController;
#if UNITY_ANDROID
    public RewardedAd _rewarded2XBooster;
    public RewardedAd _rewarded2XOfflineProfit;
    public RewardedAd _rewarded2XLvlUp;
    public RewardedAd _rewardedQuestSkip;
    public RewardedAd _rewardedRespinRoll;
    public RewardedAd _rewardedUnlockStage;
    public InterstitialAd _interstitialStandart;
    private BannerView _banner;

    [SerializeField] private BoosterObject _2x;

    public MissionCellController _missionCellController;

    public Cell _cell;

    private string _2XBoosterId = "ca-app-pub-2117946503817885/4570808666";
    private string _2XOfflineProfitId = "ca-app-pub-2117946503817885/8748591350";
    private string _2XLvlUpId = "ca-app-pub-2117946503817885/1768567265";
    private string _QuestSkipId = "ca-app-pub-2117946503817885/3700580121";
    private string _RespinRollId = "ca-app-pub-2117946503817885/5296748733";
    private string _UnlockStageId = "ca-app-pub-2117946503817885/4107171595";
    private string _interstitialId = "ca-app-pub-2117946503817885/3881339689";
    private string _bannerId = "ca-app-pub-2117946503817885/4873458354";

    private string _bannerTestId = "ca-app-pub-3940256099942544/6300978111";
    private string _interstitialTestId = "ca-app-pub-3940256099942544/1033173712";
    private string _rewardedTestId = "ca-app-pub-3940256099942544/5224354917";

    public void Start()
    {
        MobileAds.Initialize(initStatus => { Debug.Log("status of initialize ads = " + initStatus); });

        _rewarded2XBooster = new RewardedAd(_2XBoosterId);
        _rewarded2XLvlUp = new RewardedAd(_2XLvlUpId);
        _rewarded2XOfflineProfit = new RewardedAd(_2XOfflineProfitId);
        _rewardedQuestSkip = new RewardedAd(_QuestSkipId);
        _rewardedRespinRoll = new RewardedAd(_RespinRollId);
        _rewardedUnlockStage = new RewardedAd(_UnlockStageId);

        _rewarded2XBooster.OnUserEarnedReward += Earn2XBooster;
        _rewarded2XOfflineProfit.OnUserEarnedReward += Earn2XOfflineProfit;
        _rewarded2XLvlUp.OnUserEarnedReward += Earn2XLvlUp;
        _rewardedQuestSkip.OnUserEarnedReward += EarnQuestSkip;
        _rewardedRespinRoll.OnUserEarnedReward += EarnRespinRoll;
        _rewardedUnlockStage.OnUserEarnedReward += EarnUnlockStage;

        _banner = new BannerView(_bannerId, AdSize.Banner, AdPosition.Bottom);
        _interstitialStandart = new InterstitialAd(_interstitialId);

        _rewarded2XBooster.LoadAd(CreateAdRequest());
        _rewarded2XLvlUp.LoadAd(CreateAdRequest());
        _rewarded2XOfflineProfit.LoadAd(CreateAdRequest());
        _rewardedQuestSkip.LoadAd(CreateAdRequest());
        _rewardedRespinRoll.LoadAd(CreateAdRequest());
        _rewardedUnlockStage.LoadAd(CreateAdRequest());
        _interstitialStandart.LoadAd(CreateAdRequest());

        if (_gameController.IsAdShowable)
        {
            _banner.OnAdLoaded += ShowBanner;
            _banner.LoadAd(CreateAdRequest());
        }
    }

    public void CloseBanner()
    {
        _banner.Hide();
        _gameController.OnBannerClose();
    }

    public void ShowBanner(object sender, EventArgs args)
    {
        _gameController.OnBannerShow();
    }

    public void ShowRewarded2XBooster()
    {
        if (_rewarded2XBooster.IsLoaded())
        {
            _rewarded2XBooster.Show();
        }

        _rewarded2XBooster.LoadAd(CreateAdRequest());
    }

    public void ShowRewarded2XLvlUp()
    {
        if (_rewarded2XLvlUp.IsLoaded())
        {
            _rewarded2XLvlUp.Show();
        }

        _rewarded2XLvlUp.LoadAd(CreateAdRequest());
    }

    public void ShowRewarded2XOfflineProfit()
    {
        if (_rewarded2XOfflineProfit.IsLoaded())
        {
            _rewarded2XOfflineProfit.Show();
        }

        _rewarded2XOfflineProfit.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedQuestSkip()
    {
        if (_rewardedQuestSkip.IsLoaded())
        {
            _rewardedQuestSkip.Show();
        }

        _rewardedQuestSkip.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedRespinRoll()
    {
        if (_rewardedRespinRoll.IsLoaded())
        {
            _rewardedRespinRoll.Show();
        }

        _rewardedRespinRoll.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedUnlockStage()
    {
        if (_rewardedUnlockStage.IsLoaded())
        {
            _rewardedUnlockStage.Show();
        }

        _rewardedUnlockStage.LoadAd(CreateAdRequest());
    }

    public void Earn2XBooster(object sender, Reward args)
    {
        _gameController.BoostersContainer.Add(_2x);
    }

    public void Earn2XOfflineProfit(object sender, Reward args)
    {
        _gameController.OfflineProfitCard.GetComponent<PassiveCard>().GetProfit2X();
    }

    public void Earn2XLvlUp(object sender, Reward args)
    {
        _gameController.LevelCard.GetComponent<LevelCard>().GetProfit2X();
    }

    public void EarnQuestSkip(object sender, Reward args)
    {
        _missionCellController.Collect();
    }

    public void EarnRespinRoll(object sender, Reward args)
    {
        _gameController.CollectCard.GetComponent<CollectCard>().Respin();
    }

    public void EarnUnlockStage(object sender, Reward args)
    {
        _cell.BreakLock();
    }

    public void ShowInterstitial()
    {
        if (_interstitialStandart.IsLoaded())
        {
            _interstitialStandart.Show();
        }

        _interstitialStandart.LoadAd(CreateAdRequest());
    }

    public void ShowBanner()
    {
        _banner.Show();
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .Build();
    }
#endif
}