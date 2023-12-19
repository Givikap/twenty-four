/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: ScreenCaptureController.cs

-------------------------------------------------------*/

using UnityEngine;

//
// ScreenCaptureController:
//
// Used to generates a screenshot and save it to the disk with the name screenshot{integer}.png.
// 
public class ScreenCaptureController : MonoBehaviour
{
    private int counter = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Make screenshot and save it if "space" button is pressed
        if (Input.GetKeyDown("space"))
            ScreenCapture.CaptureScreenshot($"screenshot{counter++}.png");
    }
}