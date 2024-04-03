// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Tokens
{
    public class IceObstacle : Obstacle
    {
        [SerializeField] protected List<IceObstacleConfig> _obstacleConfigs;
        public IceObstacleConfig CurrentConfig { get; private set; }

        public override void SetType(CellRow.CellType cellType)
        {
            switch (cellType)
            {
            case CellRow.CellType.SimpleIce:
                CurrentConfig = _obstacleConfigs[0];
                 SpriteRenderer.sprite = CurrentConfig.ObstacleSprite;
                Name = CurrentConfig.name;
                Level = CurrentConfig.ObstacleLevel;

                ObstacleView.UpdateText(Level);
                break;
            case CellRow.CellType.StrongIce:
                var strongIceConfig = _obstacleConfigs[1];
                SpriteRenderer.sprite = strongIceConfig.ObstacleSprite;
                Name = strongIceConfig.name;
                Level = strongIceConfig.ObstacleLevel;
                
                ObstacleView.UpdateText(Level);
                break;
            }
        }
        
        public override void TakeDamage()
        {
            base.TakeDamage();
        
            if (Level == 1)
                SetType(CellRow.CellType.SimpleIce);
        }
    }
}