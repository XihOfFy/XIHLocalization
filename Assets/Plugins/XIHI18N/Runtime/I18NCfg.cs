using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif

namespace XIHLocalization
{
    //[CreateAssetMenu(fileName = "LanguageCfg_tmp", menuName = "XIHScriptableObject/Language ScriptableObject", order = 1)]
    public class I18NCfg : ScriptableObject
    {
        public List<KeyWords> keyWords;
    }
    [Serializable]
    public class KeyWords
    {
        public string key;
        public List<Words> words;
    }
    [Serializable]
    public class Words
    {
        public XIHLanguage language;
        public string word;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(KeyWords))]
    class KeyWordsDrawer : PropertyDrawer
    {
        protected readonly static float baseHeight = GUI.skin.textField.CalcSize(new GUIContent()).y;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = baseHeight;
            EditorGUI.BeginProperty(position, label, property);
            var pro = property.FindPropertyRelative(nameof(KeyWords.key));
            EditorGUI.LabelField(position, pro.stringValue);
            position.y += baseHeight;
            pro = property.FindPropertyRelative(nameof(KeyWords.words));
            EditorGUI.PropertyField(position, pro);
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true) - baseHeight;
        }
    }

    [CustomPropertyDrawer(typeof(Words))]
    class WordsDrawer : PropertyDrawer
    {
        protected readonly static float baseHeight = GUI.skin.textField.CalcSize(new GUIContent()).y;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = baseHeight;
            EditorGUI.BeginProperty(position, label, property);
            var lag = property.FindPropertyRelative(nameof(Words.language));
            EditorGUI.LabelField(position, lag.enumDisplayNames[lag.enumValueIndex]);
            position.x += 64;
            EditorGUI.TextField(position, property.FindPropertyRelative(nameof(Words.word)).stringValue);
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return baseHeight;
        }
    }
#endif
}