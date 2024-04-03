// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using Pause;
using UI;

public class CompleteWindow : Window
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private TMP_Text _victoryText;

    private const string MenuSceneName = "Main menu";

    private void Awake()
    {
        _restartButton.onClick.AddListener(OnRetryButtonClicked);
        _quitButton.onClick.AddListener(OnExitButtonClicked);
        _nextButton.onClick.AddListener(LoadNextLevel);
    }

    private void OnEnable()
    {
        CheckNextLevel();
    }

    private void CheckNextLevel()
    {
        _nextButton.gameObject.SetActive(LevelSelectManager.Instance.HasNextLevel());
        _victoryText.gameObject.SetActive(!LevelSelectManager.Instance.HasNextLevel());
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
        SceneManager.LoadScene(MenuSceneName);
    }

    private void LoadNextLevel()
    {
        WindowManager.CloseLast();
        LevelSelectManager.Instance.LoadLevel(1);
        
        OnRetryButtonClicked();
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(OnRetryButtonClicked);
        _quitButton.onClick.RemoveListener(OnExitButtonClicked);
        _nextButton.onClick.RemoveListener(LoadNextLevel);
    }
}
