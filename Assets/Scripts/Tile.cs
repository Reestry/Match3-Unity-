// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using DG.Tweening;
using Pool;
using Tokens;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileView _tileView;

    public Token Token { get; private set; }
    public bool IsPlayable { get; private set; }
    public int Row { get; private set; }
    public int Column { get; private set; }

    private readonly Vector3 _tokenSpawnIndent = new(0, 0.4f, 0);

    private Vector3 _initialScale;

    public void SetPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public void Unentered()
    {
        _tileView.Unpressed();
    }
    
    public void Entered()
    {
        _tileView.Pressed();
        AudioManager.Instance.PlaySoundEffect("match");
    }
    
    public void DestroyToken()
    {
        if (Token == null)
            return;

        PoolObject.Release(Token);
        Token = null;
    }

    public void SetToken(Token token)
    {
        Token = token;
        IsPlayable = token is PlayableToken;
    }

    private void Awake()
    {
        _initialScale = transform.localScale;
        transform.localScale = Vector2.zero;
    }

    private void OnEnable()
    {
        transform.DOScale(_initialScale, 1f);
    }

    public void UnsubscribeToken(Token token)
    {
        if (Token == null)
            return;

        Token = token;
    }

    public void CreateToken()
    {
        var token = PoolObject.Get<PlayableToken>();
        token.transform.position = transform.position + _tokenSpawnIndent;

        token.transform.parent = transform;
        token.SetRandom();
        token.Jump(transform.position);

        SetToken(token);
    }
    
    public void CreateBonus(BonusTokens tokenType)
    {
        var token = PoolObject.Get<BonusToken>();

        token.SetType(tokenType);
        token.transform.position = transform.position + _tokenSpawnIndent;
        AudioManager.Instance.PlaySoundEffect("newBonus");

        token.transform.parent = transform;
        token.Jump(transform.position);
        
        SetToken(token);
    }
}