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
    [SerializeField] private List<GameObject> bowlingPins;
    [SerializeField] private GameObject pinPrefab, localPlayer;
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
            bowlingPins.Add(Instantiate(pinPrefab));
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

        //Note: this will also size the bowling lane
        //Pins are spaced in an equilateral triangle so this janky method should allow for spacing and pin count alterations
        //The pin asset I used is oriented sideways and I used Y instead of Z to reflect this
        // NOTE ON ABOVE: I changed the model so it should be back to Z
    {
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
        GenerateLane(row);
    }

    public void GenerateLane(int numRows)
    {
        //creates GameObject and sizes it to the gutter information
        gutterLeft = ShapeGenerator.GenerateArch(PivotLocation.Center, 180f, -_alleyInfo.gutterDepth , .1f, (_alleyInfo.laneDistance + _alleyInfo.extraLaneDepthPerRow), 20, false, true, false, false, true);
        gutterRight = ShapeGenerator.GenerateArch(PivotLocation.Center, 180f, -_alleyInfo.gutterDepth, .1f, (_alleyInfo.laneDistance + _alleyInfo.extraLaneDepthPerRow), 20, false, true, false, false, true);
        gutterLeft.transform.position = new Vector3(-GutterXLocation(numRows), -_alleyInfo.gutterDepth/2, 0);
        gutterRight.transform.position = new Vector3(GutterXLocation(numRows), -_alleyInfo.gutterDepth/2, 0);
        lane = ShapeGenerator.GeneratePlane(PivotLocation.Center, _alleyInfo.laneDistance, _alleyInfo.laneWidthPerRow * numRows, 60, 2, Axis.Up);
        lane.transform.position = new Vector3(0, 0, 0);
        
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

    }

    public int CalculateScore(GameObject player)
    {
        int newBowl = 0;
        for (int i = 0; i < bowlingPins.Count; i++)
        {
            if (bowlingPins[i].GetComponent<BowlingPinBehaviour>().isOnLane && !bowlingPins[i].GetComponent<BowlingPinBehaviour>().isStanding)
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

