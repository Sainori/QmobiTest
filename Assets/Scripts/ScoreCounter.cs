public class ScoreCounter
{
    private uint _score;

    public ScoreCounter()
    {
        _score = 0;
    }

    public uint GetScore()
    {
        return _score;
    }
    
    public void AddScore(uint score)
    {
        _score += score;
    }

    public void ResetScore()
    {
        _score = 0;
    }
}