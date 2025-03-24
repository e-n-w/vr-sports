using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPlayerFrameHistory : MonoBehaviour
{
    private int lastBowl, lastLastBowl;
    private int[,] frames = new int[10, 3];
    public int SomeData { get; private set; }

    public BowlingPlayerFrameHistory() 
    {
        lastBowl = 0;
        lastLastBowl = 0;
    }

    public int GetLastBowl() => lastBowl;     
    public int GetLastLastBowl() => lastLastBowl;

    public void SetLastBowl(int score)
    {
        lastLastBowl = lastBowl;
        lastBowl = score;
    }
}
