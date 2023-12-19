/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: AdsInitializer.cs

-------------------------------------------------------*/

using UnityEngine;
using UnityEngine.Advertisements;

//
// AdsInitializer:
//
// Used to initialize ads based on the system.
//
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    // For debug purposes
    [SerializeField] bool _testMode = false;

    // Identify the systen and assign game ID accordingly
#if UNITY_IOS
    string _gameId = "5331856";
#elif UNITY_ANDROID
    string _gameId = "5331857";
#endif

    void Awake()
    {
        InitializeAds();
    }

    //
    // InitializeAds:
    //
    public void InitializeAds()
    {
        // Only if the ads are not initialized previously
        if (!Advertisement.isInitialized && Advertisement.isSupported)
            Advertisement.Initialize(_gameId, _testMode, this);
    }

    //
    // OnInitializationCompleted and OnInitializationFailed:
    //
    // Required for correct ads initialization.
    // 
    public void OnInitializationComplete() { }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { }
}
