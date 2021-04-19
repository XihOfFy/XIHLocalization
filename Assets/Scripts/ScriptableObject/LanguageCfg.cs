using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XIHLocalization
{
    [CreateAssetMenu(fileName = "LanguageCfg_tmp", menuName = "XIHScriptableObject/Language ScriptableObject", order = 1)]
    public class LanguageCfg : ScriptableObject
    {
        [SerializeField]
        public List<KeyWord> keyWords;
    }
    [Serializable]
    public struct KeyWord
    {
        public string key;
        public string word;
    }

}