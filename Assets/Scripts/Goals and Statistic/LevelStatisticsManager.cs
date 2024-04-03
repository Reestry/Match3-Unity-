// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using ScriptableObjects;
using System.Collections.Generic;
using UI.Pages;

public class LevelStatisticsManager : MonoBehaviour
{
    public static LevelStatisticsManager Instance;
    public LevelGoal CurrentGoal { get; private set; }
    public event Action<int> OnScoreUpdated;
    public event Action<ObstacleConfig, int> OnObstaclesUpdated;
    public event Action<TokenConfig, int> OnColorUpdated;

    private Dictionary<TokenConfig, int> _matchedColors = new();
    private Dictionary<ObstacleConfig, int> _obstacleColors = new();

    private LevelLayout _currentLevel;
    private int _score;

    public void SetData(LevelLayout levelSettings)
    {
        _currentLevel = levelSettings;
    }

    public void UpdateScore(int score)
    {
        _score += score;
        OnScoreUpdated?.Invoke(_score);
        
        IsVictory();
    }

    public int ScoreCount()
    {
        return _score;
    }

    public void UpdateMatchedColors(TokenConfig color)
    {
        UpdateStatistic(_matchedColors, color, OnColorUpdated);
        IsVictory();
    }

    public void UpdateObstacles(ObstacleConfig color)
    {
        UpdateStatistic(_obstacleColors, color, OnObstaclesUpdated);
        IsVictory();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        switch (_currentLevel.LevelTask)
        {
            case LevelTasks.ScoreGoal:
                CurrentGoal = new ScoreGoal(_currentLevel.ScoreGoal);
                break;
            case LevelTasks.DestroyColor:
                CurrentGoal = new DestroyColorGoal(_currentLevel.GoalColor, _currentLevel.TokenCount);
                break;
            case LevelTasks.DestroyObstacles:
                CurrentGoal = new ObstaclesGoal(_currentLevel.ObstacleColor, _currentLevel.TokenCount);
                break;
        }
    }
    
    private void UpdateStatistic<T>(Dictionary<T, int> dictionary, T key, Action<T, int> onUpdate)
    {
        if (key == null)
            return;
        
        if (!dictionary.TryAdd(key, 1))
            dictionary[key] += 1;

        onUpdate?.Invoke(key, dictionary[key]);
    }

    private void IsVictory()
    {
        if (CurrentGoal.IsGoalAchieved())
            VictoryLevel();
    }

    private void VictoryLevel()
    {
        var levelIndex = LevelSelectManager.Instance.LevelIndex;
        ChooseLvlPage.Instance.VictoryLevel(levelIndex);
    }
}