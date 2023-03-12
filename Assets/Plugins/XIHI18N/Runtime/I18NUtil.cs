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
        public static string AB_Path = "Assets/ABs/I18N";
        public static event Action<XIHLanguage> LanugeChanged;
        public readonly static Dictionary<string, Dictionary<XIHLanguage, string>> wordsDics;
        public readonly static Dictionary<string, Sprite> spriteDics;
        public static XIHLanguage SavedLanguage { get; private set; }
        static I18NUtil()
        {
            wordsDics = new Dictionary<string, Dictionary<XIHLanguage, string>>();
            spriteDics = new Dictionary<string, Sprite>();
            var lag = XIHLanguage.none;
            Debug.LogError("1/4 此处需要自己设置运行时读取本地语言");
            SavedLanguage = lag;
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
            I18NCfg cfg = UnityEditor.AssetDatabase.LoadAssetAtPath<I18NCfg>($"{AB_Path}/{ASSET_NAME}");
#else
            I18NCfg cfg = null;
#endif
            Debug.LogError("2/4 此处需要自己设置运行时读取本地化配置");
            if (cfg == null)
            {
                Debug.LogError($"File Not Exits in {AB_Path}/{ASSET_NAME},Please change yourself logic to load I18NCfg");
                return;
            }
            spriteDics.Clear();
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
            Debug.LogError("3/4 此处需要自己设置运行时保存本地语言");
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
            Sprite sp =  UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(ph);
#else
            Sprite sp = null;
#endif
            Debug.LogError("4/4(end) 最后这需要自己设置运行时读取本地化路径的图片资源");
            if (sp == null)
            {
                Debug.LogError($"File Not Exits in {ph},Please change yourself logic to load sprite");
                return null;
            }
            return sp;
        }
    }
}