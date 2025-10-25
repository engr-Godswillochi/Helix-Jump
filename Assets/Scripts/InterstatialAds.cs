// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Advertisements;

// public class InterstitialAds : MonoBehaviour , IUnityAdsLoadListener ,IUnityAdsShowListener
// {
//     [SerializeField] private string androidAdUnitId;
//     [SerializeField] private string iosAdUnitId;

//     private string adUnitId;

//     private void Awake()
//     {
//         #if UNITY_IOS
//                 adUnitId = iosAdUnitId;
//         #elif UNITY_ANDROID
//                 adUnitId = androidAdUnitId;
//         #endif
//         LoadInterstitialAd();
//     }

//     public void LoadInterstitialAd()
//     {
//         Advertisement.Load(adUnitId, this);
//     }

//     public void ShowInterstitialAd()
//     {
//         Advertisement.Show(adUnitId, this);
//         LoadInterstitialAd();
//     }




//     #region LoadCallbacks
//     public void OnUnityAdsAdLoaded(string placementId)
//     {
//         Debug.Log("Interstitial Ad Loaded");
//     }

//     public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)    {    }
//     #endregion
//     #region ShowCallbacks
//     public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)    {    }

//     public void OnUnityAdsShowStart(string placementId)    {    }

//     public void OnUnityAdsShowClick(string placementId)    {    }

//     public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
//     {
//         Debug.Log("Interstitial Ad Completed");
//     }
//     #endregion
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;
    private bool adLoaded = false;

    private void Awake()
    {
        #if UNITY_IOS
            adUnitId = iosAdUnitId;
        #elif UNITY_ANDROID
            adUnitId = androidAdUnitId;
        #elif UNITY_EDITOR
            adUnitId = androidAdUnitId;
        #endif
    }

    private void Start()
    {
        // Load the first ad when the game starts
        LoadInterstitialAd();
    }

    public void LoadInterstitialAd()
    {
        Debug.Log("Loading Interstitial Ad...");
        Advertisement.Load(adUnitId, this);
    }

    public void ShowInterstitialAd()
    {
        // Only show if the ad is loaded
        if (adLoaded)
        {
            Debug.Log("Showing Interstitial Ad...");
            Advertisement.Show(adUnitId, this);
            adLoaded = false; // Reset the flag
        }
        else
        {
            Debug.Log("Interstitial Ad not loaded yet. Loading now...");
            LoadInterstitialAd();
        }
    }

    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial Ad Loaded: " + placementId);
        adLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load ad: {placementId} - Error: {error} - {message}");
        adLoaded = false;
    }
    #endregion

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Failed to show ad: {placementId} - Error: {error} - {message}");
        adLoaded = false;
        // Load the next ad
        LoadInterstitialAd();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Interstitial Ad Started: " + placementId);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Interstitial Ad Clicked: " + placementId);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Interstitial Ad Completed: " + placementId);
        adLoaded = false;
        // Load the next ad for next time
        LoadInterstitialAd();
    }
    #endregion
}