/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: StatisticsController.cs

-------------------------------------------------------*/

using TMPro;
using UnityEngine;

//
// StatisticsController:
//
// Used to show game statistics to the player.
// 
public class StatisticsController : MonoBehaviour
{
    [SerializeField] TMP_Text statisticsText;   // statistics text

    void Start()
    {
        statisticsText.text = $" {PlayerPrefs.GetInt("PuzzlesAttemted")}\n\n" +
            $" {PlayerPrefs.GetInt("TotalPuzzles")}\n" +
            $" {PlayerPrefs.GetInt("EasyPuzzles")}\n" +
            $" {PlayerPrefs.GetInt("MediumPuzzles")}\n" +
            $" {PlayerPrefs.GetInt("HardPuzzles")}\n\n" +
            $" {PlayerPrefs.GetInt("PuzzlesSkipped")}\n" +
            $" {PlayerPrefs.GetInt("PuzzlesGaveUp")}\n\n" +
            $" {PlayerPrefs.GetInt("SolutionsWatched")}\n";
    }
}
