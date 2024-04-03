// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using ScriptableObjects;
using UI;
using Pause;

public class ObstaclesGoal : LevelGoal
{
    private readonly ObstacleConfig _targetColor;
    private readonly int _targetCount;

    public ObstaclesGoal(ObstacleConfig type, int targetCount)
    {
        _targetColor = type;
        _targetCount = targetCount;
        GameUIManager.Instance.SetGoalDescription(ReturnGoalDescription());
        GameUIManager.Instance.CreateTypeGoal(this, _targetColor, _targetCount);
        LevelStatisticsManager.Instance.OnObstaclesUpdated += UpdateScore;
    }

    private void UpdateScore(ObstacleConfig type, int count)
    {
        if (type != _targetColor)
            return;

        if (count > _targetCount)
            count = _targetCount;

        DisplayProgress(count);

        if (_isVictoryLogged || count < _targetCount) 
            return;

        WindowManager.OpenWindow<CompleteWindow>();
        PauseManager.Instance.SetPaused(true);
        _isVictoryLogged = true;
    }

    public sealed override string ReturnGoalDescription()
    {
        return $"Destroy {_targetCount} {_targetColor.Name}";
    }

    public override void DisplayProgress(int score)
    {
        GameUIManager.Instance.UpdateTypeGoalText(_targetCount - score);
    }

    public override bool IsGoalAchieved()
    {
        return _isVictoryLogged;
    }

    ~ObstaclesGoal()
    {
        LevelStatisticsManager.Instance.OnObstaclesUpdated -= UpdateScore;
    }
}