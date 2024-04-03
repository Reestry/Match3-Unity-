// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using ScriptableObjects;
using Pause;
using Pool;
using Tokens;
using UI;
using UI.Pages;
using UI.Windows;

public class GameBoardController : MonoBehaviour
{
    [SerializeField] private LevelLayout _levelLayout;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private BatAnimation _batAnimation;

    public static GameBoardController Instance;

    public static int Width => LevelWidth;
    public static int Height => LevelHeight;
    public static List<Obstacle> Obstacles { get; } = new();

    private const int LevelWidth = 7;
    private const int LevelHeight = 11;
    private const float CellsIndent = 0.65f;
    private readonly Vector3 _firstCell = new(-1.95f, -4.6f, -1.2f);

    private readonly List<Tile> _matchingTiles = new();
    private bool _isTransfering;
    private Tween _tween;
    private Tile[,] _tiles;
    private Tile _lastSelectedTile;
    private Tile _secondLastSelectedTile;

    private int _moveCount;

    private void DestroyTokens()
    {
        foreach (var tile in _matchingTiles)
        {
            if (tile.Token == null)
                continue;
            
            tile.Token.StopShakeAnimation(tile.transform);

            if (tile.Token is BonusToken bonus)
                BonusHandler.HandleBonus(_tiles, bonus.TokenType, tile.Row, tile.Column);

            if (tile.Token is PlayableToken token)
            {
                token.ActivateEffect();
                LevelStatisticsManager.Instance.UpdateMatchedColors(token.CurrentConfig);
            }

            _batAnimation.StartSingleUpDownAnimation();
            DamageAnObstacle(tile.Row, tile.Column);
            tile.DestroyToken();
        }
        
        AudioManager.Instance.PlaySoundEffect("matchExplosion");
    }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.2f;

        _levelLayout = LevelSelectManager.Instance.LoadLevel(0);

        CreateGameBoard();
        
        LevelStatisticsManager.Instance.SetData(_levelLayout);
        _moveCount = _levelLayout.MoveCount;
        GameUIManager.Instance.UpdateMoveCount(_moveCount);

        var musicKey = _levelLayout.GetSelectedMusic();
        if (!string.IsNullOrEmpty(musicKey))
            AudioManager.Instance.PlayMusic(musicKey);

