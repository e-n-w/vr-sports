using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class BowlingPlayerFrameHistory : MonoBehaviour
{
    [SerializeField] private BowlingAlleyInfo _alleyInfo;
    private int lastBowl, secondToLastBowl;
    private List<List<int>> frames;
    public int currentFrame { get; private set; }
    public int currentBowlInFrame { get; private set; }

    public enum ScoreType { Strike, Spare, Open };

    

    public BowlingPlayerFrameHistory() 
    {
        lastBowl = 0;
        secondToLastBowl = 0;
    }

    public int GetLastBowl() => lastBowl;     
    public int GetSecondToLastBowl() => secondToLastBowl;

    public void SetLastBowl(int score)
    {
        secondToLastBowl = lastBowl;
        lastBowl = score;
    }

    private void AddBowlToScore()
    {
        
    }

    private List<int> GetCurrentFrame()
    {
        return frames[currentFrame];
    }

    private ScoreType CheckForStrikeOrSpare()
    {
        if (currentBowlInFrame == _alleyInfo.numFrames)
        {
            if (currentBowlInFrame == 1 && frames[currentFrame][0] == _alleyInfo.numPins) return ScoreType.Strike;
            if (currentBowlInFrame == 0 && frames[currentFrame][0] == _alleyInfo.numPins) return ScoreType.Strike;
        }
        if (currentBowlInFrame == 0 && frames[currentFrame][0] == _alleyInfo.numPins) return ScoreType.Strike;
        if (currentBowlInFrame == 1 && frames[currentFrame][1] + frames[currentFrame][0] == 10) return ScoreType.Spare;
        return ScoreType.Open;
    }

    private ScoreType CheckForStrikeOrSpare(List<int> inputFrame)
    {
        if (currentBowlInFrame == 0 && inputFrame[0] == _alleyInfo.numPins) return ScoreType.Strike;
        if (currentBowlInFrame > 0 && inputFrame[1] + inputFrame[0] == 10) return ScoreType.Spare;
        return ScoreType.Open;
    }

    //private int StrikeBonus(int rollIndex)
    //{
    //    return rolls[rollIndex + 1] + rolls[rollIndex + 2];
    //}

    public static List<int> ScoreCumulative(List<int> rolls)
    {
        List<int> cumulativeScores = new List<int>();
        int runningTotal = 0;

        //foreach (int frameScore in ScoreFrames(rolls))
        //{
        //    runningTotal += frameScore;
        //    cumulativeScores.Add(runningTotal);
        //}

        return cumulativeScores;
    }

    // Return a list of individual frame scores.
    public List<int> ScoreFrame(List<int> rolls)
    {
        List<int> frames = new List<int>();

        // Index i points to 2nd bowl of frame
        for (int i = 1; i < rolls.Count; i += 2)
        {
            if (frames.Count == _alleyInfo.numFrames) break;            

            // OPEN frame
            if (rolls[i - 1] + rolls[i] < _alleyInfo.numPins) frames.Add(rolls[i - 1] + rolls[i]);

            // Makes sure there is another bowl to count
            if (rolls.Count - i <= 1) break;              

            // STRIKE frame;
            if (rolls[i - 1] == _alleyInfo.numPins)
            {
                i--;
                frames.Add(_alleyInfo.numPins + rolls[i + 1] + rolls[i + 2]);
            }
            // SPARE frame
            else if (rolls[i - 1] + rolls[i] == 10)
            {       
                frames.Add(_alleyInfo.numPins + rolls[i + 1]);
            }
        }
        return frames;
    }
}
