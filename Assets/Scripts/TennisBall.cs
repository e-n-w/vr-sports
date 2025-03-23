using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisBall : MonoBehaviour
{
    [SerializeField]
    private TennisManager tennisManager;
    [SerializeField]
    private bool isInHand = true;

    public bool hasBeenServed = false;

    public int server;

    [SerializeField]
    private int previousHit;
    private bool previousHitWasRacket;

    [SerializeField]
    private GameObject court;

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
        if (isInHand) 
        {
            return; 
        }
        if (!hasBeenServed) 
        {
            if(collision.gameObject.CompareTag("TennisRacket"))
            {
                previousHit = server;
                previousHitWasRacket = true;
            }

            // Return the ball to the serving player


            return;
        }

        if (collision.gameObject.CompareTag("TennisCourt"))
        {
            previousHitWasRacket = false;
            Vector3 point = collision.GetContact(0).point;
            Vector3 courtOrigin = court.transform.position;
            if (point.x < courtOrigin.x + courtLength / 2 && point.x > courtOrigin.x - courtLength / 2 && point.y < courtOrigin.y + courtWidth / 2 && point.y > courtOrigin.y - courtWidth / 2)
            {
                if(previousHit == 0 && point.x >= 0)
                {
                    tennisManager.ScorePoint(1);
                }
                else if(previousHit == 1 && point.x < 0)
                {
                    tennisManager.ScorePoint(0);
                }
                else
                {
                    previousHit = point.x > 0 ? 0 : 1;
                }
            }
            else
            {
                tennisManager.ScorePoint(previousHit == 0 ? 1 : 0);
            }
        }
        else if (collision.gameObject.CompareTag("TennisRacket"))
        {
            previousHitWasRacket = true;
            int playerId = collision.gameObject.GetComponent<TennisRacket>().PlayerId;
            if (playerId == previousHit && previousHitWasRacket)
            {
                tennisManager.ScorePoint(playerId == 0 ? 1 : 0);
            }
        }
        else if(collision.gameObject.CompareTag("TennisLava") || collision.gameObject.CompareTag("TennisNet"))
        {
            previousHitWasRacket = false;
            tennisManager.ScorePoint(previousHit == 0 ? 1 : 0);
        }
    }
}
