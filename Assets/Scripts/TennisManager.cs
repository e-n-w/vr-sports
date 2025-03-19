using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ball;

    private int player1Score = 0;
    private int player2Score = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScorePoint(int playerId)
    {
        if (playerId == 0)
        {
            player1Score++;
        }
        else if (playerId == 1)
        {
            player2Score++;
        }
    }
}