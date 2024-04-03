// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using ScriptableObjects;
using UI;
using Pause;

public class DestroyColorGoal : LevelGoal
{
    private readonly TokenConfig _targetColor;
    private readonly int _targetCount;

    public DestroyColorGoal(TokenConfig targetColor, int targetCount)
    {
        _targetColor = targetColor;
        _targetCount = targetCount;
        GameUIManager.Instance.SetGoalDescription(ReturnGoalDescription());
        GameUIManager.Instance.CreateTypeGoal(this, _targetColor, _targetCount);
        LevelStatisticsManager.Instance.OnColorUpdated += UpdateScore;
    }

    private void UpdateScore(TokenConfig color, int count)
    {
        if (color != _targetColor)
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
        return $"Destroy {_targetCount} {_targetColor.TokenName}";
    }

    public override void DisplayProgress(int score)
    {
        GameUIManager.Instance.UpdateTypeGoalText(_targetCount - score);
    }

    public override bool IsGoalAchieved()
    {
        return _isVictoryLogged;
    }

    ~DestroyColorGoal()
    {
        LevelStatisticsManager.Instance.OnColorUpdated -= UpdateScore;
    }
}