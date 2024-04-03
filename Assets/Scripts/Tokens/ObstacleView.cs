// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using TMPro;

namespace Tokens
{
    public class ObstacleView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _level;

        public void UpdateText(int level)
        {
            _level.text = level.ToString();
        }
    }
}
