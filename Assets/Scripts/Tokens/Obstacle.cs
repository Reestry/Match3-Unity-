// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using Pool;

namespace Tokens
{
    public abstract class Obstacle : Token
    {
        [SerializeField] protected ObstacleView ObstacleView;

        [HideInInspector] public bool IsDamaged;
        
        public int Level { get; protected set; }
        
        public abstract void SetType(CellRow.CellType cellType);

        public virtual void TakeDamage()
        {
            if (IsDamaged)
                return;
        
            Level -= 1;
            ObstacleView.UpdateText(Level);
            IsDamaged = true;
        }

        public override void OnRelease()
        {
            var explosion = PoolObject.Get<ObstacleExplosion>(); 
            explosion.transform.position = transform.position; 
        }
    }
}
