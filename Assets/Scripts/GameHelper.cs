/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: GameHelper.cs

-------------------------------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//
// GameHelper:
//
// Used to support the game functionality.
//
public class GameHelper
{
    // Global helper variables
    public static string gameCode = "Default";
    public static string gameDifficulty = "Easy";

    //
    // IsOperator:
    //
    // Returns true if given char is an operator and false otherwise.
    //
    private static bool IsOperator(char c)
    {
        return "+-*/".Contains(c);
    }

    //
    // GetPrecedence:
    //
    // Returns precedence of given operator.
    //
    private static int GetPrecedence(char op)
    {
        switch (op)
        {
            case '+':
            case '-':
                return 1;
            case '*':
            case '/':
                return 2;
        }
        return 0;
    }

    //
    // PerformOperation:
    //
    // Returns result of the operation.
    // Asks for two fractions and operator.
    // Used during the game.
    //
    public static Fraction PerformOperation(Fraction a, Fraction b, char op)
    {
        Fraction result = new Fraction();

        switch (op)
        {
            case '+':
                result = a + b;
                break;
            case '-':
                result = a - b;
                break;
            case '*':
                result = a * b;
                break;
            case '/':
                // Can't divide by zero
                if (b == new Fraction(0))
                {
                    // Assign gameCode value
                    gameCode = "Zero";
                    // Switch to AfterGame scene immediatly
                    SceneManager.LoadScene("AfterGameScene");
                }   
                result = a / b;
                break;
        }
        return result.Simplify();   // simplify resulting fraction
    }

    //
    // PerformOperation:
    //
    // Returns result of the operation.
    // Asks for operands stack and operator.
    // Used during puzzles generation. 
    //
    public static Fraction PerformOperation(ref Stack<Fraction> operands, char op)
    {
        Fraction result = new Fraction();
        // Get operands from operands stack and convert them to fraction
        Fraction a = new Fraction(operands.Pop());
        Fraction b = new Fraction(operands.Pop());

        switch (op)
        {
            case '+':
                result = a + b;
                break;
            case '-':
                result = a - b;
                break;
            case '*':
                result = a * b;
                break;
            case '/':
                // Can't divide by zero
                if (a == new Fraction(0))
                    // So return very big value to ensure it won't yield 24 in any way
                    return new Fraction(int.MaxValue / 1000);

                result = b / a;
                break;
        }
        return result;
    }

    //
    // EvaluateExpression:
    //
    // Returns result of the expression.
    // Used during puzzles generation. 
    //
    public static Fraction EvaluateExpression(string expression)
    {
        // Using stacks allows evaulate expressions with parentheses
        Stack<Fraction> operands = new Stack<Fraction>();
        Stack<char> operators = new Stack<char>();

        // Split expression into tokens separated by whitespaces
        string[] tokens = expression.Split(" ");

        // Helper variable
        int num;

        // Loop through all tokens
        foreach (string token in tokens)
        {
            // Case 1: token is an integer
            if (int.TryParse(token, out num))
                operands.Push(new Fraction(num));
            else
            {
                // Get char since it isn't an integer for sure
                char c = char.Parse(token);

                // Case 2: token is '('
                if (c == '(')
                    operators.Push(c);
                // Case 3: token is ')'
                else if (c == ')')
                {
                    // Perform operations if operators stack is not empty and another '(' isn't met
                    while (operators.Count != 0 && operators.Peek() != '(')
                        operands.Push(PerformOperation(ref operands, operators.Pop()));
                    operators.Pop();
                }
                // Case 4: token is an operator  
                else if (IsOperator(c))
                {
                    // Perform operations if operators stack is not empty and correct precedence is complied
                    while (operators.Count != 0 && GetPrecedence(c) <= GetPrecedence(operators.Peek()))
                        operands.Push(PerformOperation(ref operands, operators.Pop()));
                    operators.Push(c);
                }
            }
        }
        // Perform left operations until operators stack is empty
        while (operators.Count != 0)
            operands.Push(PerformOperation(ref operands, operators.Pop()));

        return operands.Pop().Simplify();   // don't forget to simplify the result
    }

    //
    // FormatExpression:
    //
    // Converts expression in more readable and nicer format for the player.
    // 
    public static string FormatExpression(string expression)
    {
        // New yet empty expression
        string formattedExpression = string.Empty;

        // Split expression into tokens separated by whitespaces
        string[] tokens = expression.Split(" ");

        // Helper variable
        int num;

        // Loop through all tokens
        foreach (string token in tokens)
        {
            // Case 1: token is an integer
            if (int.TryParse(token, out num))
            {
                // Add parentheses if the integer is negative
                if (num < 0)
                    formattedExpression += $"({num})";
                else
                    formattedExpression += num.ToString();
            }
            else
            {
                // Get char since it isn't an integer for sure
                char c = char.Parse(token);

                // Case 2: token is '(' or ')'
                if (c == '(' || c == ')')
                    formattedExpression += c.ToString();
                // Case 3: token is an operator
                else if (IsOperator(c))
                    // Add extra whitespace before the operator
                    formattedExpression += $" {c} ";
            }
        }
        return formattedExpression;
    }
}