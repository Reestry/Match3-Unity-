// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
    public class SerializableDictionaryPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel++;

            var keysProperty = property.FindPropertyRelative("Keys");
            var valuesProperty = property.FindPropertyRelative("Values");

            for (var i = 0; i < keysProperty.arraySize; i++)
            {
                var keyProperty = keysProperty.GetArrayElementAtIndex(i);
                var valueProperty = valuesProperty.GetArrayElementAtIndex(i);

                var keyRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * (i + 1), 
                    position.width * 0.4f, EditorGUIUtility.singleLineHeight);
                var valueRect = new Rect(position.x + position.width * 0.45f, position.y + EditorGUIUtility.singleLineHeight 
                    * (i + 1), position.width * 0.5f, EditorGUIUtility.singleLineHeight);

                EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none);
                EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
            
            var addButtonRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * (keysProperty.arraySize + 1),
                position.width, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(addButtonRect, "Add Element"))
            {
                keysProperty.InsertArrayElementAtIndex(keysProperty.arraySize);
                valuesProperty.InsertArrayElementAtIndex(valuesProperty.arraySize);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var keysProperty = property.FindPropertyRelative("Keys");
            return EditorGUIUtility.singleLineHeight * (keysProperty.arraySize + 2);
        }
    }
}