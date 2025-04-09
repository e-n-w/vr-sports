using UnityEngine;

[CreateAssetMenu(fileName = "Alley Info", menuName = "Bowling/Alley")]
public class BowlingAlleyInfo : ScriptableObject
{
    public float laneDistance = 720f, laneWidthPerRow = 10.375f, extraLaneDepthPerRow = 8.5468f, gutterDepth = 1.875f;
    public float gutterWidth = 9.25f, pinDistance = 12f, orthogonalPinDistance, pinSpaceScalar = 1f;
    public int numPlayers, numPins = 10;


    //the function for multiplying by the scalar in my other script was affecting this one. this is a quick fix
    public void Start()
    {
        pinDistance = 12f;
    }
}
