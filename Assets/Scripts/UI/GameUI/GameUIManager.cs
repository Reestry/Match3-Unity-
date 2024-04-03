// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    
    [SerializeField] private TMP_Text _moveCountText;
    [SerializeField] private TMP_Text _scoreCountText;
    [SerializeField] private GoalBar _goalBar;
    [SerializeField] private GoalBoard _goalBoard;

    [SerializeField] private GameObject _scoreText;
    [SerializeField] private TypeCount _typeCount;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void UpdateMoveCount(int count)
    {
        _moveCountText.text = count.ToString();
    }

    public void UpdateScoreCount(int count, int targetScore)
    {
        _scoreCountText.text = count.ToString();
        _goalBar.SetValue(count, targetScore);
    }

    public void CreateScoreGoalBar()
    {
        _goalBar.SetActive(true);
        _scoreText.SetActive(true);
    }

    public void CreateTypeGoal<T, TY>(T type, TY color, int count)
    {
        _typeCount.SetActive(true);
        _typeCount.SetTokenType(type, color, count);
    }

    public void UpdateTypeGoalText(int count)
    {
        _typeCount.UpdateCount(count);
    }

    public void SetGoalDescription(string text)
    {
        _goalBoard.SetText(text);
    }
}
