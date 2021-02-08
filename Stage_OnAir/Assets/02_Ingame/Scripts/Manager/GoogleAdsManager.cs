using System;
using UnityEngine;
using GoogleMobileAds.Api;
using Define;

public class GoogleAdsManager : Singleton<GoogleAdsManager>
{
    private RewardedAd RewardAd;
    private RewardedInterstitialAd RewardInterAd;
    private InterstitialAd IntersAd;
    private BannerView BannerAd;
    private bool isTest = false;

    new void Awake()                                                                                                                                                                                                                
    {
        base.Awake();

        MobileAds.Initialize(AD.APPID);

        RequestInterstitial();
        RequestRewardedAd();
        RequestBanner();
    }

    // 전면 광고 
    private void RequestInterstitial()
    {
        string adUnitId = isTest ? AD.TEST_INTERS : AD.INTERSAD; //테스트 아이디

        // Initialize an InterstitialAd.
        this.IntersAd = new InterstitialAd(adUnitId);
        // 전면광고
        // Called when an ad request has successfully loaded.
        this.IntersAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.IntersAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.IntersAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.IntersAd.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.IntersAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.IntersAd.LoadAd(request);
    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("interstitial Failed : "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        RequestInterstitial();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void IntersAdsShow()
    {
        if (this.IntersAd.IsLoaded())
        {
            this.IntersAd.Show();
        }
        else
        {
            Debug.Log("NOT Loaded Interstitial");
            RequestInterstitial();
        }
    }

    // 보상형 광고
    private void RequestRewardedAd()
    {
        string adUnitId = isTest ? AD.TEST_REWARD : AD.REWARDAD;

        this.RewardAd = new RewardedAd(adUnitId);
        
        // Called when an ad request has successfully loaded.
        this.RewardAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.RewardAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.RewardAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.RewardAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.RewardAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.RewardAd.OnAdClosed += HandleRewardedAdClosed;
        
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.RewardAd.LoadAd(request);

        AdRequest request2 = new AdRequest.Builder().Build();
        RewardedInterstitialAd.LoadAd(adUnitId, request2, adLoadCallback);
    }
    private void adLoadCallback(RewardedInterstitialAd ad, string error)
    {
        if (error == null)
        {
            RewardInterAd = ad;

            RewardInterAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresent;
            RewardInterAd.OnAdDidPresentFullScreenContent += HandleAdDidPresent;
            RewardInterAd.OnAdDidDismissFullScreenContent += HandleAdDidDismiss;
            RewardInterAd.OnPaidEvent += HandlePaidEvent;
        }
    }

    public void ShowRewardedInterstitialAd()
    {
        if (RewardInterAd != null)
        {
            RewardInterAd.Show(userEarnedRewardCallback);
        }
    }

    private void userEarnedRewardCallback(Reward reward)
    {
        int Result = Mathf.CeilToInt(GameManager.Instance.Money * 0.1f);
        GameManager.Instance.CostMoney(Result, false);
    }
    private void HandleAdFailedToPresent(object sender, AdErrorEventArgs args)
    {
        print("Rewarded interstitial ad has failed to present.");
    }

    private void HandleAdDidPresent(object sender, EventArgs args)
    {
        print("Rewarded interstitial ad has presented.");
    }

    private void HandleAdDidDismiss(object sender, EventArgs args)
    {
        print("Rewarded interstitial ad has dismissed presentation.");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        MonoBehaviour.print(
            "Rewarded interstitial ad has received a paid event.");
    }
///////


    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        RequestRewardedAd();
    }

    // 리워드 보상 넣는 곳
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;

        int Result = Mathf.CeilToInt(GameManager.Instance.Money * 0.1f);
        GameManager.Instance.CostMoney(Result, false);

        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }

    public void RewardAdsShow()
    {
        if (this.RewardAd.IsLoaded())
        {
            this.RewardAd.Show();
        }
        else
        {
            Debug.Log("NOT Loaded RewardedAd");
            RequestRewardedAd();
        }

        //if (RewardInterAd != null)
        //{
        //    RewardInterAd.Show(userEarnedRewardCallback);
        //}
    }

    // 배너 광고
    private void RequestBanner()
    {
        //string adUnitId = AD.BANNERAD;
        string adUnitId = isTest ? AD.TEST_BANNER : AD.BANNERAD;

        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        BannerAd = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Called when an ad request has successfully loaded.
        BannerAd.OnAdLoaded += HandleOnAdLoaded_banner;
        // Called when an ad request failed to load.
        BannerAd.OnAdFailedToLoad += HandleOnAdFailedToLoad_banner;
        // Called when an ad is clicked.
        BannerAd.OnAdOpening += HandleOnAdOpened_banner;
        // Called when the user returned from the app after an ad click.
        BannerAd.OnAdClosed += HandleOnAdClosed_banner;
        // Called when the ad click caused the user to leave the application.
        BannerAd.OnAdLeavingApplication += HandleOnAdLeavingApplication_banner;

        AdRequest request = new AdRequest.Builder().Build();

        BannerAd.LoadAd(request);
    }

    public void ShowBanner()
    {
        BannerAd.Show();
    }

    public void HideBanner()
    {
        BannerAd.Hide();
    }

    public Vector2 GetBannerSize()
    {
        Vector2 Size = new Vector2(BannerAd.GetWidthInPixels(), BannerAd.GetHeightInPixels());

        return Size;
    }

    public float GetBannerHeight()
    {
        float Size = BannerAd.GetHeightInPixels();

        return Size;
    }

    public void HandleOnAdLoaded_banner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received_banner");
    }

    public void HandleOnAdFailedToLoad_banner(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd_banner event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened_banner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received_banner");
    }

    public void HandleOnAdClosed_banner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received_banner");
    }

    public void HandleOnAdLeavingApplication_banner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received_banner");
    }
}

