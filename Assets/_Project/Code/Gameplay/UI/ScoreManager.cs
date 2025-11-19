using _Project.Code.Core.General;

public class ScoreManager : Singleton<ScoreManager>
{
    public int NumberOfHits { get; private set; }
    public int NumberOfMisses { get; private set; }

    public void AddHit() { NumberOfHits++; }
    public void AddMiss() { NumberOfMisses++; }

    private void Start()
    {
        ResetScores();
    }

    public void ResetScores()
    {
        NumberOfHits = 0;
        NumberOfMisses = 0;
    }
}
