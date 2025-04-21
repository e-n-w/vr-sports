using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BallReturnBehaviour : MonoBehaviour
{

    [SerializeField] private XRSocketInteractor[] bowlingBallSockets;
    [SerializeField] private int maxSockets = 4;
    private int socketsFilled;
    void Start()
    {
        bowlingBallSockets = new XRSocketInteractor[4];
        bowlingBallSockets = GetComponentsInChildren<XRSocketInteractor>(true);
        
    }
    

    void FixedUpdate()
    {
        
    }

    

    public void ActivateNextSocket()
    {
        socketsFilled++;
        if (socketsFilled < maxSockets) bowlingBallSockets[socketsFilled].gameObject.SetActive(true);
    }

    public void DeactivateNextSocket() 
    {
        
        if (socketsFilled < maxSockets) bowlingBallSockets[socketsFilled].gameObject.SetActive(false);
        socketsFilled--;
    }
    
}
