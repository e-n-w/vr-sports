using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisBall : MonoBehaviour
{
    private bool isInPlay;
    private bool hasBeenServed;

    private int server;
    private int previousHit;

    [SerializeField]
    private float courtWidth;
    [SerializeField]
    private float courtLength;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isInPlay) 
        {
            return; 
        }
        if (!hasBeenServed) 
        {
            // Return the ball to the serving player
            return;
        }

        if (collision.gameObject.tag == "")
        {
            
        }
        else if(collision.gameObject.tag == "TennisLava" || collision.gameObject.tag == "TennisNet")
        {

        }

    }
}
