/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: PuzzleController.cs

-------------------------------------------------------*/

using System;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 
// PuzzleController:
//
// Used to manage puzzle.
//
public class PuzzleController : MonoBehaviour
{
    // Global list with puzzles
    public static List<int[]> puzzlesList = new List<int[]>();
    // List size
    public static int puzzlesSize = 0;

    // Global puzzle
    public static int[] puzzle;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        // Reset both the list and size counter
        puzzlesList.Clear();
        puzzlesSize = 0;
    }

    //
    // UpdatePuzzle:
    //
    // Sets a new random puzzle from the list.
    //
    public static void UpdatePuzzle()
    {
        puzzle = puzzlesList[UnityEngine.Random.Range(0, puzzlesSize)];
    }

    //
    // GenPuzzles:
    //
    // Generates all possible puzzles and writes them into respective files.
    // NOTE: not used during the game, only used once before the build.
    //
    private void GenPuzzles()
    {
        // Hashsets used to avoid duplicates puzzles
        HashSet<string> easyPuzzles = new HashSet<string>();
        HashSet<string> mediumPuzzles = new HashSet<string>();
        HashSet<string> hardPuzzles = new HashSet<string>();

        // Loop through all possible operand combinations
        for (int d1 = -13; d1 < 14; d1++)
            for (int d2 = -13; d2 < 14; d2++)
                for (int d3 = -13; d3 < 14; d3++)
                    for (int d4 = -13; d4 < 14; d4++)
                    {
                        // Used to store all expressions for one set of operands
                        List<string> expressions = new List<string>();

                        // Loop through all possible operators combinations
                        foreach (char o1 in "+-*/")
                            foreach (char o2 in "+-*/")
                                foreach (char o3 in "+-*/")
                                    // Add expression to the list only if the puzzle doesn't already exist in any hashsets 
                                    if (!easyPuzzles.Contains($"{d1} {d2} {d3} {d4}") &&
                                        !mediumPuzzles.Contains($"{d1} {d2} {d3} {d4}") &&
                                        !hardPuzzles.Contains($"{d1} {d2} {d3} {d4}") &&
                                        d1 != 0 && d2 != 0 && d3 != 0 && d4 != 0)
                                    {
                                        // Add to the list all expression for one set of operators
                                        expressions.AddRange(new List<string> {
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
                                        });
                                    }
                        int puzzlesCount = 0;

                        // Count how many expressions yield 24
                        foreach (string expression in expressions)
                            if (GameHelper.EvaluateExpression(expression).ToString() == "24")
                                puzzlesCount++;

                        // Add the puzzle to hashset depending on how many expressions yield 24
                        if (puzzlesCount > 0)
                        {
                            if (puzzlesCount < 4)
                                hardPuzzles.Add($"{d1} {d2} {d3} {d4}");
                            else if (puzzlesCount < 11)
                                mediumPuzzles.Add($"{d1} {d2} {d3} {d4}");
                            else
                                easyPuzzles.Add($"{d1} {d2} {d3} {d4}");
                        }
                    }
        // Create streamwriter to write into the file
        StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/easySolutions.txt", true);
        // Add all easy puzzles from the hashset into respective file
        foreach (string expression in easyPuzzles)
            writer.Write(expression + "\n");
        // Close streamwriter
        writer.Close();

        // Reassign streamwriter to write into another file
        writer = new StreamWriter(Application.streamingAssetsPath + "/mediumSolutions.txt", true);
        // Add all medium puzzles from the hashset into respective file
        foreach (string expression in mediumPuzzles)
            writer.Write(expression + "\n");
        // Close streamwriter
        writer.Close();

        // Reassign streamwriter to write into another file
        writer = new StreamWriter(Application.streamingAssetsPath + "/hardSolutions.txt", true);
        // Add all hard puzzles from the hashset into respective file
        foreach (string expression in hardPuzzles)
            writer.Write(expression + "\n");
        // Close streamwriter
        writer.Close();
    }

    //
    // LoadPuzzles:
    //
    // Reads all puzzles from the files depending on the difficulty.
    //
    public void LoadPuzzles(Button button)
    {
        string filename;

        // Assign name of file according to the difficulty and set gameDifficulty value
        switch (button.GetComponentInChildren<TMP_Text>().text)
        {
            case "Pro":
                filename = "/hardSolutions.txt";
                GameHelper.gameDifficulty = "Hard";
                break;
            case "Advanced":
                filename = "/mediumSolutions.txt";
                GameHelper.gameDifficulty = "Medium";
                break;
            default:
                filename = "/easySolutions.txt";
                GameHelper.gameDifficulty = "Easy";
                break;
        }

        // Create streamreader to read the file
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + filename, true);
        string line;

        // Read all puzzles from the file and add them to the puzzles list
        while ((line = reader.ReadLine()) != null)
        {
            puzzlesList.Add(Array.ConvertAll(line.Split(), int.Parse));
            puzzlesSize++;
        }
    }
}
