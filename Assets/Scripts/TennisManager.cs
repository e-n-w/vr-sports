using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TennisManager : MonoBehaviour
{ 
    [SerializeField] private TennisBall ballScript;
    [SerializeField] private GameObject courtPrefab;
    [SerializeField] private GameObject netPrefab;
    [SerializeField] private GameObject hostPlayerRig;
    [SerializeField] private GameObject localRacket;
    private List<TennisPlayer> players;

    [SerializeField] private float courtWidth;
    [SerializeField] private float courtLength;
    [SerializeField, Range(2, 6)] private int NumPlayers;

    public TennisPlayer Server { get; private set; }

    public void StartMatch(int numberOfPlayers)
    {
        if (numberOfPlayers <= 1) numberOfPlayers = 2;

        NumPlayers = numberOfPlayers;
        players = new List<TennisPlayer>();
        float distanceFromCenter = courtWidth / (2 * Mathf.Tan(Mathf.PI/NumPlayers)) + courtLength/2;

        players.Add(new TennisPlayer(0));
        hostPlayerRig.transform.position = CreateCourtPiece(distanceFromCenter, 0).transform.position;
        for (int i = 1; i < NumPlayers; i++)
        {
            players.Add(new TennisPlayer(i));
            CreateCourtPiece(distanceFromCenter, i);
            GameObject net = Instantiate(netPrefab);
            net.transform.eulerAngles = new Vector3(0, 90 + 360 / NumPlayers * i, 0);
            net.transform.localScale = new Vector3(courtWidth, 1, 0.25f);
            net.transform.Translate((distanceFromCenter - courtLength/2 ) * net.transform.forward, Space.World);
        }
        localRacket.GetComponent<TennisRacket>().Player = players[0];
        Server = players[0];
        ReturnBallToServer();
    }
    GameObject CreateCourtPiece(float distanceFromCenter, int index)
    {
        GameObject courtPart = Instantiate(courtPrefab);
        courtPart.transform.eulerAngles = new Vector3(90, 360 / NumPlayers * index, 0);
        courtPart.transform.localScale = new Vector3(courtLength, courtWidth, 0.25f);
        courtPart.transform.Translate(distanceFromCenter * courtPart.transform.right, Space.World);
        courtPart.GetComponent<TennisCourt>().AssociatedPlayer = players[index];
        return courtPart;
    }

    public void ScorePoint(TennisPlayer player, TennisPlayer scoredOn)
    {
        if (player == null) { ReturnBallToServer(); return; }

        player.Score++;

        if(player.Score == 4 && !players.Any(p => p.Score >= 3)) WinGame(player);
        else if (player.Score >= players.Max(p => p.Score) + 2) WinGame(player);

        Server = scoredOn;
        ReturnBallToServer();
    }

    private void WinGame(TennisPlayer player) => Debug.Log($"Winner is {player.Id}");
    public void ReturnBallToServer()
    {
        ballScript.hasBeenServed = false;
        ballScript.GetComponent<Rigidbody>().isKinematic = true;

        if(Server.Id == 0)
            ballScript.gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        else
        {

        }
    }
}