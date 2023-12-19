/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: GameController.cs

-------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//
// GameController:
//
// Used to control majority of the game functionality.
//
public class GameController : MonoBehaviour
{
    [SerializeField] GameObject operandsButtonsParent;  // parent for all operands buttons

    [SerializeField] Button operandButtonPrefab;        // prefab to make new operand buttons
    [SerializeField] Button[] operandsButtons;          // stores all operands buttons

    [SerializeField] Sprite selectedRedSprite;          // sprite for red selected button
    [SerializeField] Sprite notSelectedRedSprite;       // sprite for red unselected button
    [SerializeField] Sprite selectedBlueSprite;         // sprite for blue selected button
    [SerializeField] Sprite notSelectedBlueSprite;      // sprite for blue unselected button

    // Helper bool variables to track what buttons were already pressed or not
    private Button selectedOperandButton = null;    
    private Button selectedOperatorButton = null; 

    // Helper stack to store hidden operand buttons
    private Stack<Button> modifiedOperandsButtons = new Stack<Button>();

    // Helper integer variable used to set index for new operand buttons
    private int index = 5;

    void Start()
    {
        SetPuzzle();
    }

    //
    // SetPuzzle:
    //
    // Sets a new puzzle to the text object.
    //
    private void SetPuzzle()
    {
        // Get a new puzzle
        PuzzleController.UpdatePuzzle();

        for (int i = 0; i < 4; i++)
            operandsButtons[i].GetComponentInChildren<TMP_Text>().text = PuzzleController.puzzle[i].ToString();

        // Increase counter of attempted puzzles by one
        PlayerPrefs.SetInt("PuzzlesAttemted", PlayerPrefs.GetInt("PuzzlesAttemted") + 1);
    }

    //
    // SelectOperandButton:
    //
    // Manages what to do when operand button is pressed.
    //
    public void SelectOperandButton(Button operandButton)
    {
        // Case 1: none of operand buttons is selected
        if (selectedOperandButton == null)
        {
            // Change the operand button sprite to selected
            operandButton.GetComponent<Image>().sprite = selectedRedSprite;
            // Update operand helper variable
            selectedOperandButton = operandButton;
        }
        // Case 2: none of operator buttons is selected
        else if (selectedOperatorButton == null && selectedOperandButton != operandButton)
        {
            // Change selected operand button sprite to unselected and then other operand button sprite to selected
            selectedOperandButton.GetComponent<Image>().sprite = notSelectedRedSprite;
            operandButton.GetComponent<Image>().sprite = selectedRedSprite;
            // Update operand helper variable
            selectedOperandButton = operandButton;
        }
        // Case 3: pressed operand button differs from previously selected operand button
        else if (selectedOperandButton != operandButton)
        {
            // Reset previously selected operand button sprite to unselected 
            selectedOperandButton.GetComponent<Image>().sprite = notSelectedRedSprite;
            // Reset other selected operator button sprite to unselected
            selectedOperatorButton.GetComponent<Image>().sprite = notSelectedBlueSprite;

            // Get a result of operation
            string result = GameHelper.PerformOperation(
                new Fraction(selectedOperandButton.GetComponentInChildren<TMP_Text>().text),    // convert the operand button text to fraction
                new Fraction(operandButton.GetComponentInChildren<TMP_Text>().text),            // convert other operand button text to fraction
                char.Parse(selectedOperatorButton.GetComponentInChildren<TMP_Text>().text)      // convert the operator button text to fraction
            ).ToString();

            // If 6th operand button was created, finish the game
            if (index == 7)
            {
                // If division by zero is attemted, result will be empty
                if (result == string.Empty)
                    StartCoroutine(FinishGame("Zero"));
                else if (result == "24")
                    StartCoroutine(FinishGame("Win"));
                else
                    StartCoroutine(FinishGame("Loss"));
            }

            // Create a new operand button
            Button newOperandButton = Instantiate(
                operandButtonPrefab,                // prefab for a new button
                operandButton.transform.position,   // position of selected operand button
                Quaternion.identity,                // IDK
                operandsButtonsParent.transform     // transform property of parent
            );

            // Set a name for the new button in correct order
            newOperandButton.name = $"Operand {index++}";
            // Set a default sprite
            newOperandButton.GetComponent<Image>().sprite = selectedRedSprite;
            // Set the result as text of the new button
            newOperandButton.GetComponentInChildren<TMP_Text>().text = result;

            // Hide two used operand buttons
            selectedOperandButton.gameObject.SetActive(false);
            operandButton.gameObject.SetActive(false);
            // Show the new operand button
            newOperandButton.gameObject.SetActive(true);

            // Push all three operand buttons to helper stack
            modifiedOperandsButtons.Push(selectedOperandButton);
            modifiedOperandsButtons.Push(operandButton);
            modifiedOperandsButtons.Push(newOperandButton);

            // Select the new operand button
            selectedOperandButton = newOperandButton;
            // Reset operator helper variable
            selectedOperatorButton = null;
        }
    }

