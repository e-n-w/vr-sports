using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TennisBall : MonoBehaviour
{
    [SerializeField]
    private TennisManager tennisManager;

    public bool isInHand = true;
    public bool hasBeenServed = false;

    [SerializeField]
    private int previousHit;
    public bool previousHitWasRacket;

    [SerializeField]
    private GameObject court;

    [SerializeField]
    private float courtWidth;
    [SerializeField]
    private float courtLength;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenServed) 
        {
            if(collision.gameObject.CompareTag("TennisRacket"))
            {
                previousHit = tennisManager.Server;
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

    public void OnSelected(SelectEnterEventArgs args)
    {
        isInHand = true;
        rb.isKinematic = true;
    }

    public void OnDeselect(SelectExitEventArgs args)
    {
        isInHand = false;
        rb.isKinematic = false;
    }
}
