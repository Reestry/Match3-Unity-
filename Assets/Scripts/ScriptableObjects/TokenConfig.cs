// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TokenConfig", menuName = "Configs/Token Config")]
    public class TokenConfig : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _score;
        [SerializeField] private Sprite _sprite;

        public string TokenName => _name;
        public int TokenScore => _score;
        public Sprite TokenSprite => _sprite;
    }
}
