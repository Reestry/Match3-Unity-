// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjects;
using UnityEngine.SceneManagement;

namespace UI.Pages
{
    public class ChooseLvlPage : Page
    {
        [SerializeField] private GameObject _levelButtonPrefab;
        [SerializeField] private Button _nextLvlPageButton;
        [SerializeField] private Button _backLvlPageButton;
        [SerializeField] private Transform _buttonParent; // Панель для кнопок

        private List<LevelLayout> _levelConfigs;
        public static ChooseLvlPage Instance;
        private const int InitialLevelButtonsCount = 4;
        private List<Button> _levelButtons = new();
        private int _currentPage;

        private List<int> _completedLevels = new();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            _nextLvlPageButton.onClick.AddListener(NextPage);
            _backLvlPageButton.onClick.AddListener(PreviousPage);
        }

        private void Start()
        {
            if (!PlayerPrefs.HasKey("Level0Completed"))
            {
                PlayerPrefs.SetInt("Level0Completed", 1);
                PlayerPrefs.Save();
            }

            var completedLevelsString = PlayerPrefs.GetString("CompletedLevels", "");
            var levelIndexes = completedLevelsString.Split(',');
            foreach (var index in levelIndexes)
            {
                if (int.TryParse(index, out var levelIndex))
                    _completedLevels.Add(levelIndex);
            }

            _levelConfigs = LevelSelectManager.Instance.GetLevels();
            var buttonCounter = _levelConfigs.Count;

            if (buttonCounter > InitialLevelButtonsCount)
                buttonCounter = InitialLevelButtonsCount;

            for (var i = 0; i < buttonCounter; i++)
            {
                var button = Instantiate(_levelButtonPrefab, _buttonParent).GetComponent<Button>();
                _levelButtons.Add(button);
            }

            UpdateMenu();
        }

        private void UpdateMenu()
        {
            _nextLvlPageButton.interactable = (_currentPage + 1) * InitialLevelButtonsCount < _levelConfigs.Count;
            _backLvlPageButton.interactable = _currentPage > 0;

            for (var i = 0; i < _levelButtons.Count; i++)
            {
                var levelIndex = _currentPage * InitialLevelButtonsCount + i;
                _levelButtons[i].gameObject.SetActive(levelIndex < _levelConfigs.Count);

                if (levelIndex < _levelConfigs.Count)
                {
                    _levelButtons[i].onClick.RemoveAllListeners();
                    _levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));

                    // Обновление текста кнопки
                    var text = _levelButtons[i].GetComponentInChildren<TMP_Text>();
                    if (text != null)
                        text.text = "Уровень " + (levelIndex + 1);

                    // Проверка, является ли уровень пройденным
                    if (_completedLevels.Contains(levelIndex))
                        _levelButtons[i].image.color = Color.yellow;
                    else
                        _levelButtons[i].image.color = Color.white;

                    // Доступен только следующий уровень от непройденного
                    var nextButtonIndex = i + 1;
                    if (nextButtonIndex < _levelButtons.Count)
                    {
                        var nextLevelIndex = levelIndex;
                        if (!_completedLevels.Contains(nextLevelIndex))
                            _levelButtons[nextButtonIndex].interactable = false;
                        else
                            _levelButtons[nextButtonIndex].interactable = true;
                    }
                }
            }
        }

        private void NextPage()
        {
            _currentPage++;
            UpdateMenu();
        }

        private void PreviousPage()
        {
            _currentPage--;
            UpdateMenu();
        }

        private void LoadLevel(int levelIndex)
        {
            // Проверка, что выбранный уровень пройден
            if (!_completedLevels.Contains(levelIndex))
                Debug.LogWarning("Level " + levelIndex + " is not completed!");

            // Загрузка выбранного уровня
            LevelSelectManager.Instance.SetLevelIndex(levelIndex);
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            _nextLvlPageButton.onClick.RemoveListener(NextPage);
            _backLvlPageButton.onClick.RemoveListener(PreviousPage);
        }

        public void VictoryLevel(int levelIndex)
        {
            if (_completedLevels.Contains(levelIndex))
                return;

            _completedLevels.Add(levelIndex);
            SaveCompletedLevels();
            UpdateMenu();
        }

        private void SaveCompletedLevels()
        {
            var completedLevelsString = string.Join(",", _completedLevels);
            PlayerPrefs.SetString("CompletedLevels", completedLevelsString);
            PlayerPrefs.Save();
        }
    }
}