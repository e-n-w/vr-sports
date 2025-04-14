using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TennisBall : NetworkBehaviour
{
    [SerializeField]
    private TennisManager tennisManager;
    private List<TennisHit> previousHits = new();
    private Rigidbody rb;

    public bool hasBeenServed = false;
    private bool RacketCooldownActive = false;

    public readonly float RacketCooldownBase = 250;

    public static TennisBall Instance;

    private void Awake() => rb = gameObject.GetComponent<Rigidbody>();

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
            Name(collision);
            return;
        }

        if (collision.gameObject.CompareTag("TennisOut"))
        {
            TennisPlayer scorer = GetLastHit(p => p.player.Id != GetLastHit().player.Id)?.player;
            tennisManager.ScorePoint(scorer, GetLastHit().player);
            previousHits = new List<TennisHit>();
            return;
        }
        
        if (collision.gameObject.CompareTag("TennisRacket") || collision.gameObject.CompareTag("TennisCourt"))
        {
            bool wasRacket = collision.gameObject.CompareTag("TennisRacket");

            if (wasRacket && RacketCooldownActive) return;

            TennisPlayer thisPlayer = wasRacket ? collision.gameObject.GetComponent<TennisRacket>().Player : collision.gameObject.GetComponent<TennisCourt>().AssociatedPlayer;
            TennisHit lastHit = GetLastHit();

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

    void Name(Collision collision)
    {
        if(!collision.gameObject.CompareTag("TennisRacket")) { tennisManager.ReturnBallToServer();  return; }

        previousHits.Add(new TennisHit(collision.gameObject.GetComponent<TennisRacket>().Player, HitSurface.Racket));
        hasBeenServed = true;
        StartCoroutine(CoolDown());
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
        rb.isKinematic = true;
    }

    public void OnDeselect(SelectExitEventArgs args)
    {
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