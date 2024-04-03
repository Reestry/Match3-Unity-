// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private Levels _levels;

    public static LevelSelectManager Instance;
    public int LevelIndex { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    public List<LevelLayout> GetLevels()
    {
        return _levels.AllLevels;
    }

    public void SetLevelIndex(int index)
    {
        LevelIndex = index;
    }

    public LevelLayout LoadLevel(int next)
    {
        LevelIndex += next;
        
        if (_levels.AllLevels[LevelIndex] != null)
            return _levels.AllLevels[LevelIndex];
        
        return null;
    }
    
    public bool HasNextLevel()
    {
        return LevelIndex + 1 < _levels.AllLevels.Count;
    }
}
