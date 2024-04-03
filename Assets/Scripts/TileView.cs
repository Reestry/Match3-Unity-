// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private Color _inputColor;
    [SerializeField] private Color _baseColor;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void Pressed()
    {
        _spriteRenderer.color = _inputColor;
    }

    public void Unpressed()
    {
        _spriteRenderer.color = _baseColor;
    }
}
