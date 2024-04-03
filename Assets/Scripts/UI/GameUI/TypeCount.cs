// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using ScriptableObjects;
using TMPro;

public class TypeCount : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _countText;

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void UpdateCount(int score)
    {
        _countText.text = score.ToString();
    }

    public void SetTokenType<T, TY>(T type, TY color, int count)
    {
        _countText.text = count.ToString();

        switch (type)
        {
            case DestroyColorGoal:
                var token = color as TokenConfig;
                _image.sprite = token.TokenSprite;
                break;
            case ObstaclesGoal:
                var obstacle = color as ObstacleConfig;
                _image.sprite = obstacle.ObstacleSprite;
                break;
        }
    }
}