using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
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
        float height;
        bool explore=false;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = baseHeight;
            float ori_height = position.y;
            EditorGUI.BeginProperty(position, label, property);
            var pro = property.FindPropertyRelative(nameof(KeyWords.key));
            int offset = 64;
            EditorGUI.LabelField(position, pro.stringValue);
            position.x += offset;

            explore = EditorGUI.Toggle(position, explore);
            position.x -= offset;
            position.y += baseHeight;

            if (explore) {
                pro = property.FindPropertyRelative(nameof(KeyWords.words));
                var size = pro.arraySize;
                for (int i = 0; i < size; ++i)
                {
                    var word = pro.GetArrayElementAtIndex(i);
                    var lag = word.FindPropertyRelative(nameof(Words.language));
                    EditorGUI.LabelField(position, lag.enumDisplayNames[lag.enumValueIndex]);
                    position.x += offset;
                    EditorGUI.TextField(position, word.FindPropertyRelative(nameof(Words.word)).stringValue);
                    position.x -= offset;
                    position.y += baseHeight;
                }
            }

            //EditorGUI.PropertyField(position, pro);
            height = position.y-ori_height;
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }
    }
#endif
}