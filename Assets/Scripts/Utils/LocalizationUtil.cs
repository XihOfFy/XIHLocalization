
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
    public static class LocalizationUtil
    {
        public static event Action<XIHLanguage> LanugeChanged;
        private readonly static Dictionary<string, string> wordsDics = new Dictionary<string, string>();
        public static XIHLanguage SavedLanguage { get; private set; }
        static LocalizationUtil()
        {
            int lag = PlayerPrefs.GetInt("FATTY_LANGUAGE", 0);
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
        }
        public static void SetLanguage(XIHLanguage language)
        {
            if (SavedLanguage == language && wordsDics.Count > 0) return;
            PlayerPrefs.SetInt("FATTY_LANGUAGE", (int)language);
            PlayerPrefs.Save();
            SavedLanguage = language;
            LanguageCfg cfg = ResUtil.LoadScriptableObject<LanguageCfg>($"Config/Localization/{SavedLanguage}");
            if (cfg == null)
            {
                Debug.LogError($"File Not Exits in Config/Localization/{SavedLanguage}");
                return;
            }
            wordsDics.Clear();
            foreach (var kw in cfg.keyWords)
            {
                wordsDics[kw.key] = kw.word;
            }
            LanugeChanged?.Invoke(SavedLanguage);
        }
        public static string TranslateText(string key)
        {
            key = key.ToLower();
            if (!wordsDics.ContainsKey(key))
            {
                Debug.LogWarning($"发现[{key}]不存在，请检验翻译字典是否准确");
                return "";
            }
            return wordsDics[key];
        }
        public static Sprite TranslateImg(string relativeName)
        {
            Sprite sp = ResUtil.LoadSprite($"Sprite/Localization/{relativeName}_{SavedLanguage}");
            if (sp == null)
            {
                Debug.LogWarning($"Sprite/Localization/{relativeName}_{SavedLanguage}图片不存在");
            }
            return sp;
        }
    }
}