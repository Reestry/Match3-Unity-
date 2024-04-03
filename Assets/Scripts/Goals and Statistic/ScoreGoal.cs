// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UI;
using Pause;

public class ScoreGoal : LevelGoal
{
    private readonly int _targetScore;

    public ScoreGoal(int targetScore)
    {
        _targetScore = targetScore;
        GameUIManager.Instance.CreateScoreGoalBar();
        GameUIManager.Instance.SetGoalDescription(ReturnGoalDescription());
        LevelStatisticsManager.Instance.OnScoreUpdated += UpdateScore;
    }

    private void UpdateScore(int score)
    {
        DisplayProgress(score);

        if (_isVictoryLogged || score < _targetScore)
            return;

        WindowManager.OpenWindow<CompleteWindow>();
        PauseManager.Instance.SetPaused(true);
        _isVictoryLogged = true;
    }

    public override void DisplayProgress(int score)
    {
        GameUIManager.Instance.UpdateScoreCount(score, _targetScore);
    }

    public override bool IsGoalAchieved()
    {
        return _isVictoryLogged;
    }

    public sealed override string ReturnGoalDescription()
    {
        return $"Earn {_targetScore} points";
    }

    ~ScoreGoal()
    {
        LevelStatisticsManager.Instance.OnScoreUpdated -= UpdateScore;
    }
}