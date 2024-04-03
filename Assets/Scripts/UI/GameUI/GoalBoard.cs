// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using TMPro;

public class GoalBoard : MonoBehaviour
{
    [SerializeField] private TMP_Text _goalText;

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetText(string text)
    {
        _goalText.text = text;
    }
}