/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: TutorialAnimator.cs

-------------------------------------------------------*/

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//
// TutorialAnimator:
//
// Used to animate the game tutorial in PreGame scene.
//
public class TutorialAnimator : MonoBehaviour
{
    [SerializeField] Button[] operandsButtons;  // stores all tutorial operand buttons
    [SerializeField] Button[] operatorsButtons; // stores all tutorial operators buttons

    [SerializeField] Sprite selectedRedSprite;      // sprite for red selected button
    [SerializeField] Sprite notSelectedRedSprite;   // sprite for red unselected button
    [SerializeField] Sprite selectedBlueSprite;     // sprite for blue selected button
    [SerializeField] Sprite notSelectedBlueSprite;  // sprite for blue unselected button

    void Start()
    {
        // Assign random integers in range from 1 to 9 to tutorial operand buttons text fields
        foreach (Button operandButton in operandsButtons)
            operandButton.GetComponentInChildren<TMP_Text>().text = Random.Range(1, 10).ToString();
        // Start the animation
        StartCoroutine(RunAnimation());
    }

    //
    // RunAnimation:
    //
    // Runs tutorial animation.
    // 
    IEnumerator RunAnimation()
    {
        // Helper variables
        int i, j;

        // Run animation indefinetely until scene is destroyed
        while (true)
        {
            // Run animation 10 times before resetting operand buttons
            for (int k = 0; k < 10; k++)
            {
                // Randomly choose one operand button
                i = Random.Range(0, 2);
                yield return new WaitForSeconds(1f);
                // Select that random operand button after one second
                operandsButtons[i].GetComponent<Image>().sprite = selectedRedSprite;
                yield return new WaitForSeconds(1f);
                // Randomly choose one operator button after one second
                j = Random.Range(0, 4);
                // Select that random operator button 
                operatorsButtons[j].GetComponent<Image>().sprite = selectedBlueSprite;
                yield return new WaitForSeconds(1f);
                // Select another operand button after one second
                operandsButtons[(i == 0) ? 1 : 0].GetComponent<Image>().sprite = selectedRedSprite;
                // Assign a new value for another operand button
                operandsButtons[(i == 0) ? 1 : 0].GetComponentInChildren<TMP_Text>().text = GameHelper.PerformOperation(
                    new Fraction(operandsButtons[i].GetComponentInChildren<TMP_Text>().text),                   // convert another operand button text to fraction
                    new Fraction(operandsButtons[(i == 0) ? 1 : 0].GetComponentInChildren<TMP_Text>().text),    // convert random operand button text to fraction
                    char.Parse(operatorsButtons[j].GetComponentInChildren<TMP_Text>().text)                     // convert the operator button text to fraction
                ).ToString();
                // Hide random operand button
                operandsButtons[i].gameObject.SetActive(false);
                // Unselect that random operator button
                operatorsButtons[j].GetComponent<Image>().sprite = notSelectedBlueSprite;
                yield return new WaitForSeconds(1f);
                // Show hidden operand button after one second
                operandsButtons[i].gameObject.SetActive(true);
                // Unselect that operand button
                operandsButtons[i].GetComponent<Image>().sprite = notSelectedRedSprite;
                // Assign random integers in range from 1 to 9 to that operand button
                operandsButtons[i].GetComponentInChildren<TMP_Text>().text = Random.Range(1, 10).ToString();
                // Unselect another operand button
                operandsButtons[(i == 0) ? 1 : 0].GetComponent<Image>().sprite = notSelectedRedSprite;
                yield return new WaitForSeconds(1f);
            }
            // Reset operand buttons
            foreach (Button operandButton in operandsButtons)
                operandButton.GetComponentInChildren<TMP_Text>().text = Random.Range(1, 10).ToString();
        }
    }
}
