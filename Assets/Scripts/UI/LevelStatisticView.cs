// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using UnityEngine;
using TMPro;

public class LevelStatisticView : MonoBehaviour
{
   [SerializeField] private TMP_Text _scoreCount;

   public void Start()
   {
      _scoreCount.text = LevelStatisticsManager.Instance.ScoreCount().ToString();
   }
}
