using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TennisBall : MonoBehaviour
{
    [SerializeField]
    private TennisManager tennisManager;

    public bool isInHand = true;
    public bool hasBeenServed = false;

    private Rigidbody rb;

    public readonly float RacketCooldownBase = 250;
    private bool RacketCooldownActive = false;

    private List<TennisHit> previousHits = new();

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private IEnumerator CoolDown()
    {
        RacketCooldownActive = true;
        yield return new WaitForSeconds(RacketCooldownBase / 1000f);
        RacketCooldownActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenServed)
        {
            if (collision.gameObject.CompareTag("TennisRacket"))
            {
                CycleHits(new TennisHit(collision.gameObject.GetComponent<TennisRacket>().Player, HitSurface.Racket));
                hasBeenServed = true;
                StartCoroutine(CoolDown());
                return;
            }

            // Return the ball to the serving player
            tennisManager.ReturnBallToServer();

            return;
        }

        if (collision.gameObject.CompareTag("TennisOut"))
        {
            tennisManager.ScorePoint(LastHit().player);
            previousHits = new List<TennisHit>();
        }
        else if (collision.gameObject.CompareTag("TennisRacket"))
        {
            if (RacketCooldownActive)
            {
                return;
            }

            TennisPlayer thisPlayer = collision.gameObject.GetComponent<TennisRacket>().Player;
            TennisHit lastHit = LastHit();
            if(lastHit.player.Id == thisPlayer.Id && lastHit.hitSurface == HitSurface.Racket)
            {
                tennisManager.ScorePoint(previousHits.Last(p => p.player.Id != thisPlayer.Id).player);
                previousHits = new List<TennisHit>();
            }
            else
            {
                CycleHits(new TennisHit(thisPlayer, HitSurface.Racket));
            }
        }
        else if (collision.gameObject.CompareTag("TennisCourt"))
        {
            TennisPlayer thisPlayer = collision.gameObject.GetComponent<TennisCourt>().AssociatedPlayer;
            TennisHit lastHit = LastHit();
            if(lastHit.player.Id == thisPlayer.Id)
            {
                tennisManager.ScorePoint(previousHits.Last(p => p.player.Id != thisPlayer.Id).player);
            }
            else
            {
                CycleHits(new TennisHit(thisPlayer, HitSurface.Court));
            }
        }
        else
        {
            tennisManager.ReturnBallToServer();
        }
    }

    private void CycleHits(TennisHit newHit)
    {
        if (previousHits.Count < 3)
        { 
            previousHits.Add(newHit);
        }
        else
        {
            previousHits[0] = previousHits[1];
            previousHits[1] = previousHits[2];
            previousHits[2] = newHit;
        }
    }

    private TennisHit LastHit()
    {
        return previousHits.Last();
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

public enum HitSurface
{
    Court,
    Racket
}

public class TennisHit
{
    public readonly TennisPlayer player;
    public readonly HitSurface hitSurface;

    public TennisHit(TennisPlayer player, HitSurface surface)
    {
        this.player = player;
        this.hitSurface = surface;
    }

}