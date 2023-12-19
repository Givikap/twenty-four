/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: RewardedAdController.cs

-------------------------------------------------------*/

using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

//
// RewardedAdController:
//
// Used to manage rewared ad based on the system.
//
public class RewardedAdController : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;          // show rewarded ad button
    [SerializeField] GameObject messageManager;     // reference to messageManager script
    [SerializeField] GameObject solutionsWidnow;    // pseudo canvas with solutions

    // Identify the systen and assign rewarded ID accordingly
#if UNITY_IOS
    string _RewardedAd = "Rewarded_iOS";
#else
    string _RewardedAd = "Rewarded_Android"; 
#endif

    void Start()
    {
        Advertisement.Load(_RewardedAd, this);
    }

    public void ShowAd()
    {
        Advertisement.Show(_RewardedAd, this);
    }

    //
    // OnUnityAdsAdLoaded:
    //
    // Makes the ad button clickable.
    //
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(_RewardedAd))
            _showAdButton.onClick.AddListener(ShowAd);
    }

    //
    // OnUnityAdsShowComplete:
    //
    // Gives a reward after watching ad.
    //
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_RewardedAd) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            PlayerPrefs.SetInt("SolutionsLeft", 5);
            messageManager.GetComponent<MessageController>().ShowSolutions();
            _showAdButton.gameObject.SetActive(false);
        }
    }

    //
    // OnUnityAdsFailedToLoad:
    //
    // Informs the player thar failure happened during loading ad.
    //
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        _showAdButton.gameObject.SetActive(false);
        solutionsWidnow.GetComponent<TMP_Text>().text = "Can't load ad!\nTry again later.";
    }

    //
    // OnUnityAdsShowFailure:
    //
    // Informs the player thar failure happened during showing ad.
    //
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        _showAdButton.gameObject.SetActive(false);
        solutionsWidnow.GetComponent<TMP_Text>().text = "Something went wrong!\nTry again later.";
    }

    //
    // OnUnityAdsShowStart and OnUnityAdsShowClick:
    //
    // Required for correct rewarded ads work.
    //
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}
