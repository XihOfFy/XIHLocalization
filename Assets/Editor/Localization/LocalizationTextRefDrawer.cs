using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace XIHLocalization
{
    [CustomPropertyDrawer(typeof(LocalizationTextRefAttribute))]
    class LocalizationTextRefDrawer : PropertyDrawer
    {
        readonly float baseHeight = GUI.skin.textField.CalcSize(new GUIContent()).y;
        static LocalizationTextRefDrawer()
        {
            Init();

        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect propertylabelRect = new Rect(position.x, position.y, position.width, baseHeight);
            EditorGUI.PropertyField(propertylabelRect, property, GUIContent.none);
            GUIStyle gs = new GUIStyle() { richText = true };
            GUI.Label(new Rect(position.x, position.y + baseHeight, position.width, baseHeight), new GUIContent(TranslateTextSpecific(property.stringValue, XIHLanguage.cn)), gs);
            GUI.Label(new Rect(position.x, position.y + baseHeight * 2, position.width, baseHeight), new GUIContent(TranslateTextSpecific(property.stringValue, XIHLanguage.zh_TW)), gs);
            GUI.Label(new Rect(position.x, position.y + baseHeight * 3, position.width, baseHeight), new GUIContent(TranslateTextSpecific(property.stringValue, XIHLanguage.en)), gs);
            if (GUI.Button(new Rect(position.x, position.y + baseHeight * 4, position.width, baseHeight), new GUIContent("ReFresh")))
            {
                Init();
                LocalizationText lo = Selection.activeGameObject.GetComponent<LocalizationText>();
                lo.Refresh();
                EditorUtility.SetDirty(Selection.activeGameObject);
                //AssetDatabase.SaveAssets();
            };
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return baseHeight * 5;
        }

        private readonly static Dictionary<string, Dictionary<XIHLanguage, string>> wordsAllDics = new Dictionary<string, Dictionary<XIHLanguage, string>>();
        private static string TranslateTextSpecific(string key, XIHLanguage language)
        {
            if (!wordsAllDics.ContainsKey(key))
            {
                return $"<color=red>key[{key}] is Not Exist</color>";
            }
            if (wordsAllDics[key].ContainsKey(language))
                return wordsAllDics[key][language];
            return "";
        }
        private static void Init()
        {
            wordsAllDics.Clear();
            foreach (var val in Enum.GetNames(typeof(XIHLanguage)))
            {
                Enum.TryParse(val, out XIHLanguage lge);
                if (lge == XIHLanguage.none) continue;
                LanguageCfg cfg = ResUtil.LoadScriptableObject<LanguageCfg>($"Config/Localization/{val}");
                if (cfg == null)
                {
                    Debug.LogError($"File Not Exits in Config/Localization/{val}");
                    continue;
                }
                if (cfg.keyWords == null) continue;
                foreach (var kw in cfg.keyWords)
                {
                    if (!wordsAllDics.ContainsKey(kw.key))
                        wordsAllDics[kw.key] = new Dictionary<XIHLanguage, string>();
                    wordsAllDics[kw.key][lge] = kw.word;
                }
            }
        }
    }
}