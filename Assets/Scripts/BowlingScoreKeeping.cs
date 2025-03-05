using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BowlingScoreKeeping : MonoBehaviour
{

    private int numPlayers;
    private BowlingPinBehaviour[] bowlingPins;
    private BowlingPlayerFrameHistory[] playerScore;

    void Start()
    {
        numPlayers = 0;
        playerScore = new BowlingPlayerFrameHistory[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            playerScore[i] = new BowlingPlayerFrameHistory();
        }
    }

    void Update()
    {
        
    }

    public int CalculateBowl (GameObject player)
    {
        int newBowl = 0;
        for (int i = 0; i < bowlingPins.Length; i++)
        {
            if (bowlingPins[i].GetStandingStatus())
            {
                newBowl +=1;
            }
        }
        return newBowl;
    }
    
    public int CalculateBowl(GameObject player, int lastBowl)
        {
            for (int i = 0; i < bowlingPins.Length; i++)
            {
                if (bowlingPins[i].GetStandingStatus())
                {
                    lastBowl += 1;
                }
            }
            return lastBowl;
        }


    private void UpdateScore(GameObject player)
    {
        
    }
}
