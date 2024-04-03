// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using UI.Pages;
using UI.Windows;

namespace UI
{
    public class StartControlPanel : MonoBehaviour
    {
        [SerializeField] private Button _chooseLvlButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _quiteButton;

        private void Awake()
        {
            _quiteButton.onClick.AddListener(OnQuiteButtonClicked);
            _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            _chooseLvlButton.onClick.AddListener(OnChooseLvlButtonClicked);
        }

        private void Start()
        {
            AudioManager.Instance.PlayMusic("menu");
        }

        private void OnQuiteButtonClicked()
        {
            PageManager.ClosePage();
            WindowManager.OpenWindow<DialogWindow>();
        }

        private void OnOptionsButtonClicked()
        {
            PageManager.OpenPage<OptionsPage>();
        }

        private void OnChooseLvlButtonClicked()
        {
            PageManager.OpenPage<ChooseLvlPage>();
        }

        private void OnDestroy()
        {
            _quiteButton.onClick.RemoveListener(OnQuiteButtonClicked);
            _optionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
            _chooseLvlButton.onClick.RemoveListener(OnChooseLvlButtonClicked);
        }
    }
}
