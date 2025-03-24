using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class TennisManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;

    [SerializeField]
    private GameObject ball;
    private TennisBall ballScript;

    [SerializeField]
    public int Player1Score { get; private set; } = 0;
    [SerializeField]
    public int Player2Score { get; private set; } = 0;

    public int Server { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        ballScript = ball.GetComponent<TennisBall>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScorePoint(int PlayerId)
    {
        Debug.Log($"Point scored for {PlayerId}");
        if (PlayerId == 0)
        {
            Player1Score++;
        }
        else if (PlayerId == 1)
        {
            Player2Score++;
        }

        if(Player1Score == 4 && Player2Score < 3)
        {
            WinGame(0);
        }
        else if(Player1Score == Player2Score + 2 && Player2Score >= 3)
        {
            WinGame(0);
        }
        else if (Player2Score == 4 && Player1Score < 3)
        {
            WinGame(1);
        }
        else if (Player2Score == Player1Score + 2 && Player1Score >= 3)
        {
            WinGame(1);
        }

        Server = PlayerId == 0 ? 1 : 0;
        ReturnBallToServer();
    }

    private void WinGame(int PlayerId)
    {
        Debug.Log($"Winner is {PlayerId}");
    }

    public void ReturnBallToServer()
    {
        ballScript.hasBeenServed = false;

        if(Server == 0)
        {
            ball.transform.position = player1.transform.position + player1.transform.forward + player1.GetNamedChild("Camera Offset").transform.position;
        }
    }
}