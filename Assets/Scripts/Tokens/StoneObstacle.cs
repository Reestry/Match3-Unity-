// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using ScriptableObjects;
using Tokens;
using UnityEngine;

public class StoneObstacle : Obstacle
{
    [SerializeField] protected List<StoneObstacleConfig> _obstacleConfigs;
    public StoneObstacleConfig CurrentConfig { get; private set; }

    public override void SetType(CellRow.CellType cellType)
    {
        switch (cellType)
        {
            case CellRow.CellType.SimpleStone:
                CurrentConfig = _obstacleConfigs[0];
                SpriteRenderer.sprite = CurrentConfig.ObstacleSprite;
                Name = CurrentConfig.name;
                Level = CurrentConfig.ObstacleLevel;

            ObstacleView.UpdateText(Level);
            break;
        case CellRow.CellType.StrongStone:
            var strongStoneConfig = _obstacleConfigs[1];
            SpriteRenderer.sprite = strongStoneConfig.ObstacleSprite;
            Name = strongStoneConfig.name;
            Level = strongStoneConfig.ObstacleLevel;
                
            ObstacleView.UpdateText(Level);
            break;
        }
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        
        if (Level == 1)
            SetType(CellRow.CellType.SimpleStone);
    }
}