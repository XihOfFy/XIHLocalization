using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public class Words {
        public XIHLanguage language;
        public string word;
    }
}