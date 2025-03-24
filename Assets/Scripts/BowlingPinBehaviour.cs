using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPinBehaviour : MonoBehaviour
{
    private bool isStanding;
    private bool isOnLane;
    
    public bool GetStandingStatus()
    {
        return isStanding;
    }
    public bool GetLaneStatus()
    {
        return isOnLane;
    }
}
