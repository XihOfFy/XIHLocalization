using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XIHLocalization
{
    [CustomPropertyDrawer(typeof(LocalizationImageRefAttribute))]
    class LocalizationImageRefDrawer : PropertyDrawer
    {
        readonly float baseHeight = GUI.skin.textField.CalcSize(new GUIContent()).y;
        float height = 0;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            height = baseHeight;
            EditorGUI.BeginProperty(position, label, property);
            Rect propertylabelRect = new Rect(position.x, position.y, position.width, height);
            EditorGUI.PropertyField(propertylabelRect, property, GUIContent.none);
            Texture2D t2d = TranslateImgSpecific(property.stringValue, XIHLanguage.cn);
            if (t2d != null)
            {
                GUI.Label(new Rect(position.x, position.y + height, t2d.width, t2d.height), t2d);
                height += t2d.height;
            }
            else
            {
                GUI.Label(new Rect(position.x, position.y + height, position.width, baseHeight), $"Resources/Sprite/Localization/{property.stringValue}_{XIHLanguage.cn} Pic is Not Exist");
                height += baseHeight;
            }
            t2d = TranslateImgSpecific(property.stringValue, XIHLanguage.zh_TW);
            if (t2d != null)
            {
                GUI.Label(new Rect(position.x, position.y + height, t2d.width, t2d.height), t2d);
                height += t2d.height;
            }
            else
            {
                GUI.Label(new Rect(position.x, position.y + height, position.width, baseHeight), $"Resources/Sprite/Localization/{property.stringValue}_{XIHLanguage.zh_TW} Pic is Not Exist");
                height += baseHeight;
            }
            t2d = TranslateImgSpecific(property.stringValue, XIHLanguage.en);
            if (t2d != null)
            {
                GUI.Label(new Rect(position.x, position.y + height, t2d.width, t2d.height), t2d);
                height += t2d.height;
            }
            else
            {
                GUI.Label(new Rect(position.x, position.y + height, position.width, baseHeight), $"Resources/Sprite/Localization/{property.stringValue}_{XIHLanguage.en} Pic is Not Exist");
                height += baseHeight;
            }
            if (GUI.Button(new Rect(position.x, position.y + height, position.width, baseHeight), new GUIContent("ReFresh")))
            {
                LocalizationImage lo = Selection.activeGameObject.GetComponent<LocalizationImage>();
                lo.DoTranslate(property.stringValue);
                EditorUtility.SetDirty(Selection.activeGameObject);
                //AssetDatabase.SaveAssets();
            };
            height += baseHeight;
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }
        private static Texture2D TranslateImgSpecific(string relativeName, XIHLanguage lag)
        {
            return ResUtil.LoadSprite($"Sprite/Localization/{relativeName}_{lag}")?.texture;
        }
    }

}

