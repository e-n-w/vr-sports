using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;


public class BowlingManager : MonoBehaviour
{
    // I have been working in inches through this entire script
    // bowling lanes are always 60ft to the first pin
    // a four row lane has a width of 41.5 inches, so I will use it to scale each additional row
    // Pin distance is scaled within the spacing method

    [SerializeField] private BowlingAlleyInfo _alleyInfo;
    [SerializeField] private List<BowlingPinBehaviour> bowlingPins;
    [SerializeField] private GameObject pinPrefab, lanePrefab, localPlayer;
    [SerializeField] private Vector3 initialLanePosition = Vector3.zero;
    [SerializeField] private float laneDistance = 10f;
    private int numPlayers = 2;
    private BowlingPlayerFrameHistory[] playerScore;
    private ProBuilderMesh gutterLeft, gutterRight, lane;
    [SerializeField] private Material gutter;


    void Start()
    {
        CreatePlayerFrameHistories();
        
        InstantiatePins();
        ScalePinDistance();
        CalculatePinDistanceOrthogonality();
        SpacePins();
        GenerateLanes();
    }

    // Remove before release!!!
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            ScalePinDistance();
            CalculatePinDistanceOrthogonality();
         
        }
    }


    void CreatePlayerFrameHistories()
    {
        playerScore = new BowlingPlayerFrameHistory[_alleyInfo.numPlayers];
        for (int i = 0; i < _alleyInfo.numPlayers; i++)
        {
            playerScore[i] = new BowlingPlayerFrameHistory();
        }
    }
 
    

    void InstantiatePins()
    {
        for (int i = 0; i < _alleyInfo.numPins; i++) 
        {
            bowlingPins.Add(Instantiate(pinPrefab).GetComponent<BowlingPinBehaviour>());
        }
        
    }

    void ScalePinDistance()
    {
        _alleyInfo.pinDistance *= _alleyInfo.pinSpaceScalar;
    }

    void CalculatePinDistanceOrthogonality()
    {
        _alleyInfo.orthogonalPinDistance = MathF.Sqrt((_alleyInfo.pinDistance * _alleyInfo.pinDistance) - (_alleyInfo.pinDistance * _alleyInfo.pinDistance / 4));
        Debug.Log(_alleyInfo.orthogonalPinDistance);
    }

    
   
    void SpacePins() 
    {
        //Pins are spaced in an equilateral triangle so this janky method should allow for spacing and pin count alterations
        //The pin asset I used is oriented sideways and I used Y instead of Z to reflect this
        // NOTE ON ABOVE: I changed the model so it should be back to Z
        float relativeX = transform.position.x, relativeZ = transform.position.z, leftBound = 0;
        int pinInRow = 1, row = 1;

        for (int i = 0; i < bowlingPins.Count; i++)
        {
            if (pinInRow > row)
            {
                pinInRow = 1;
                row++;
                leftBound -= _alleyInfo.pinDistance / 2;
                relativeX = leftBound;
                relativeZ += _alleyInfo.orthogonalPinDistance;
            }
            bowlingPins[i].transform.Translate(new Vector3(relativeX, 0, relativeZ));
            relativeX += _alleyInfo.pinDistance;
            pinInRow++;
        }

    }

    public void GenerateLanes()
    {
        //Creates a lane for each player as specified by the numPlayers variable
        Vector3 laneOffset = new Vector3(laneDistance, 0, 0);

        for (int i = 0; i < numPlayers; i++) {
            GameObject lane = Instantiate(lanePrefab);
            lane.transform.position = Vector3.zero;
            lane.transform.Translate(initialLanePosition + i * laneOffset, Space.World);
            Debug.Log(initialLanePosition + i * laneOffset);
        }

        
    }

    private float GutterXLocation(int numRows)
    {
        return (_alleyInfo.laneWidthPerRow * numRows + _alleyInfo.gutterWidth) / 2;
    }

    private void ApplyLaneMaterial()
    {
        gutterLeft.SetMaterial(gutterLeft.faces, gutter);
        gutterRight.SetMaterial(gutterRight.faces, gutter);
        lane.SetMaterial(lane.faces, gutter);
    }

    public void UpdateLane()
    {
        foreach (var pin in bowlingPins)
        {
            if (pin.isStanding)
            {

            }
        }
    }

    public int CalculateScore(GameObject player)
    {
        int newBowl = 0;
        for (int i = 0; i < bowlingPins.Count; i++)
        {
            if (bowlingPins[i].isOnLane && !bowlingPins[i].isStanding)
            {
                newBowl += 1;
            }
        }
        return newBowl;
    }


    private void UpdateScore(GameObject player)
    {
        
    }
}

