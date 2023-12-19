/*-----------------------------------------------------

    Final project: twenty4

    Course: CS 50G
    System: MacOS using Unity and Visual Studio
    Author: Kaito Sekiya
 
    File: Fraction.cs

-------------------------------------------------------*/

using System;

//
// Fraction:
//
// Custom class for fraction numbers abstraction.
// NOTE: it isn't a complete class, it has a bare minimum for the game.
//
public class Fraction
{
    private int numerator;      
    private int denominator;

    //
    // Default constructor:
    //
    // Construct 0/1 fraction which is 0.
    //
    public Fraction()
    {
        numerator = 0;
        denominator = 1;
    }

    //
    // Parameterized constructor:
    //
    // Construct fraction with numerator set by the user.
    // 
    public Fraction(int numerator)
    {
        this.numerator = numerator;
        denominator = 1;
    }

    //
    // Parameterized constructor:
    //
    // Construct fraction with numerator and denominator set by the user.
    // 
    public Fraction(int numerator, int denominator)
    {
        this.numerator = numerator;
        this.denominator = denominator;
    }

    //
    // Copy constructor:
    //
    // Construct a copy of existing fraction.
    // 
    public Fraction(Fraction fraction)
    {
        numerator = fraction.numerator;
        denominator = fraction.denominator;
    }

    //
    // Parameterized constructor:
    //
    // Construct fraction from string.
    // NOTE: can't hold any edge cases.
    // 
    public Fraction(string fraction)
    {
        // String is already a fraction
        if (fraction.Contains("/"))
        {
            numerator = Int32.Parse(fraction[0..fraction.IndexOf("/")]);
            denominator = Int32.Parse(fraction[(fraction.IndexOf("/") + 1)..]);
        }
        // String is an integer
        else
        {
            numerator = Int32.Parse(fraction);
            denominator = 1;
        }
    }

    //
    // Equals:
    //
    // Used for comparison and required by the system.
    //
    public override bool Equals(object obj)
    {
        Fraction other = obj as Fraction;
        return (numerator == other.numerator && denominator == other.denominator);
    }

    //
    // operator ==:
    //
    // Returns true if two fractions are equal and false otherwise.
    // 
    public static bool operator ==(Fraction f1, Fraction f2)
    {
        return f1.Equals(f2);
    }

    //
    // operator !=:
    //
    // Returns true if two fractions aren't equal and false otherwise.
    //
    public static bool operator !=(Fraction f1, Fraction f2)
    {
        return !(f1 == f2);
    }

    //
    // operator +:
    //
    // Returns a new fraction which is sum of given two fractions.
    //
    public static Fraction operator +(Fraction f1, Fraction f2)
    {
        return new Fraction(f1.numerator * f2.denominator + f2.numerator * f1.denominator, f1.denominator * f2.denominator);
    }

    //
    // operator -:
    //
    // Returns a new fraction which is difference of given two fractions.
    //
    public static Fraction operator -(Fraction f1, Fraction f2)
    {
        return new Fraction(f1.numerator * f2.denominator - f2.numerator * f1.denominator, f1.denominator * f2.denominator);
    }

    //
    // operator *:
    //
    // Returns a new fraction which is product of given two fractions.
    //
    public static Fraction operator *(Fraction f1, Fraction f2)
    {
        return new Fraction(f1.numerator * f2.numerator, f1.denominator * f2.denominator);
    }

    //
    // operator /:
    //
    // Returns a new fraction which is quotient of given two fractions.
    //
    public static Fraction operator /(Fraction f1, Fraction f2)
    {
        return new Fraction(f1.numerator * f2.denominator, f1.denominator * f2.numerator);
    }

    //
    // GetHashCode:
    //
    // Returns a hash code for the fraction.
    // NOTE: honestly, I don't know why the system requires it.
    //
    public override int GetHashCode()
    {
        return numerator * denominator;
    }

    //
    // ToString:
    //
    // Returns a fraction in string format.
    //
    public override string ToString()
    {
        // Return integer if the fraction is an integer
        if (denominator == 1)
            return numerator.ToString();
        // Otherwise return the fraction in fraction form
        else
            return numerator + "/" + denominator;
    }

    //
    // Simplify:
    //
    // Simplifies given fraction.
    //
    public Fraction Simplify()
    {
        for (int divider = denominator; divider > 0; divider--)
        {
            // Fancy formula to check if both numerator and denominator have a common divider I copied from Internet
            bool divisible = ((int)(numerator / divider) * divider == numerator) && ((int)(denominator / divider) * divider == denominator);

            if (divisible)
            {
                numerator /= divider;
                denominator /= divider;
            }
        }
        // TODO: if both numerator and denominator are negative, cancel negative sign

        return this;
    }
}