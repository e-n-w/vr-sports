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

    public readonly float RacketCooldownBase = 250;
    private bool RacketCooldownActive = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CoolDown()
    {
        RacketCooldownActive = true;
        yield return new WaitForSeconds(RacketCooldownBase / 1000f);
        RacketCooldownActive = false;
    }
    void OnCollisionEnter(Collision collision)
    {

        if (!hasBeenServed) 
        {
            if(collision.gameObject.CompareTag("TennisRacket"))
            {
                previousHit = tennisManager.Server;
                previousHitWasRacket = true;
                hasBeenServed = true;
                StartCoroutine(CoolDown());
                return;
            }

            // Return the ball to the serving player
            tennisManager.ReturnBallToServer();

            return;
        }

        if (collision.gameObject.CompareTag("TennisCourt")) CourtWasHit(collision);

        else if (collision.gameObject.CompareTag("TennisRacket")) RacketWasHit(collision);

        else if(collision.gameObject.CompareTag("TennisLava") || collision.gameObject.CompareTag("TennisNet"))
        {
            previousHitWasRacket = false;
            tennisManager.ScorePoint(previousHit == 0 ? 1 : 0);
        }
    }
    private void CourtWasHit(Collision collision)
    {
        previousHitWasRacket = false;
        Vector3 collisionPoint = collision.GetContact(0).point;
        if (IsPointInCourt(collisionPoint))
        {
            if (previousHit == 0 && collisionPoint.x >= 0)
            {
                tennisManager.ScorePoint(1);
            }
            else if (previousHit == 1 && collisionPoint.x < 0)
            {
                tennisManager.ScorePoint(0);
            }
            else
            {
                previousHit = collisionPoint.x > 0 ? 0 : 1;
            }
        }
        else
        {
            tennisManager.ScorePoint(previousHit == 0 ? 1 : 0);
        }
    }

    private void RacketWasHit(Collision collision)
    {
        if (RacketCooldownActive)
        {
            return;
        }
        previousHitWasRacket = true;
        int playerId = collision.gameObject.GetComponent<TennisRacket>().PlayerId;
        if ((playerId == previousHit) && previousHitWasRacket)
        {
            tennisManager.ScorePoint(playerId == 0 ? 1 : 0);
        }
        previousHit = playerId;
    }

    private bool IsPointInCourt(Vector3 point)
    {
        Vector3 courtOrigin = court.transform.position;
        if (point.x < courtOrigin.x + courtLength / 2 && point.x > courtOrigin.x - courtLength / 2 && point.y < courtOrigin.y + courtWidth / 2 && point.y > courtOrigin.y - courtWidth / 2)
        {
            return true;
        }
        else
        {
            return false;
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
