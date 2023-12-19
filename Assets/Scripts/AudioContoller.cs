/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: AudioContoller.cs

-------------------------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//
// AudioContoller:
//
// Used to manage the audio in the game.
// 
public class AudioContoller : MonoBehaviour
{
    [SerializeField] Sprite unmutedButtonSprite;    // sprite for unmuted button
    [SerializeField] Sprite mutedButtonSprite;      // sprite for muted button

    [SerializeField] Button muteUnmuteButton;       // mute/unmute button
    [SerializeField] Canvas audioCanvas;            // seperate canvas for the button

    // Helper variable storing if the audio is muted or not
    private bool toggle = true;

    void Start()
    {
        // Do not destroy both the game object with script and audio source and the canvas with the button
        DontDestroyOnLoad(audioCanvas);
        DontDestroyOnLoad(gameObject);

        // Immediately switch to Menu scene
        SceneManager.LoadScene("MenuScene");
    }

    //
    // MuteUnmute:
    //
    // Mutes the audio if unmuted and vice versa.
    // NOTE: muting audio with volume works better than using mute property
    //
    public void MuteUnmute()
    {
        if (toggle)
        {
            AudioListener.volume = 0f;
            muteUnmuteButton.GetComponent<Image>().sprite = mutedButtonSprite;
        }

        else
        {
            AudioListener.volume = 1f;
            muteUnmuteButton.GetComponent<Image>().sprite = unmutedButtonSprite;
        }
        toggle = !toggle;
    }
}
