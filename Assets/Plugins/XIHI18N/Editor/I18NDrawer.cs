using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XIHLocalization
{
    public abstract class I18NDrawer<T, C> : PropertyDrawer where T : XIHLocalization.I18NBase<C> where C : Component
    {
        protected readonly static float baseHeight = GUI.skin.textField.CalcSize(new GUIContent()).y;
        protected float height;
        protected GUIStyle gs = new GUIStyle() { richText = true };
        List<XIHLanguage> lgs;
        public I18NDrawer()
        {
            lgs = new List<XIHLanguage>();
            foreach (var en in Enum.GetNames(typeof(XIHLanguage))) {
                if (Enum.TryParse<XIHLanguage>(en, out var lg)) {
                    if (lg == XIHLanguage.none) continue;
                    lgs.Add(lg);
                }
            }
            index = -1;
        }
        int index;
        string previousString;
        string[] suggests;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect propertylabelRect = new Rect(position.x, position.y, position.width, baseHeight);
            EditorGUI.PropertyField(propertylabelRect, property, GUIContent.none);
            if (!property.stringValue.Equals(previousString)) {
                suggests = I18NEditorUtil.GetRecommendKeys(property.stringValue);
                previousString = property.stringValue;
                index = -1;
            }
            height = baseHeight;
            var newIdx = EditorGUI.Popup(new Rect(position.x, position.y + height, position.width, baseHeight), index, suggests);
            if (newIdx != index) {
                property.stringValue = suggests[newIdx];
                index = newIdx;
            }
            height += baseHeight;
            foreach (var lg in lgs) {
                height+=DrawContent(position, property.stringValue,lg);
            }
            if (GUI.Button(new Rect(position.x, position.y + height, position.width, baseHeight), new GUIContent("ReFresh")))
            {
                var lo = Selection.activeGameObject.GetComponent<T>();
                lo.DoTranslate(property.stringValue);
                EditorUtility.SetDirty(Selection.activeGameObject);
                //AssetDatabase.SaveAssets();
            };
            height += baseHeight;
            if (GUI.Button(new Rect(position.x, position.y + height, position.width, baseHeight), new GUIContent("Reset")))
            {
                I18NUtil.Reset();
                I18NEditorUtil.Reset();
            };
            height += baseHeight;
            EditorGUI.EndProperty();
        }
        public abstract float DrawContent(Rect position, string key, XIHLanguage lg);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }
        const int MAX_HEIGHT = 64;
        const int MAX_WIDTH = 128;
        public Vector2 GetTextureShowRect(Texture2D texture)
        {
            if (texture.height < MAX_HEIGHT)
            {
                if (texture.width < MAX_WIDTH)
                {
                    return new Vector2(texture.width, texture.height);
                }
                else
                {
                    return new Vector2(MAX_WIDTH, texture.height * MAX_WIDTH / texture.width);
                }
            }
            else
            {
                if (texture.width < MAX_WIDTH)
                {
                    return new Vector2(texture.width * MAX_HEIGHT / texture.height, MAX_HEIGHT);
                }
                else
                {
                    float max = Mathf.Max(texture.width / MAX_WIDTH, texture.height / MAX_HEIGHT);
                    return new Vector2(texture.width / max, texture.height / max);
                }
            }
        }
    }
}
