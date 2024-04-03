// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;

[CreateAssetMenu(fileName = "BonusConfig", menuName = "Configs/Bonus Config")]
public class BonusConfig : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    
    public Sprite Sprite => _sprite;
}
