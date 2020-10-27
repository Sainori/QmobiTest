using System;

public class ScoreCounter
{
    private uint _currentScore;
    public Action<uint> OnScoreChange;

    public ScoreCounter()
    {
        _currentScore = 0;
    }

    public uint GetScore()
    {
        return _currentScore;
    }

    public void AddScore(uint score)
    {
        _currentScore += score;
        OnScoreChange(_currentScore);
    }

    public void ResetScore()
    {
        _currentScore = 0;
        OnScoreChange(_currentScore);
    }
}