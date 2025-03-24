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
        CreatePlayerFrameHistories();
    }
    void CreatePlayerFrameHistories()
    {
        playerScore = new BowlingPlayerFrameHistory[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            playerScore[i] = new BowlingPlayerFrameHistory();
        }
    }
    void Update()
    {
        BowlingPlayerFrameHistory history = new BowlingPlayerFrameHistory();
    }

    public void UpdateLane()
    {
      
    }

    public int CalculateScore (GameObject player)
    {
        int newBowl = 0;
        for (int i = 0; i < bowlingPins.Length; i++)
        {
            if (bowlingPins[i].GetLaneStatus() && !bowlingPins[i].GetStandingStatus())
            {
                newBowl +=1;
            }
        }
        return newBowl;
    }


    private void UpdateScore(GameObject player)
    {
        
    }
}
