using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingManager : MonoBehaviour
{
    // I have been working in inches through this entire script
    // bowling lanes are always 60ft to the first pin
    // a four row lane has a width of 41.5 inches, so I will use it to scale each additional row
    // Pin distance is scaled within the spacing method
    
    [SerializeField] private float laneDistance = 720f, laneWidthPerRow = 10.375f, extraLaneDepthPerRow = 8.5468f, gutterDepth = 1.875f, gutterWidth = 9.25f, pinDistance = 12f, orthogonalPinDistance, scalar = 1f;
    [SerializeField] private int numPlayers, numPins = 10;
    [SerializeField] private List<GameObject> bowlingPins;
    [SerializeField] private GameObject pinPrefab, gutterLeft, gutterRight, localPlayer;
    private BowlingPlayerFrameHistory[] playerScore;
    

    void Start()
    {
        CreatePlayerFrameHistories();
        
        InstantiatePins();
        ScalePinDistance(.5f);
        CalculatePinDistanceOrthogonality();
        SpacePins();
    }

  
    void CreatePlayerFrameHistories()
    {
        playerScore = new BowlingPlayerFrameHistory[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            playerScore[i] = new BowlingPlayerFrameHistory();
        }
    }
 
    

    void InstantiatePins()
    {
        for (int i = 0; i < numPins; i++) 
        {
            bowlingPins.Add(Instantiate(pinPrefab));
        }
        
    }

    void ScalePinDistance(float scalar)
    {
        pinDistance *= scalar;
    }

    void CalculatePinDistanceOrthogonality()
    {
        orthogonalPinDistance = MathF.Sqrt((pinDistance * pinDistance) - (pinDistance * pinDistance / 4));
        Debug.Log(orthogonalPinDistance);
    }

    
   
    void SpacePins() 

        //Note: this will also size the bowling lane
        //Pins are spaced in an equilateral triangle so this janky method should allow for spacing and pin count alterations
        //The pin asset I used is oriented sideways and I used Y instead of Z to reflect this
    {
        float relativeX = 0, relativeY = 0, leftBound = 0;
        int pinInRow = 1, row = 1;

        for (int i = 0; i < bowlingPins.Count; i++)
        {
            if (pinInRow > row)
            {
                pinInRow = 1;
                row++;
                leftBound -= pinDistance  / 2;
                relativeX = leftBound;
                relativeY += orthogonalPinDistance;
            }
            bowlingPins[i].transform.Translate(new Vector3(relativeX, relativeY, 0));
            relativeX += pinDistance ;
            pinInRow++;
        }
        //gutterLeft.transform.position = new Vector3(-laneWidthPerRow * row / 2, 0, 0);
        //gutterRight.transform.position = new Vector3(laneWidthPerRow * row / 2, 0, 0);




    }
    public void UpdateLane()
    {

    }

    public int CalculateScore(GameObject player)
    {
        int newBowl = 0;
        for (int i = 0; i < bowlingPins.Count; i++)
        {
            if (bowlingPins[i].GetComponent<BowlingPinBehaviour>().isOnLane && !bowlingPins[i].GetComponent<BowlingPinBehaviour>().isStanding)
            {
                newBowl += 1;
            }
        }
        return newBowl;
    }


    private void UpdateScore(GameObject player)
    {
        
    }
}

