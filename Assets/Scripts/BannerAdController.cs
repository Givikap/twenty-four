/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: BannerAdController.cs

-------------------------------------------------------*/

using UnityEngine;
using UnityEngine.Advertisements;

//
// BannerAdController:
//
// Used to manage banner ad based on the system.
//
public class BannerAdController : MonoBehaviour
{
    // Identify the systen and assign banner ID accordingly
#if UNITY_IOS
    string _BannerAd = "Banner_iOS";
#else
    string _BannerAd = "Banner_Android";
#endif

    void Awake()
    {
        // Do not the game object with this script
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(_BannerAd);
        Advertisement.Banner.Show(_BannerAd);
    }
}
