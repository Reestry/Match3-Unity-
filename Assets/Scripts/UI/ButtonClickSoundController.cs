// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickSoundController : MonoBehaviour
    {
        private Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlayClickSound);
        }

        private void PlayClickSound()
        {
            AudioManager.Instance.PlaySoundEffect("button");
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PlayClickSound);
        }
    }
}
