using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPlayerFrameHistory : MonoBehaviour
{
    private int lastBowl;
    private int lastLastBowl;
    private int[] frames = new int[10];
    
        public BowlingPlayerFrameHistory() {
        lastBowl = 
            lastBowl = 0;
            lastLastBowl = 0;
    }

    public int GetLastBowl()
    {
        return lastBowl;     
    }

    public int GetLastLastBowl()
    {
        return lastLastBowl;
    }

    public void SetLastBowl(int score)
    {
        lastLastBowl = lastBowl;
        lastBowl = score;
    }
}
