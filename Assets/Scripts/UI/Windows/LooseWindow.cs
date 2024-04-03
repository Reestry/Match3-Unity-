// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Pause;

namespace UI.Windows
{
    public class LooseWindow : Window
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;
        
        private const string MenuSceneName = "Main menu";
        
        private void Awake()
        {
            _playButton.onClick.AddListener(OnRetryButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnRetryButtonClicked()
        {
            WindowManager.CloseLast();
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            PauseManager.Instance.SetPaused(false);
        }

        private void OnExitButtonClicked()
        { 
           WindowManager.CloseLast();
           DOTween.KillAll();
           SceneManager.LoadScene(MenuSceneName);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnRetryButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }
    }
}
