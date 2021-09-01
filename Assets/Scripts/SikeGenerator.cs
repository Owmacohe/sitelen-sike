using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SikeGenerator : MonoBehaviour
{
    /* PUBLIC VARIABLLES */ 

    public Color circleColour; // colour of the circles/bars
    public CirclePresets circlePreset = CirclePresets.Thirty_Six_Sided; // the preset number of sides to each circle
    public enum CirclePresets { Twelve_Sided, Thirty_Six_Sided, One_Hundred_Twenty_Sided };

    /* CIRCLE PARAMETERS */

    private string circleWord; // the word to be translated
    private int circleSegments;  // number of sides to each circle
    private float circleThickness; // the thickness of the LineRenderers for the circles/bars
    private float circleRadius; // radius of the initial circle

    /* OTHER VARIABLES */

    private int[] segmentPresetValues = { 12, 36, 120 }; // integer values corresponding to the circle preset enum
    private List<GameObject> circleList; // list of all of the base letter circles
    private enum fullnesses { Outer, Inner, Whole } // bar types

    private void Start()
    {
        circleSegments = segmentPresetValues[(int)circlePreset];
        circleRadius = 4.5f;
    }

    /// <summary>
    /// Method to quit the program using a button
    /// </summary>
    public void quit()
    {
        Application.Quit(0);
    }

    /// <summary>
    /// Default method to be called to draw the circles and bars of each letter
    /// </summary>
    public void drawWord()
    {
        circleWord = GameObject.FindGameObjectWithTag("WordField").GetComponent<TMP_Text>().text;
        circleThickness = 1.4f / (circleWord.Length - 1); // The longer the word, the thinner the circles/bars

        // Making sure that all previous circles/bars are gone
        for (int childIndex = 0; childIndex < gameObject.transform.childCount; childIndex++)
        {
            Destroy(gameObject.transform.GetChild(childIndex).gameObject);
        }

        circleList = new List<GameObject>();

        float radiusInterval = circleRadius / (circleWord.Length - 1); // Segmenting the initial radius into equal sections

        // Drawing the inital circles for each letter
        for (int i = 0; i < (circleWord.Length - 1); i++)
        {
            drawCircle((circleWord.Length - 1 - i) * radiusInterval, false);
        }

        // Drawing the bars for each letter
        for (int j = 0; j < (circleWord.Length - 1); j++)
        {
            char sikeLetter = circleWord[j];

            switch (char.ToLower(sikeLetter))
            {
                case 'a':
                    // DOUBLE CIRCLE
                    drawCircle(circleList[j].transform.localScale.x - (radiusInterval / 2), true);
                    break;
                case 'e':
                    // NOTHING
                    break;
                case 'i':
                    // TOP BAR (OUTER)
                    drawBar(j, 0, fullnesses.Outer);
                    break;
                case 'j':
                    // TOP & BOTTOM BARS (OUTER)
                    drawBar(j, 0, fullnesses.Outer);
                    drawBar(j, circleSegments / 2, fullnesses.Outer);
                    break;
                case 'k':
                    // TOP, 1/3, AND 2/3 ANGLE BARS (OUTER)
                    drawBar(j, 0, fullnesses.Outer);
                    drawBar(j, circleSegments / 3, fullnesses.Outer);
                    drawBar(j, 2 * (circleSegments / 3), fullnesses.Outer);
                    break;
                case 'l':
                    // TOP, RIGHT, BOTTOM, AND LEFT BARS (OUTER)
                    drawBar(j, 0, fullnesses.Outer);
                    drawBar(j, circleSegments / 4, fullnesses.Outer);
                    drawBar(j, circleSegments / 2, fullnesses.Outer);
                    drawBar(j, 3 * (circleSegments / 4), fullnesses.Outer);
                    break;
                case 'm':
                    // TOP BAR (INNER)
                    drawBar(j, 0, fullnesses.Inner);
                    break;
                case 'n':
                    // TOP & BOTTOM BARS (INNER)
                    drawBar(j, 0, fullnesses.Inner);
                    drawBar(j, circleSegments / 2, fullnesses.Inner);
                    break;
                case 'o':
                    // TOP, 1/3, AND 2/3 ANGLE BARS (INNER)
                    drawBar(j, 0, fullnesses.Inner);
                    drawBar(j, circleSegments / 3, fullnesses.Inner);
                    drawBar(j, 2 * (circleSegments / 3), fullnesses.Inner);
                    break;
                case 'p':
                    // TOP, RIGHT, BOTTOM, AND LEFT BARS (INNER)
                    drawBar(j, 0, fullnesses.Inner);
                    drawBar(j, circleSegments / 4, fullnesses.Inner);
                    drawBar(j, circleSegments / 2, fullnesses.Inner);
                    drawBar(j, 3 * (circleSegments / 4), fullnesses.Inner);
                    break;
                case 's':
                    // TOP BAR (WHOLE)
                    drawBar(j, 0, fullnesses.Whole);
                    break;
                case 't':
                    // TOP & BOTTOM BARS (WHOLE)
                    drawBar(j, 0, fullnesses.Whole);
                    drawBar(j, circleSegments / 2, fullnesses.Whole);
                    break;
                case 'u':
                    // TOP, 1/3, AND 2/3 ANGLE BARS (WHOLE)
                    drawBar(j, 0, fullnesses.Whole);
                    drawBar(j, circleSegments / 3, fullnesses.Whole);
                    drawBar(j, 2 * (circleSegments / 3), fullnesses.Whole);
                    break;
                case 'w':
                    // TOP, RIGHT, BOTTOM, AND LEFT BARS (WHOLE)
                    drawBar(j, 0, fullnesses.Whole);
                    drawBar(j, circleSegments / 4, fullnesses.Whole);
                    drawBar(j, circleSegments / 2, fullnesses.Whole);
                    drawBar(j, 3 * (circleSegments / 4), fullnesses.Whole);
                    break;
            }
        }
    }

    /// <summary>
    /// Method to create a circular LineRenderer
    /// </summary>
    /// <param name="radius">Radius of teh new circle</param>
    /// <param name="isA">Whether the new circle is the interior part of an A character</param>
    private void drawCircle(float radius, bool isA)
    {
        // Creating and positioning the new circle
        GameObject circleObject = new GameObject("circle");
        circleObject.transform.localScale = Vector3.one * radius;
        circleObject.transform.SetParent(gameObject.transform);

        if (!isA)
        {
            circleList.Add(circleObject); // Adding it to the list of circles
        }

        // Creating and setting LineRenderer parameters
        LineRenderer circle = circleObject.AddComponent<LineRenderer>();
        circle.useWorldSpace = false;
        circle.loop = true;
        circle.material = (Material)Resources.Load("SikeMaterial");
        circle.material.color = circleColour;
        circle.startWidth = circleThickness;
        circle.positionCount = circleSegments;

        Vector3[] circlePositions = new Vector3[circle.positionCount];

        // Looping through each side, and setting the new position accordingly
        for (int i = 0; i < circlePositions.Length; i++)
        {
            float circleAngle = Mathf.Deg2Rad * (i * (360 / circleSegments)); // Getting the next angle

            circlePositions[i] = new Vector3(Mathf.Sin(circleAngle), Mathf.Cos(circleAngle), 0); // Using basic trigonometry to apply the angle and get the position
        }

        circle.SetPositions(circlePositions);
    }

    /// <summary>
    /// Method to create a linear LineRenderer
    /// </summary>
    /// <param name="circleIndex">The index in the letter array to which the bars should be attached</param>
    /// <param name="linePosition">Index on the respective circles' LineRenderers that the bar should span accross</param>
    /// <param name="fullness">What type of bar should be created</param>
    private void drawBar(int circleIndex, int linePosition, fullnesses fullness)
    {
        // Creating and positioning the new bar
        GameObject barObject = new GameObject("bar");
        barObject.transform.SetParent(circleList[circleIndex].transform);
        barObject.transform.localScale = Vector3.one;

        // Creating and setting LineRenderer parameters
        LineRenderer bar = barObject.AddComponent<LineRenderer>();
        bar.useWorldSpace = false;
        bar.material = (Material)Resources.Load("SikeMaterial");
        bar.material.color = circleColour;
        bar.startWidth = circleThickness;

        Vector3[] barPositions = new Vector3[2];

        // Setting the initial start and end positions of the bar based on their circle indexes

        barPositions[0] = barObject.transform.InverseTransformPoint(
            circleList[circleIndex].transform.TransformPoint(
                circleList[circleIndex].GetComponent<LineRenderer>().GetPosition(linePosition)
            )
        );

        if (circleIndex != (circleList.Count - 1))
        {
            barPositions[1] = barObject.transform.InverseTransformPoint(
                circleList[circleIndex + 1].transform.TransformPoint(
                    circleList[circleIndex + 1].GetComponent<LineRenderer>().GetPosition(linePosition)
                )
            );
        }
        // If there is no next circle, the end position is set to the centre
        else
        {
            barPositions[1] = Vector3.zero;
        }

        // Pulling either the start or end positions to the midpoint if the bar type is not whole
        if (fullness.Equals(fullnesses.Outer))
        {
            barPositions[1] = (barPositions[0] + barPositions[1]) / 2;
        }
        else if (fullness.Equals(fullnesses.Inner))
        {
            barPositions[0] = (barPositions[0] + barPositions[1]) / 2;
        }

        bar.SetPositions(barPositions);
    }
}
