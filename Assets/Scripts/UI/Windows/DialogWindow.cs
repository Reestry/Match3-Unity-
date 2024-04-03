// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class DialogWindow : Window
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _backButton;

        private void Awake()
        {
          _backButton.onClick.AddListener(OnBackButtonClicked);
          _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            WindowManager.CloseLast();
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }
    }
}
