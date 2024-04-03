// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Tokens;

public static class BonusHandler
{
    public static void HandleBonus(Tile[,] tiles, BonusTokens bonusType, int row, int column)
    {
        switch (bonusType)
        {
            case BonusTokens.Rocket:
                ActivateRocketBonus(tiles, row, column);
                break;
            case BonusTokens.Bomb:
                ActivateBombBonus(tiles, row, column);
                break;
        }
    }

    private static void ActivateRocketBonus(Tile[,] tiles, int row, int column)
    {
        var tile = tiles[row, column];
        if (tile?.Token is not BonusToken)
            return;

        for (var i = 0; i < GameBoardController.Width; i++)
        {
            var currentTile = tiles[i, column];
            if (currentTile?.Token is PlayableToken playable && i != row)
            {
                playable.ActivateEffect();
                LevelStatisticsManager.Instance.UpdateMatchedColors(playable.CurrentConfig);
            }

            if (currentTile?.Token is Obstacle)
            {
                var obstacle = currentTile.Token as Obstacle;
                GameBoardController.Obstacles.Add(obstacle);
                HighlightTile(currentTile);
                obstacle.TakeDamage();

                DestroyAnObstacle(obstacle, currentTile);
                continue;
            }

            HighlightTile(currentTile);
            AudioManager.Instance.PlaySoundEffect("rocket");
            currentTile?.DestroyToken();
        }
    }


    private static void ActivateBombBonus(Tile[,] tiles, int row, int column)
    {
        var tile = tiles[row, column];
        if (tile.Token is not BonusToken)
            return;

        for (var i = Mathf.Max(0, row - 2); i <= Mathf.Min(GameBoardController.Width - 1, row + 2); i++)
        {
            for (var j = Mathf.Max(0, column - 2); j <= Mathf.Min(GameBoardController.Height - 1, column + 2); j++)
            {
                var currentTile = tiles[i, j];
                if (currentTile?.Token is PlayableToken playable && i != row && j != column)
                {
                    playable.ActivateEffect();
                    LevelStatisticsManager.Instance.UpdateMatchedColors(playable.CurrentConfig);
                }

                if (currentTile?.Token is Obstacle)
                {
                    var obstacle = currentTile.Token as Obstacle;
                    GameBoardController.Obstacles.Add(obstacle);
                    HighlightTile(currentTile);
                    obstacle.TakeDamage();

                    DestroyAnObstacle(obstacle, currentTile);
                    continue;
                }

                HighlightTile(currentTile);
                AudioManager.Instance.PlaySoundEffect("bomb");
                tiles[i, j]?.DestroyToken();
            }
        }
    }

    private static void HighlightTile(Tile tile)
    {
        CoroutineRunner.Instance.RunCoroutine(CoroutineRunner.Instance.HighlightTile(tile));  
    }

    private static void DestroyAnObstacle(Obstacle obstacle, Tile currentTile)
    {
        if (obstacle.Level <= 0)
        {
            if (obstacle is StoneObstacle stone)
                LevelStatisticsManager.Instance.UpdateObstacles(stone.CurrentConfig);

            if (obstacle is IceObstacle ice)
                LevelStatisticsManager.Instance.UpdateObstacles(ice.CurrentConfig);

            currentTile.DestroyToken();
        }
    }
}