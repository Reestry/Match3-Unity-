public abstract class LevelGoal
{
    protected bool _isVictoryLogged = false;
    public abstract string ReturnGoalDescription();
    public abstract void DisplayProgress(int score);

    public abstract bool IsGoalAchieved();
}
