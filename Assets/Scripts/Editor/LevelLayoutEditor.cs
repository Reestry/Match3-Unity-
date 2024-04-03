// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEditor;
using ScriptableObjects;

namespace Editor
{
    [CustomEditor(typeof(LevelLayout))]
    public class LevelLayoutEditor : UnityEditor.Editor
    {
        private SerializedProperty _soundConfigProperty;

        private void OnEnable()
        {
            _soundConfigProperty = serializedObject.FindProperty("_soundConfig");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_soundConfigProperty);

            var levelLayout = (LevelLayout) target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_levelTask"), new GUIContent("Level Task"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_moveCount"), new GUIContent("Move Count"));

            switch (levelLayout.LevelTask)
            {
            case LevelTasks.ScoreGoal:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_scoreGoal"),
                    new GUIContent("Score Goal"));
                break;
            case LevelTasks.DestroyColor:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_goalColor"),
                    new GUIContent("Goal color"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_tokenCount"),
                    new GUIContent("Token Count"));
                break;
            case LevelTasks.DestroyObstacles:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_obstacleColor"),
                    new GUIContent("Goal color"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_tokenCount"),
                    new GUIContent("Token Count"));
                break;
            }
            
            foreach (var row in levelLayout.Layout)
            {
                EditorGUILayout.BeginHorizontal();
                for (var i = 0; i < row.Cells.Count; i++)
                {
                    var defaultColor = GUI.color;
                    switch (row.Cells[i])
                    {
                    case CellRow.CellType.Empty:
                        GUI.color = Color.gray;
                        break;
                    case CellRow.CellType.SimpleIce:
                    case CellRow.CellType.StrongIce:
                        GUI.color = Color.cyan;
                        break;
                    case CellRow.CellType.SimpleStone:
                    case CellRow.CellType.StrongStone:
                        GUI.color = Color.yellow;
                        break;
                    case CellRow.CellType.Normal:
                        GUI.color = Color.green;
                        break;
                    default:
                        GUI.color = defaultColor;
                        break;
                    }

                    row.Cells[i] = (CellRow.CellType) EditorGUILayout.EnumPopup(row.Cells[i]);
                }

                EditorGUILayout.EndHorizontal();
            }
            
            if (_soundConfigProperty.objectReferenceValue != null)
            {
                var soundConfig = (SoundConfig) _soundConfigProperty.objectReferenceValue;
                if (soundConfig.SoundDictionary != null && soundConfig.SoundDictionary.Keys.Count > 0)
                {
                    var keys = soundConfig.SoundDictionary.Keys.ToArray();
                    var selectedIndex = Mathf.Max(0, System.Array.IndexOf(keys, levelLayout.SelectedMusicKey));
                    var newIndex = EditorGUILayout.Popup("Level Music", selectedIndex, keys);
                    if (newIndex != selectedIndex)
                        levelLayout.SelectedMusicKey = keys[newIndex];
                }
                else
                    EditorGUILayout.HelpBox("No audio clips found in SoundConfig", MessageType.Warning);
            }
            else
                EditorGUILayout.HelpBox("Please assign a SoundConfig", MessageType.Warning);

            if (GUI.changed)
                EditorUtility.SetDirty(levelLayout);

            serializedObject.ApplyModifiedProperties();
        }
    }
}