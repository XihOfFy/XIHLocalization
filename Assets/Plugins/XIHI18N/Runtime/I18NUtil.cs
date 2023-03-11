
using System;
using System.Collections.Generic;
using UnityEngine;
namespace XIHLocalization
{
    public enum XIHLanguage
    {
        none = 0,
        cn = 1,
        zh_TW = 2,
        en = 3,
    }
    public static class I18NUtil
    {
        public const string ASSET_NAME = "i18n.asset";
        public static string AssetPath = $"Assets/ABs/I18N/{ASSET_NAME}";
        public static event Action<XIHLanguage> LanugeChanged;
        public readonly static Dictionary<string, Dictionary<XIHLanguage, string>> wordsDics;
        public readonly static Dictionary<string, Sprite> spriteDics;
        public static XIHLanguage SavedLanguage { get; private set; }
        static I18NUtil()
        {
            int lag = PlayerPrefs.GetInt("LANGUAGE", 0);
            wordsDics = new Dictionary<string, Dictionary<XIHLanguage, string>>();
            spriteDics = new Dictionary<string, Sprite>();
            SavedLanguage = (XIHLanguage)lag;
            if (SavedLanguage == XIHLanguage.none)
            {
                SystemLanguage language = Application.systemLanguage;
                if (language == SystemLanguage.English)
                {
                    SavedLanguage = XIHLanguage.en;
                }
                else if (language == SystemLanguage.ChineseTraditional)
                {
                    SavedLanguage = XIHLanguage.zh_TW;
                }
                else
                {
                    SavedLanguage = XIHLanguage.cn;
                }
            }
            SetLanguage(SavedLanguage);
            Reset();
        }
        public static void Reset() {
#if UNITY_EDITOR
            I18NCfg cfg = UnityEditor.AssetDatabase.LoadAssetAtPath<I18NCfg>(AssetPath);
#else 
            I18NCfg cfg=null;
#endif
            if (cfg == null)
            {
                Debug.LogError($"File Not Exits in {AssetPath},Please change yourself logic to load I18NCfg");
                return;
            }
            wordsDics.Clear();
            foreach (var keys in cfg.keyWords)
            {
                var dic = new Dictionary<XIHLanguage, string>();
                wordsDics[keys.key] = dic;
                foreach (var key in keys.words)
                {
                    dic[key.language] = key.word;
                }
            }
        }
        public static void SetLanguage(XIHLanguage language)
        {
            if (SavedLanguage == language && wordsDics.Count > 0) return;
            PlayerPrefs.SetInt("LANGUAGE", (int)language);
            PlayerPrefs.Save();
            Debug.Log("自定义保存当前语言");
            SavedLanguage = language;
            LanugeChanged?.Invoke(SavedLanguage);
        }
        public static string TranslateText(string key)
        {
            return TranslateText(key,SavedLanguage);
        }
        public static string TranslateText(string key, XIHLanguage language)
        {
            if (!wordsDics.ContainsKey(key) || !wordsDics[key].ContainsKey(language))
            {
                Debug.LogWarning($"[{key} >> {language}]不存在，请检验翻译字典是否准确");
                return "";
            }
            return wordsDics[key][language];
        }
        public static Sprite TranslateSprite(string key)
        {
            return TranslateSprite(key,SavedLanguage);
        }
        public static Sprite TranslateSprite(string key, XIHLanguage language)
        {
            var ph = TranslateText(key, language);
            if (string.IsNullOrEmpty(ph))
            {
                return null;
            }
            if (spriteDics.ContainsKey(ph)) return spriteDics[ph];
#if UNITY_EDITOR
            Sprite sp = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(ph);
#else 
            Sprite sp = null;
#endif
            if (sp == null)
            {
                Debug.LogError($"File Not Exits in {ph},Please change yourself logic to load sprite");
                return null;
            }
            return sp;
        }
    }
}