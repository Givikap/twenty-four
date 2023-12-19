/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: UIController.cs

-------------------------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;

//
// UIController:
//
// Contains functions for each game button.
//
public class UIController : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadPreGame()
    {
        SceneManager.LoadScene("PreGameScene");
    }

    public void LoadDifficulty()
    {
        SceneManager.LoadScene("DifficultyScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadAfterGame()
    {
        // State that the player gave up
        GameHelper.gameCode = "GiveUp";
        // Increase counter of give ups by one
        PlayerPrefs.SetInt("PuzzlesGaveUp", PlayerPrefs.GetInt("PuzzlesGaveUp") + 1);
        SceneManager.LoadScene("AfterGameScene");
    }

    public void LoadStatistics()
    {
        SceneManager.LoadScene("StatisticsScene");
    }

    public void UnloadPuzzles()
    {
        Destroy(GameObject.Find("PuzzleManager"));
    }

    //
    // PlayButtonSound:
    //
    public void PlayButtonSound()
    {
        // Start the button audio 0.2 seconds later since it's delayed 
        GameObject.Find("AudioManager").GetComponent<AudioSource>().time = 0.2f;
        GameObject.Find("AudioManager").GetComponent<AudioSource>().Play();
    }
}
