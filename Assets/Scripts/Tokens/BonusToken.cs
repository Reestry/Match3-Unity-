// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Tokens;

public class BonusToken : PlayableToken
{
    [SerializeField] protected List<BonusConfig> BonusConfig;

    public BonusTokens TokenType { get; private set; }

    public void SetType(BonusTokens tokenType)
    {
        TokenType = tokenType;

        switch (tokenType)
        {
            case BonusTokens.Bomb:
                SpriteRenderer.sprite = BonusConfig[0].Sprite;
                break;
            case BonusTokens.Rocket:
                SpriteRenderer.sprite = BonusConfig[1].Sprite;
                break;
        }
    }
}