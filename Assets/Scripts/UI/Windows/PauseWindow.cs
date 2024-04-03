// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Pause;

namespace UI.Windows
{
    public class PauseWindow : Window
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Toggle _soundEffectsToggle;
        [SerializeField] private Toggle _musicToggle;

        private const string MenuSceneName = "Main menu";

        private void Awake()
        {
            _musicToggle.onValueChanged.AddListener(OnMusicToggleValueChanged);
            _soundEffectsToggle.onValueChanged.AddListener(OnSoundEffectsToggleValueChanged);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _retryButton.onClick.AddListener(OnRetryButtonClicked);
        }

        private void Start()
        {
            _musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            _soundEffectsToggle.isOn = PlayerPrefs.GetInt("SoundEffectsEnabled", 1) == 1;
        }

        private void OnContinueButtonClicked()
        {
            WindowManager.CloseLast();
            PauseManager.Instance.SetPaused(false);
        }

        private void OnRetryButtonClicked()
        {
            DOTween.KillAll();
            WindowManager.CloseLast();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            PauseManager.Instance.SetPaused(false);
        }
        
        private void OnExitButtonClicked()
        {
            WindowManager.CloseLast();
            DOTween.KillAll();
            PauseManager.Instance.SetPaused(false);
            SceneManager.LoadScene(MenuSceneName);
        }
        
        private void OnMusicToggleValueChanged(bool isOn)
        {
            AudioManager.Instance.SetMusicEnabled(isOn);
            PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0); // Сохранение состояния в PlayerPrefs
            PlayerPrefs.Save();
        }

        private void OnSoundEffectsToggleValueChanged(bool isOn)
        {
            AudioManager.Instance.SetSoundEffectsEnabled(isOn);
            PlayerPrefs.SetInt("SoundEffectsEnabled", isOn ? 1 : 0); // Сохранение состояния в PlayerPrefs
            PlayerPrefs.Save();
        }

        private void OnDestroy()
        {
            _musicToggle.onValueChanged.RemoveListener(OnMusicToggleValueChanged);
            _soundEffectsToggle.onValueChanged.RemoveListener(OnSoundEffectsToggleValueChanged);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
            _retryButton.onClick.RemoveListener(OnRetryButtonClicked);
        }
    }
}
