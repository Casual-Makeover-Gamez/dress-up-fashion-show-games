using UnityEngine;
using System;

[System.Serializable]
public enum AdsType
{
    TestAds, LiveAds
}

public class MyAdsManager : MonoBehaviour
{

    public delegate void actionRewardedCompleted();
    public event actionRewardedCompleted onRewardedVideoAdCompletedEvent;
    public static MyAdsManager instance;
    public AdsType adsType;
    [Header("Applovin Ids")]
    public MaxSdkBase.BannerPosition applovinBannerPosition = MaxSdkBase.BannerPosition.TopCenter;
    public string MaxSdkKey = "Eii0lqMx5nRJ9H7FwV1aH1IBsJUXdDcK0LJqId0ZIkWxH3GzgCu7ZmaLbW_VUqO5aovzq_TqzUaJcwOw6zr2hc";
    public string BannerAdUnitId = "ENTER_BANNER_AD_UNIT_ID_HERE";
    public string InterstitialAdUnitId = "ENTER_INTERSTITIAL_AD_UNIT_ID_HERE";
    public string RewardedAdUnitId = "ENTER_REWARD_AD_UNIT_ID_HERE";
    public string RewardedInterstitialAdUnitId = "ENTER_REWARD_INTER_AD_UNIT_ID_HERE";
    public string MRecAdUnitId = "ENTER_MREC_AD_UNIT_ID_HERE";
    public bool isDebugLog = false;
    private bool isBannerShowing;
    private bool isMRecShowing;
    private int interstitialRetryAttempt;
    private int rewardedRetryAttempt;
    private int rewardedInterstitialRetryAttempt;
    private Action rewardedAction;