    //
    // SelectOperatorButton:
    //
    // Manages what to do when operator button is pressed.
    //
    public void SelectOperatorButton(Button operatorButton)
    {
        // Case 1: one operand button is selected
        if (selectedOperandButton != null && selectedOperatorButton != operatorButton)
        {
            // Change operator button sprite to selected
            operatorButton.GetComponent<Image>().sprite = selectedBlueSprite;

            // If needed, change previously selected operator button sprite to unselected
            if (selectedOperatorButton != null)
                selectedOperatorButton.GetComponent<Image>().sprite = notSelectedBlueSprite;

            // Update operator helper variable
            selectedOperatorButton = operatorButton;
        }
        // Default: do nothing
    }

    //
    // UndoOperation:
    //
    // Undoes previously made operations.
    //
    public void UndoOperation()
    {
        // Undo only if helper stack isn't empty
        if (modifiedOperandsButtons.Count > 0)
        {
            // Destroy a new operand button
            Destroy(modifiedOperandsButtons.Pop().gameObject);
            // Show two old operand buttons
            modifiedOperandsButtons.Pop().gameObject.SetActive(true);
            modifiedOperandsButtons.Pop().gameObject.SetActive(true);

            // Decrease the index for correct order
            index--;
        }
    }

    //
    // SkipPuzzle:
    //
    // Skips undesirable puzzle.
    //
    public void SkipPuzzle()
    {
        // Undo all previous operations
        for (int i = index - 4; i > 0; i--)
            UndoOperation();

        foreach (Button operandButton in operandsButtons)
            operandButton.GetComponent<Image>().sprite = notSelectedRedSprite;

        // Unselect the operator button if it's selected
        if (selectedOperatorButton != null)
        {
            // Change operator button sprite to unselected
            selectedOperatorButton.GetComponent<Image>().sprite = notSelectedBlueSprite;
            // Reset operator helper variable
            selectedOperatorButton = null;
        }
        // Reset operand helper variable
        selectedOperandButton = null;

        // Increase counter of skipped puzzles by one
        PlayerPrefs.SetInt("PuzzlesSkipped", PlayerPrefs.GetInt("PuzzlesSkipped") + 1);
        SetPuzzle();
    }

    //
    // FinishGame:
    //
    // Runs some code before finising the game.
    // 
    IEnumerator FinishGame(string code)
    {
        Button[] buttons = FindObjectsOfType<Button>();
        // Disable all buttons to prevent the player from doing anything
        foreach (Button button in buttons)
            if (button.name != "Mute/Unmute Button")
                button.enabled = false;

        // Assign gameCode value with according value
        GameHelper.gameCode = code;
        // Wait for 0.5 seconds to allow the player see the final result
        yield return new WaitForSeconds(0.5f);
        // Load AfterGame scene
        SceneManager.LoadScene("AfterGameScene");
    }
}