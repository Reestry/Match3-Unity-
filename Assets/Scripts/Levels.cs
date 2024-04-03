// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "Configs/Levels")]
public class Levels : ScriptableObject
{
    [SerializeField] private List<LevelLayout> _allLevels;
    
    public List<LevelLayout> AllLevels => _allLevels;
}
