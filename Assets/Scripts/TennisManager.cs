using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using System.Linq;
using System;

public class TennisManager : MonoBehaviour
{ 
    [SerializeField]
    private TennisBall ballScript;

    [SerializeField]
    private GameObject courtPrefab;
    [SerializeField] 
    private GameObject netPrefab;

    [SerializeField]
    private float courtWidth;
    [SerializeField]
    private float courtLength;

    [SerializeField, Range(2, 6)]
    private int NumPlayers;
    private List<TennisPlayer> players;

    [SerializeField]
    private GameObject hostPlayerRig;

    public TennisPlayer Server { get; private set; }

    public void StartMatch(int numberOfPlayers)
    {
        NumPlayers = numberOfPlayers;
        players = new List<TennisPlayer>();
        float distanceFromCenter = courtWidth / (2 * Mathf.Tan(Mathf.PI/NumPlayers)) + courtLength/2;
        for (int i = 0; i < NumPlayers; i++)
        {
            players.Add(new TennisPlayer(i));
            GameObject courtPart = Instantiate(courtPrefab);
            courtPart.transform.eulerAngles = new Vector3(90, 360 / NumPlayers*i, 0);
            courtPart.transform.localScale = new Vector3(courtLength, courtWidth, 0.25f);
            courtPart.transform.Translate(distanceFromCenter * courtPart.transform.right, Space.World);
            courtPart.GetComponent<TennisCourt>().AssociatedPlayer = players[i];

            if(i == 0)
            {
                hostPlayerRig.transform.position = courtPart.transform.position;
            }

            GameObject net = Instantiate(netPrefab);
            net.transform.eulerAngles = new Vector3(0, 90 + 360 / NumPlayers * i, 0);
            net.transform.localScale = new Vector3(courtWidth, 1, 0.25f);
            net.transform.Translate((distanceFromCenter - courtLength/2 ) * net.transform.forward, Space.World);
        }
        GameObject.Find("HostRacket").GetComponent<TennisRacket>().Player = players[0];
        Server = players[0];
        ReturnBallToServer();
    }

    public void ScorePoint(TennisPlayer player, TennisPlayer scoredOn)
    {
        Debug.Log($"Player scored : {(player == null ? "null" : player.Id)} | {scoredOn.Id}");

        if (player == null)
        {
            ReturnBallToServer();
            return;
        }

        player.Score++;
        string msg = "";
        foreach(TennisPlayer p in players)
        {
            msg += $"{p.Id} : {p.Score}, ";
        }
        Debug.Log(msg);
        if(player.Score == 4 && !players.Any(p => p.Score >= 3))
        {
            WinGame(player);

        }
        else if (player.Score >= players.Max(p => p.Score) + 2)
        {
            WinGame(player);

        }

        Server = scoredOn;
        ReturnBallToServer();
    }

    private void WinGame(TennisPlayer player)
    {
        Debug.Log($"Winner is {player.Id}");
    }

    public void ReturnBallToServer()
    {
        ballScript.hasBeenServed = false;
        ballScript.GetComponent<Rigidbody>().isKinematic = true;

        if(Server.Id == 0)
        {
            ballScript.gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        }
        else
        {

        }
    }
}