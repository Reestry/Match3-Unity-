// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages
{
    public class OptionsPage : Page
    {
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _soundEffectsToggle;
        [SerializeField] private Button _resetButton;

        private void Start()
        {
            _musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            _soundEffectsToggle.isOn = PlayerPrefs.GetInt("SoundEffectsEnabled", 1) == 1;

            _musicToggle.onValueChanged.AddListener(OnMusicToggleValueChanged);
            _soundEffectsToggle.onValueChanged.AddListener(OnSoundEffectsToggleValueChanged);
            _resetButton.onClick.AddListener(OnResetButtonClick);
        }

        private void OnMusicToggleValueChanged(bool isOn)
        {
            AudioManager.Instance.SetMusicEnabled(isOn);
            PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0); 
            PlayerPrefs.Save();
        }

        private void OnSoundEffectsToggleValueChanged(bool isOn)
        {
            AudioManager.Instance.SetSoundEffectsEnabled(isOn);
            PlayerPrefs.SetInt("SoundEffectsEnabled", isOn ? 1 : 0); 
            PlayerPrefs.Save();
        }

        private void OnResetButtonClick()
        {
            AudioManager.Instance.ResetOptions();
            _musicToggle.isOn = true;
            _soundEffectsToggle.isOn = true;
        }

        private void OnDestroy()
        {
            _musicToggle.onValueChanged.RemoveListener(OnMusicToggleValueChanged);
            _soundEffectsToggle.onValueChanged.RemoveListener(OnSoundEffectsToggleValueChanged);
            _resetButton.onClick.RemoveListener(OnResetButtonClick);
        }
    }
}
