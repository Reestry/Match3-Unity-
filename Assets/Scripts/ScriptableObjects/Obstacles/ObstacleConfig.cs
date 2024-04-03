// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace ScriptableObjects
{
    public abstract class ObstacleConfig : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _lvl;
        [SerializeField] private Sprite _sprite;
        
        public string Name => _name;
        public int ObstacleLevel => _lvl;
        public Sprite ObstacleSprite => _sprite;
    }
}
