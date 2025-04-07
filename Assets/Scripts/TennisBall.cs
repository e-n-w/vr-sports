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
                previousHits.Add(new TennisHit(collision.gameObject.GetComponent<TennisRacket>().Player, HitSurface.Racket));
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
            TennisPlayer scorer = GetLastHit(p => p.player.Id != GetLastHit().player.Id)?.player;
            tennisManager.ScorePoint(scorer, GetLastHit().player);
            previousHits = new List<TennisHit>();
        }
        else if (collision.gameObject.CompareTag("TennisRacket") || collision.gameObject.CompareTag("TennisCourt"))
        {
            bool wasRacket = collision.gameObject.CompareTag("TennisRacket");
            if (wasRacket && RacketCooldownActive)
            {
                return;
            }

            TennisPlayer thisPlayer = wasRacket ? collision.gameObject.GetComponent<TennisRacket>().Player : collision.gameObject.GetComponent<TennisCourt>().AssociatedPlayer;
            TennisHit lastHit = GetLastHit();

            Debug.Log($"{thisPlayer.Id} | {lastHit.player.Id}");
            if(lastHit.player.Id == thisPlayer.Id && (wasRacket && lastHit.hitSurface == HitSurface.Racket))
            {
                tennisManager.ScorePoint(GetLastHit(p => p.player.Id != thisPlayer.Id)?.player, thisPlayer);
                previousHits = new List<TennisHit>();
            }
            else
            {
                previousHits.Add(new TennisHit(thisPlayer, wasRacket ? HitSurface.Racket : HitSurface.Court));
            }
        }
        else
        {
            tennisManager.ReturnBallToServer();
        }
    }

    private TennisHit GetLastHit(Func<TennisHit, bool> predicate)
    {
        try
        {
            return previousHits.Last(predicate);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    private TennisHit GetLastHit()
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