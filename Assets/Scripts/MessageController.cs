/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: MessageController.cs

-------------------------------------------------------*/

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//
// MessageController:
//
// Used to manage the after game message.
// 
public class MessageController : MonoBehaviour
{
    [SerializeField] TMP_Text messageText;          // message text
    [SerializeField] GameObject solutionsWidnow;    // pseudo canvas with solutions
    [SerializeField] Button showAdButton;           // show rewarded ad button

    [SerializeField] AudioSource audioSource;   // reference to audio source

    [SerializeField] AudioClip winAudio;        // win audio
    [SerializeField] AudioClip loseAudio;       // lose audio
    [SerializeField] AudioClip errorAudio;      // error audio

    void Start()
    {
        // Reset number of solutions left to see on next or later day
        if (PlayerPrefs.GetString("Date") != DateTime.Now.ToString("MM/dd/yyyy"))
        {
            PlayerPrefs.SetString("Date", DateTime.Now.ToString("MM/dd yyyy"));
            PlayerPrefs.SetInt("SolutionsLeft", 10);
        }

        // Show message depending on gameCode value
        switch (GameHelper.gameCode)
        {
            // Case 1: win
            case "Win":
                messageText.text = "Well done! Your solution yield 24!";
                audioSource.PlayOneShot(winAudio);

                // Increase counter of some difficulty solved puzzles by one
                switch (GameHelper.gameDifficulty)
                {
                    case "Hard":
                        PlayerPrefs.SetInt("HardPuzzles", PlayerPrefs.GetInt("HardPuzzles") + 1);
                        break;
                    case "Medium":
                        PlayerPrefs.SetInt("MediumPuzzles", PlayerPrefs.GetInt("MediumPuzzles") + 1);
                        break;
                    default:
                        PlayerPrefs.SetInt("EasyPuzzles", PlayerPrefs.GetInt("EasyPuzzles") + 1);
                        break;
                }
                // Increase counter of total solved puzzles by one
                PlayerPrefs.SetInt("TotalPuzzles", PlayerPrefs.GetInt("TotalPuzzles") + 1);
                break;
            // Case 2: lose
            case "Loss":
                messageText.text = "Unfortunately, your solution did not yield 24.";
                audioSource.PlayOneShot(loseAudio);
                break;
            // Case 3: give up
            case "GiveUp":
                messageText.text = "Do not give up so fast, try again!";
                audioSource.PlayOneShot(loseAudio);
                break;
            // Case 4: tried to divide by zero
            case "Zero":
                messageText.text = "Sorry, you can not divide by zero!";
                audioSource.PlayOneShot(errorAudio);
                break;
            // Default: something is wrong but how
            default:
                messageText.text = "Math is not mathing! Something is wrong!";
                audioSource.PlayOneShot(errorAudio);
                break;
        }
        // Reset gameCode
        GameHelper.gameCode = "Default";
    }

    //
    // GetSolutions:
    //
    // Finds all solutions to one puzzle.
    //
    private void GetSolutions()
    {
        // Show solutions only if solutions text field is empty
        if (solutionsWidnow.GetComponentInChildren<TMP_Text>().text == string.Empty)
        {
            // Print intro
            solutionsWidnow.GetComponentInChildren<TMP_Text>().text = "Solutions for: ";
            foreach (int num in PuzzleController.puzzle)
                solutionsWidnow.GetComponentInChildren<TMP_Text>().text += num.ToString() + "  ";
            solutionsWidnow.GetComponentInChildren<TMP_Text>().text += "\n";

            // Get puzzle operands
            int d1 = PuzzleController.puzzle[0];
            int d2 = PuzzleController.puzzle[1];
            int d3 = PuzzleController.puzzle[2];
            int d4 = PuzzleController.puzzle[3];

            int puzzlesCount = 0;

            // Loop through all possible expressions
            foreach (char o1 in "+-*/")
                foreach (char o2 in "+-*/")
                    foreach (char o3 in "+-*/")
                    {
                        string[] expressions = {
                            $"{d1} {o1} {d2} {o2} {d3} {o3} {d4}",
                            $"( {d1} {o1} {d2} ) {o2} {d3} {o3} {d4}",
                            $"{d1} {o1} ( {d2} {o2} {d3} ) {o3} {d4}",
                            $"{d1} {o1} {d2} {o2} ( {d3} {o3} {d4} )",

                            $"( {d1} {o1} {d2} {o2} {d3} ) {o3} {d4}",
                            $"{d1} {o1} ( {d2} {o2} {d3} {o3} {d4} )",

                            $"( ( {d1} {o1} {d2} ) {o2} {d3} ) {o3} {d4}",
                            $"( {d1} {o1} ( {d2} {o2} {d3} ) ) {o3} {d4}",
                            $"{d1} {o1} ( ( {d2} {o2} {d3} ) {o3} {d4} )",
                            $"{d1} {o1} ( {d2} {o2} ( {d3} {o3} {d4} ) )",

                            $"( {d1} {o1} {d2} ) {o2} ( {d3} {o3} {d4} )"
                        };
                        // Loop through all expressions for one operators combination
                        foreach (string expression in expressions)
                        {
                            if (GameHelper.EvaluateExpression(expression).ToString() == "24")
                            {
                                // Add solution to solutions text field up to 10 of them
                                if (puzzlesCount < 11)
                                    solutionsWidnow.GetComponentInChildren<TMP_Text>().text += GameHelper.FormatExpression(expression) + "\n";
                                puzzlesCount++;
                            }
                        }
                    }
            // Add how many more solutions exist if there is more than 11 solutions
            if (puzzlesCount > 11)
                solutionsWidnow.GetComponentInChildren<TMP_Text>().text += $"and {puzzlesCount - 11} more...\n";

            // Increase counter of solutions watched by one
            PlayerPrefs.SetInt("SolutionsWatched", PlayerPrefs.GetInt("SolutionsWatched") + 1);
            // Decrease counter of solutions left by one
            PlayerPrefs.SetInt("SolutionsLeft", PlayerPrefs.GetInt("SolutionsLeft") - 1);
        }
    }

    //
    // ShowSolutions:
    //
    public void ShowSolutions()
    {
        // Show solutions pseudo canvas
        solutionsWidnow.SetActive(true);
        // Show solutions if the player has some left
        if (PlayerPrefs.GetInt("SolutionsLeft") != 0)
            GetSolutions();
        // Otherwise show ad button
        else
        {
            solutionsWidnow.GetComponentInChildren<TMP_Text>().text = string.Empty;
            showAdButton.gameObject.SetActive(true);
        }
    }

    //
    // HideSolutions:
    //
    public void HideSolutions()
    {
        // Hide solutions pseudo canvas
        solutionsWidnow.SetActive(false);
    }
}
