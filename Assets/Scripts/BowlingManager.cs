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
    private BowlingPlayerFrameHistory playerScore;
    private ProBuilderMesh gutterLeft, gutterRight, lane;
    [SerializeField] private Material gutter;

    private List<int> rolls = new List<int>();
    private int currentFrame = 0;
    private bool isFirstRollInFrame = true;
    private int score;


    void Start()
    {
        CreatePlayerFrameHistory();
        
        InstantiatePins();
        ScalePinDistance();
        CalculatePinDistanceOrthogonality();
        SpacePins();
        GenerateLanes();
        InitializeScoring();
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


    void CreatePlayerFrameHistory()
    {
        playerScore = new BowlingPlayerFrameHistory();
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


    void InitializeScoring()
    {
        rolls.Clear();
        currentFrame = 0;
        isFirstRollInFrame = true;
    }

    public void RecordBowlingRoll()
    {
        int pinsKnockedDown = CalculateKnockedDownPins();
        AddRoll(pinsKnockedDown);
        ResetPinsForNextRoll();
    }

    private int CalculateKnockedDownPins()
    {
        int count = 0;
        foreach (var pin in bowlingPins)
        {
            if (pin.isOnLane && !pin.isStanding) count++;
        }
        return count;
    }

    private void AddRoll(int pins)
    {
        rolls.Add(pins);

        // Update frame state
        if (IsStrike(currentFrame) || !isFirstRollInFrame)
        {
            currentFrame = Mathf.Min(currentFrame + 1, _alleyInfo.numFrames - 1);
            isFirstRollInFrame = true;
        }
        else
        {
            isFirstRollInFrame = false;
        }
    }

    public int GetTotalScore()
    {
        int total = 0;
        int rollIndex = 0;

        for (int frame = 0; frame < _alleyInfo.numFrames; frame++)
        {
            if (IsStrike(rollIndex))
            {
                total += _alleyInfo.numPins + StrikeBonus(rollIndex);
                rollIndex++;
            }
            else if (IsSpare(rollIndex))
            {
                total += _alleyInfo.numPins + SpareBonus(rollIndex);
                rollIndex += 2;
            }
            else
            {
                total += FrameTotal(rollIndex);
                rollIndex += 2;
            }
        }
        return total;
    }

    private bool IsStrike(int rollIndex)
    {
        return rollIndex < rolls.Count && rolls[rollIndex] == _alleyInfo.numPins;
    }

    private bool IsSpare(int rollIndex)
    {
        return rollIndex + 1 < rolls.Count &&
               rolls[rollIndex] + rolls[rollIndex + 1] == _alleyInfo.numPins;
    }

    private int StrikeBonus(int rollIndex)
    {
        return (rollIndex + 1 < rolls.Count ? rolls[rollIndex + 1] : 0) +
               (rollIndex + 2 < rolls.Count ? rolls[rollIndex + 2] : 0);
    }

    private int SpareBonus(int rollIndex)
    {
        return rollIndex + 2 < rolls.Count ? rolls[rollIndex + 2] : 0;
    }

    private int FrameTotal(int rollIndex)
    {
        int total = 0;
        int maxRolls = Mathf.Min(rollIndex + 2, rolls.Count);

        for (int i = rollIndex; i < maxRolls; i++)
        {
            total += rolls[i];
        }
        return total;
    }

    public List<int> GetRollHistory()
    {
        return new List<int>(rolls);
    }

    public int GetCurrentFrame()
    {
        return currentFrame + 1; 
    }

    public bool IsFrameComplete()
    {
        return !isFirstRollInFrame || IsStrike(currentFrame);
    }

    private void ResetPinsForNextRoll()
    {
        foreach (var pin in bowlingPins)
        {
            if (pin.isStanding)
            {
                pin.ResetPin();
            }
            else
            {
                pin.RemoveFromPlay();
            }
        }
    }
}

