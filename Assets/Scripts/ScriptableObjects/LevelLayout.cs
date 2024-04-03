// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelLayout", menuName = "Configs/Level Layout")]
    public class LevelLayout : ScriptableObject
    {
        [SerializeField] private List<CellRow> _levelLayout;
        [SerializeField] private LevelTasks _levelTask;
        [SerializeField] private int _moveCount;
        [SerializeField] private int _scoreGoal;
        [SerializeField] private int _tokenCount;
        [SerializeField] private TokenConfig _goalColor;
        [SerializeField] private ObstacleConfig _obstacleColor;
        [SerializeField] private SoundConfig _soundConfig;
        [SerializeField] private string _selectedMusicKey;
        
        public TokenConfig GoalColor => _goalColor;
        public ObstacleConfig ObstacleColor => _obstacleColor;
        
        public List<CellRow> Layout => _levelLayout;
        public LevelTasks LevelTask => _levelTask;

        public int MoveCount => _moveCount;
        public int ScoreGoal => _scoreGoal;
        
        public int TokenCount => _tokenCount;

        private bool _initialized = false;

        public string SelectedMusicKey
        {
            get => _selectedMusicKey;
            set => _selectedMusicKey = value;
        }

        public LevelLayout()
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            if (_initialized) 
                return; 
            
            _levelLayout = new List<CellRow>();
            for (var i = 0; i < 11; i++)
                _levelLayout.Add(new CellRow(7));
                
            _initialized = true; 
        }
        
        public string GetSelectedMusic()
        {
            if (_soundConfig != null && _soundConfig.SoundDictionary.ContainsKey(_selectedMusicKey))
                return _selectedMusicKey;
            
            return null;
        }
    }
}

