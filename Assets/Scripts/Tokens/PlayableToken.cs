// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;

namespace Tokens
{
    public class PlayableToken : Token
    {
        [SerializeField] public List<TokenConfig> _tokenConfigs;

        public TokenConfig CurrentConfig { get; private set; }

        public void SetRandom()
        {
            Index = Random.Range(0, _tokenConfigs.Count);
            CurrentConfig = _tokenConfigs[Index];
        
            SpriteRenderer.sprite = CurrentConfig.TokenSprite;
            Name = CurrentConfig.TokenName;
            Score = CurrentConfig.TokenScore;
        }
        
        public override void ActivateEffect()
        {
            LevelStatisticsManager.Instance.UpdateScore(Score);
        }
    }
}