        SetDefault();
    }

    private void Update()
    {
        if (_tween != null && _tween.active)
            return;

        if (Input.GetMouseButton(0) && !PauseManager.IsPaused)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _layerMask);

            if (hit.collider == null)
                return;

            var tile = hit.collider.GetComponent<Tile>();

            if (!tile.IsPlayable)
                return;

            // Возможность выделить только рядом стоящие тайлы
            if (_lastSelectedTile != null)
            {
                var deltaX = Mathf.Abs(_lastSelectedTile.transform.position.x - tile.transform.position.x);
                var deltaY = Mathf.Abs(_lastSelectedTile.transform.position.y - tile.transform.position.y);

                if (Mathf.Max(deltaX, deltaY) > 1)
                    return;
            }
            
            if (tile.Token == null)
                return;

            if (tile.Token.GetType() == typeof(PlayableToken) && _matchingTiles.Any() &&
                _matchingTiles.LastOrDefault(tile => tile.Token is not BonusToken))
            {
                var token = (PlayableToken)tile.Token;
                var firstIndex = _matchingTiles.FirstOrDefault(tile => tile.Token is not BonusToken)?.Token.GetIndex();

                if (token.GetIndex() != firstIndex)
                    return;
            }

            if (!_matchingTiles.Contains(tile))
            {
                tile.Entered();
                _matchingTiles.Add(tile);
                tile.Token.ShakeAnimation();

                _secondLastSelectedTile = _lastSelectedTile;
                _lastSelectedTile = tile;
            }

            // Если выделяется предпоследний тайл, то последний тайл удаляется из списка
            if (tile == _secondLastSelectedTile)
            {
                _lastSelectedTile.Token.StopShakeAnimation(_lastSelectedTile.transform);
                _lastSelectedTile.Unentered();
                AudioManager.Instance.PlaySoundEffect("match");
                _matchingTiles.Remove(_lastSelectedTile);

                _lastSelectedTile = _secondLastSelectedTile;
                _secondLastSelectedTile = _matchingTiles.Count > 1 ? _matchingTiles[_matchingTiles.Count - 2] : null;
            }

            _lineRenderer.positionCount = _matchingTiles.Count;
            for (var i = 0; i < _matchingTiles.Count; i++)
                _lineRenderer.SetPosition(i, _matchingTiles[i].transform.position);
        }

        if (Input.GetMouseButtonUp(0) && _matchingTiles != null)
            Matching();
    }

    private void SetDefault()
    {
        foreach (var tile in _matchingTiles)
        {
            if (tile.Token != null) 
                tile.Token.StopShakeAnimation(tile.transform);
            
            tile.Unentered();
        }

        ResetObstacles();
        _matchingTiles.Clear();
        _lineRenderer.positionCount = 0;
        _lastSelectedTile = null;

        Obstacles.Clear();
    }

    private void Matching()
    {
        if (_matchingTiles.Count <= 2 && !_matchingTiles.Any(tile => tile.Token is BonusToken))
        {
            SetDefault();
            return;
        }

        var firstIndex = _matchingTiles.FirstOrDefault(tile => tile.Token is not BonusToken)?.Token.GetIndex();

        foreach (var tile in _matchingTiles)
        {
            if (tile.Token != null && (tile.Token is BonusToken || tile.Token.GetIndex() == firstIndex))
                continue;
            
            tile.Token.StopShakeAnimation(tile.transform);
            SetDefault();
            return;
        }
        
        DestroyTokens();

        var lastTile = _matchingTiles.Last();

        switch (_matchingTiles.Count)
        {
            case >= 5 and <= 7:
                lastTile.CreateBonus(BonusTokens.Rocket);
                break;
            case >= 8:
                lastTile.CreateBonus(BonusTokens.Bomb);
                break;
        }

        EndTurn();
        TransferTokens();
        SetDefault();
    }

    private void EndTurn()
    {
        _moveCount--;
        GameUIManager.Instance.UpdateMoveCount(_moveCount);

        if (_moveCount > 0 || LevelStatisticsManager.Instance.CurrentGoal.IsGoalAchieved())
            return;
        
        WindowManager.OpenWindow<LooseWindow>();
        PauseManager.Instance.SetPaused(true);
    }

    private void ResetObstacles()
    {
        foreach (var obstacle in Obstacles)
            obstacle.IsDamaged = false;
    }

    private void DamageAnObstacle(int row, int column)
    {
        int[,] offsets = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

        for (var offsetIndex = 0; offsetIndex < 4; offsetIndex++)
        {
            var adjacentRow = row + offsets[offsetIndex, 0];
            var adjacentColumn = column + offsets[offsetIndex, 1];

            if (adjacentRow < 0 || adjacentRow >= LevelWidth || adjacentColumn < 0 || adjacentColumn >= LevelHeight)
                continue;

            if (_tiles[adjacentRow, adjacentColumn] == null ||
                (_tiles[adjacentRow, adjacentColumn].Token is not Obstacle))
                continue;

            var obstacle = _tiles[adjacentRow, adjacentColumn].Token as Obstacle;
            Obstacles.Add(obstacle);

            if (obstacle.IsDamaged)
                continue;

            obstacle.TakeDamage();

            obstacle.IsDamaged = true;

            if (obstacle.Level <= 0)
            {
                if (obstacle is StoneObstacle stone)
                    LevelStatisticsManager.Instance.UpdateObstacles(stone.CurrentConfig);
                
                if (obstacle is IceObstacle ice)
                    LevelStatisticsManager.Instance.UpdateObstacles(ice.CurrentConfig);
                
                _tiles[adjacentRow, adjacentColumn].DestroyToken();
            }

            if (obstacle.Level <= 0)
                _tiles[adjacentRow, adjacentColumn].DestroyToken();
        }
    }

    private void TransferTokens()
    {
        _isTransfering = false;
        for (var i = LevelWidth - 1; i >= 0; i--)
        {
            for (var j = LevelHeight - 1; j >= 0; j--)
            {
                if (_tiles[i, j] == null)
                    continue;

                // Создание токена, если тайл самый верхний 
                if ((j == LevelHeight - 1 || _tiles[i, j + 1] == null) && _tiles[i, j].Token == null)
                    _tiles[i, j].CreateToken();

                if (_tiles[i, j]?.Token != null && _tiles[i, j]?.Token is not IceObstacle)
                    FallATokenDown(i, j);
            }
        }

        if (_isTransfering)
            TransferTokens();

        for (var j = LevelHeight - 1; j >= 0; j--)
        {
            for (var i = LevelWidth - 1; i >= 0; i--)
            {
                if (_tiles[i, j]?.Token != null && _tiles[i, j]?.Token is not IceObstacle)
                    FallATokenDiagonally(i, j);
            }
        }
    }

    private void FallATokenDown(int i, int j)
    {
        var currentJ = j;
        if (currentJ - 1 < 0 || (_tiles[i, currentJ - 1] == null || _tiles[i, currentJ - 1].Token != null))
            return;

        MoveCurrentTokenDown(i, j);
        currentJ--;
        FallATokenDown(i, currentJ);
    }

    private void MoveCurrentTokenDown(int currentI, int currentJ)
    {
        var currentToken = _tiles[currentI, currentJ].Token;

        _tiles[currentI, currentJ - 1].SetToken(currentToken);
        _tiles[currentI, currentJ].UnsubscribeToken(null);
        _tween = currentToken.transform.DOMove(_tiles[currentI, currentJ - 1].transform.position, 1f)
            .SetEase(Ease.OutBounce)
            .SetAutoKill(true);

        _isTransfering = true;
    }

    private void FallATokenDiagonally(int i, int j)
    {
        var currentI = i;
        var currentJ = j;

        // Проверка для диагонали влево и вправо
        if (currentJ - 1 < 0 ||
            (_tiles[currentI, currentJ - 1]?.Token == null && _tiles[currentI, currentJ - 1] != null))
            return;

        // Падение для диагонали влево
        if (currentI - 1 >= 0 && _tiles[currentI - 1, currentJ - 1] != null &&
            _tiles[currentI - 1, currentJ - 1].Token == null)
        {
            MoveCurrentTokenDiagonally(currentI, currentJ, currentI - 1, currentJ - 1);
            currentI--;
            currentJ--;
            FallATokenDiagonally(currentI, currentJ);
            TransferTokens();
        }

        // Падение для диагонали вправо
        if (currentI + 1 < _tiles.GetLength(0) && _tiles[currentI + 1, currentJ - 1] != null &&
            _tiles[currentI + 1, currentJ - 1].Token == null)
        {
            MoveCurrentTokenDiagonally(currentI, currentJ, currentI + 1, currentJ - 1);
            currentI++;
            currentJ--;
            FallATokenDiagonally(currentI, currentJ);
            TransferTokens();
        }
    }

    private void MoveCurrentTokenDiagonally(int fromI, int fromJ, int toI, int toJ)
    {
        var tokenDiagonal = _tiles[fromI, fromJ].Token;
        _tiles[toI, toJ].SetToken(tokenDiagonal);
        _tiles[fromI, fromJ].UnsubscribeToken(null);
        _tween = tokenDiagonal.transform.DOMove(_tiles[toI, toJ].transform.position, 1f)
            .SetEase(Ease.OutBounce)
            .SetAutoKill(true);

        _isTransfering = true;
    }

    private void CreateGameBoard()
    {
        _tiles = new Tile[LevelWidth, LevelHeight];
        for (var i = 0; i < LevelWidth; i++)
        {
            for (var j = 0; j < LevelHeight; j++)
            {
                var cellType = _levelLayout.Layout[LevelHeight - 1 - j].Cells[i];
                if (cellType != CellRow.CellType.Empty)
                {
                    var cellPosition = new Vector3(_firstCell.x + i * CellsIndent, _firstCell.y + j * CellsIndent, _firstCell.z);
                    var tile = PoolObject.Get<Tile>();

                    tile.SetPosition(i, j);
                    _tiles[i, j] = tile;

                    var transform1 = tile.transform;
                    transform1.position = cellPosition;
                    transform1.rotation = Quaternion.identity;
                    transform1.parent = transform;

                    switch (cellType)
                    {
                        case CellRow.CellType.Normal:
                            var token = PoolObject.Get<PlayableToken>();
                            token.transform.parent = transform;
                            token.SetRandom();
                            token.Jump(cellPosition);

                            tile.SetToken(token);
                            break;
                        case CellRow.CellType.SimpleIce:
                            var simpleIce = PoolObject.Get<IceObstacle>();
                            simpleIce.SetType(CellRow.CellType.SimpleIce);
                            simpleIce.Jump(cellPosition);
                            simpleIce.transform.parent = transform;

                            tile.SetToken(simpleIce);
                            break;
                        case CellRow.CellType.SimpleStone:
                            var simpleStone = PoolObject.Get<StoneObstacle>();
                            simpleStone.SetType(CellRow.CellType.SimpleStone);
                            simpleStone.Jump(cellPosition);
                            simpleStone.transform.parent = transform;

                            tile.SetToken(simpleStone);
                            break;
                        case CellRow.CellType.StrongIce:
                            var strongIce = PoolObject.Get<IceObstacle>();
                            strongIce.SetType(CellRow.CellType.StrongIce);
                            strongIce.Jump(cellPosition);
                            strongIce.transform.parent = transform;
                            
                            tile.SetToken(strongIce);
                            break;
                        case CellRow.CellType.StrongStone:
                            var strongStone = PoolObject.Get<StoneObstacle>();
                            strongStone.SetType(CellRow.CellType.StrongStone);
                            strongStone.Jump(cellPosition);
                            strongStone.transform.parent = transform;
                            
                            tile.SetToken(strongStone); 
                            break;
                    }
                }
            }
        }
    }
}