    public static MyAdsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MyAdsManager>();
                if (instance && instance.gameObject)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    #region Awake
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Start
    void Start()
    {
        if (adsType == AdsType.TestAds)
        {
#if UNITY_EDITOR || UNITY_ANDROID
            //MaxSdkKey = "ENTER_MAX_SDK_KEY_HERE";
            InterstitialAdUnitId = "ENTER_INTERSTITIAL_AD_UNIT_ID_HERE";
            RewardedAdUnitId = "ENTER_REWARD_AD_UNIT_ID_HERE";
            RewardedInterstitialAdUnitId = "ENTER_REWARD_INTER_AD_UNIT_ID_HERE";
            BannerAdUnitId = "ENTER_BANNER_AD_UNIT_ID_HERE";
            MRecAdUnitId = "ENTER_MREC_AD_UNIT_ID_HERE";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif
        }
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, configure and start loading ads.
            //MaxSdk.ShowMediationDebugger();
            Debug.Log("MAX SDK Initialized");
            InitializeApplovinInterstitial();
            InitializeApplovinRewarded();
            InitializeApplovinBanner();
            ShowApplovinBanner();
        };
        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
    }
    #endregion

    #region Applovin Banner Ad Methods
    private void InitializeApplovinBanner()
    {
        // Attach Callbacks
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
        MaxSdk.CreateBanner(BannerAdUnitId, applovinBannerPosition);
        // Set background or background color for banners to be fully functional.
        Color color = new Color(0, 0, 0, 0.1f);
        MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, color);
    }
    public void ShowApplovinBanner()
    {
        if (!isBannerShowing)
        {
            isBannerShowing = !isBannerShowing;
            MaxSdk.ShowBanner(BannerAdUnitId);
        }
    }
    public void HideApplovinBanner()
    {
        if (isBannerShowing)
        {
            isBannerShowing = !isBannerShowing;
            MaxSdk.HideBanner(BannerAdUnitId);
        }
    }
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad is ready to be shown.
        // If you have already called MaxSdk.ShowBanner(BannerAdUnitId) it will automatically be shown on the next ad refresh.
        if (isDebugLog)
            Debug.Log("Banner ad loaded");
    }
    private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Banner ad failed to load. MAX will automatically try loading a new ad internally.
        if (isDebugLog)
            Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
    }
    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (isDebugLog)
            Debug.Log("Banner ad clicked");
    }
    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad revenue paid. Use this callback to track user revenue.
        if (isDebugLog)
            Debug.Log("Banner ad revenue paid");
        // Ad revenue
        double revenue = adInfo.Revenue;
        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
        //TrackAdRevenue(adInfo);
    }
    #endregion

    #region Applovin Interstitial Ad Methods
    private void InitializeApplovinInterstitial()
    {
        // Attach callbacks
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
        // Load the first interstitial
        LoadApplovinInterstitial();
    }
    private void LoadApplovinInterstitial()
    {
        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }
    public void ShowApplovinInterstitial()
    {
        if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
        {
            AudioListener.pause = true;
            AudioListener.volume = 0;
            MaxSdk.ShowInterstitial(InterstitialAdUnitId);
        }
    }
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
        // Reset retry attempt
        interstitialRetryAttempt = 0;
    }
    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        Invoke("LoadApplovinInterstitial", (float)retryDelay);
    }
    private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        if (isDebugLog)
            Debug.Log("Interstitial failed to display with error code: " + errorInfo.Code);
        LoadApplovinInterstitial();
        AudioListener.pause = false;
        AudioListener.volume = 1;
    }
    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        if (isDebugLog)
            Debug.Log("Interstitial dismissed");
        LoadApplovinInterstitial();
        AudioListener.pause = false;
        AudioListener.volume = 1;
    }
    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad revenue paid. Use this callback to track user revenue.
        if (isDebugLog)
            Debug.Log("Interstitial revenue paid");

        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
        //TrackAdRevenue(adInfo);
    }
    #endregion

    #region Applovin Rewarded Ad Methods
    private void InitializeApplovinRewarded()
    {
        // Attach callbacks
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        // Load the first RewardedAd
        LoadApplovinRewarded();
    }
    public void LoadApplovinRewarded()
    {
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }
    public bool IsApplovinRewardedReady()
    {
        return MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
    }
    public void ShowApplovinRewarded()
    {
        if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
        {
            AudioListener.pause = true;
            AudioListener.volume = 0;
            MaxSdk.ShowRewardedAd(RewardedAdUnitId);
        }
    }
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
        if (isDebugLog)
            Debug.Log("Rewarded ad loaded");
        // Reset retry attempt
        rewardedRetryAttempt = 0;
    }
    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        if (isDebugLog)
            Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);
        Invoke("LoadApplovinRewarded", (float)retryDelay);
    }
    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        if (isDebugLog)
            Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
        LoadApplovinRewarded();
        AudioListener.pause = false;
        AudioListener.volume = 1;
    }
    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (isDebugLog)
            Debug.Log("Rewarded ad displayed");
    }
    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (isDebugLog)
            Debug.Log("Rewarded ad clicked");
    }
    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        if (isDebugLog)
            Debug.Log("Rewarded ad dismissed");
        Invoke("LoadApplovinRewarded", 1f);
        AudioListener.pause = false;
        AudioListener.volume = 1;
    }
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad was displayed and user should receive the reward
        if (isDebugLog)
            Debug.Log("Rewarded ad received reward");
        Invoke("GiveReward", 0.5f);
        AudioListener.pause = false;
        AudioListener.volume = 1;
    }
    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad revenue paid. Use this callback to track user revenue.
        if (isDebugLog)
            Debug.Log("Rewarded ad revenue paid");
        // Ad revenue
        double revenue = adInfo.Revenue;
        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
        //TrackAdRevenue(adInfo);
    }
    #endregion

    #region Applovin Rewarded Interstitial Ad Methods
    private void InitializeApplovinRewardedInterstitial()
    {
        // Attach callbacks
        MaxSdkCallbacks.RewardedInterstitial.OnAdLoadedEvent += OnRewardedInterstitialAdLoadedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdLoadFailedEvent += OnRewardedInterstitialAdFailedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdDisplayFailedEvent += OnRewardedInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdDisplayedEvent += OnRewardedInterstitialAdDisplayedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdClickedEvent += OnRewardedInterstitialAdClickedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdHiddenEvent += OnRewardedInterstitialAdDismissedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdReceivedRewardEvent += OnRewardedInterstitialAdReceivedRewardEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdRevenuePaidEvent += OnRewardedInterstitialAdRevenuePaidEvent;
        // Load the first RewardedInterstitialAd
        LoadApplovinRewardedInterstitial();
    }
    private void LoadApplovinRewardedInterstitial()
    {
        MaxSdk.LoadRewardedInterstitialAd(RewardedInterstitialAdUnitId);
    }
    private void ShowRewardedInterstitialAd()
    {
        if (MaxSdk.IsRewardedInterstitialAdReady(RewardedInterstitialAdUnitId))
        {
            MaxSdk.ShowRewardedInterstitialAd(RewardedInterstitialAdUnitId);
        }
    }
    private void OnRewardedInterstitialAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded interstitial ad is ready to be shown. MaxSdk.IsRewardedInterstitialAdReady(rewardedInterstitialAdUnitId) will now return 'true'
        if (isDebugLog)
            Debug.Log("Rewarded interstitial ad loaded");
        // Reset retry attempt
        rewardedInterstitialRetryAttempt = 0;
    }

    private void OnRewardedInterstitialAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedInterstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedInterstitialRetryAttempt));
        if (isDebugLog)
            Debug.Log("Rewarded interstitial ad failed to load with error code: " + errorInfo.Code);
        Invoke("LoadRewardedInterstitialAd", (float)retryDelay);
    }
    private void OnRewardedInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded interstitial ad failed to display. We recommend loading the next ad
        if (isDebugLog)
            Debug.Log("Rewarded interstitial ad failed to display with error code: " + errorInfo.Code);
        LoadApplovinRewardedInterstitial();
    }
    private void OnRewardedInterstitialAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (isDebugLog)
            Debug.Log("Rewarded interstitial ad displayed");
    }
    private void OnRewardedInterstitialAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (isDebugLog)
            Debug.Log("Rewarded interstitial ad clicked");
    }
    private void OnRewardedInterstitialAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded interstitial ad is hidden. Pre-load the next ad
        if (isDebugLog)
            Debug.Log("Rewarded interstitial ad dismissed");
        LoadApplovinRewardedInterstitial();
    }
    private void OnRewardedInterstitialAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded interstitial ad was displayed and user should receive the reward
        if (isDebugLog)
            Debug.Log("Rewarded interstitial ad received reward");
    }
    private void OnRewardedInterstitialAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded interstitial ad revenue paid. Use this callback to track user revenue.
        if (isDebugLog)
            Debug.Log("Rewarded interstitial ad revenue paid");
        // Ad revenue
        double revenue = adInfo.Revenue;
        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
        //TrackAdRevenue(adInfo);
    }
    #endregion

    #region Applovin MREC Ad Methods
    private void InitializeMRecAds()
    {
        // Attach Callbacks
        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;
        // MRECs are automatically sized to 300x250.
        MaxSdk.CreateMRec(MRecAdUnitId, MaxSdkBase.AdViewPosition.BottomCenter);
    }

    private void ToggleMRecVisibility()
    {
        if (!isMRecShowing)
        {
            MaxSdk.ShowMRec(MRecAdUnitId);
        }
        else
        {
            MaxSdk.HideMRec(MRecAdUnitId);
        }
        isMRecShowing = !isMRecShowing;
    }
    private void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // MRec ad is ready to be shown.
        // If you have already called MaxSdk.ShowMRec(MRecAdUnitId) it will automatically be shown on the next MRec refresh.
        if (isDebugLog)
            Debug.Log("MRec ad loaded");
    }
    private void OnMRecAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // MRec ad failed to load. MAX will automatically try loading a new ad internally.
        if (isDebugLog)
            Debug.Log("MRec ad failed to load with error code: " + errorInfo.Code);
    }
    private void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (isDebugLog)
            Debug.Log("MRec ad clicked");
    }
    private void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // MRec ad revenue paid. Use this callback to track user revenue.
        if (isDebugLog)
            Debug.Log("MRec ad revenue paid");
        // Ad revenue
        double revenue = adInfo.Revenue;
        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD"!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
        //TrackAdRevenue(adInfo);
    }
    //private void TrackAdRevenue(MaxSdkBase.AdInfo adInfo)
    //{
    //    AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
    //    adjustAdRevenue.setRevenue(adInfo.Revenue, "USD");
    //    adjustAdRevenue.setAdRevenueNetwork(adInfo.NetworkName);
    //    adjustAdRevenue.setAdRevenueUnit(adInfo.AdUnitIdentifier);
    //    adjustAdRevenue.setAdRevenuePlacement(adInfo.Placement);
    //    Adjust.trackAdRevenue(adjustAdRevenue);
    //}
    #endregion

    #region ShowBanners
    public void ShowBanner()
    {
        ShowApplovinBanner();
    }
    #endregion

    #region HideBanners
    public void HideBanner()
    {
        HideApplovinBanner();
    }
    #endregion

    #region ShowInterstitialAds
    public bool IsInterstitialAvailable()
    {
        return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
    }
    public void ShowInterstitialAds()
    {
        ShowApplovinInterstitial();
    }
    #endregion

    #region ShowRewardedVideos
    public bool IsRewardedAvailable()
    {
        return MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
    }
    public void ShowRewardedVideos()
    {
        ShowApplovinRewarded();
    }
    public void ShowRewardedVideos(Action actionToCall = null)
    {
        ShowApplovinRewarded();
    }
    #endregion

    #region GiveReward
    private void GiveReward()
    {
        if(rewardedAction != null)
        {
            rewardedAction.Invoke();
        }
        else if (onRewardedVideoAdCompletedEvent != null)
        {
            onRewardedVideoAdCompletedEvent();
        }
        rewardedAction = null;
    }
    #endregion
}
