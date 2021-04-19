using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace XIHLocalization
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class LocalizationText : MonoBehaviour
    {
        static readonly string[] SZ = new string[0];
        [LocalizationTextRef]
        public string key;
        [SerializeField]
        private LocalizationInsert[] inserts;//无法多层次嵌套，最大深度为2
        private Text text;
        public void DoTranslate(string k, params LocalizationInsert[] ins)
        {
            string[] insertV = SZ;
            if (ins != null)
            {
                int len = ins.Length;
                insertV = new string[len];
                for (int i = 0; i < len; ++i)
                {
                    insertV[i] = ins[i].Translate();
                }
            }
            if (text == null) text = GetComponent<Text>();
            text.text = string.Format(LocalizationUtil.TranslateText(k), insertV);
            key = k;
            inserts = ins;
        }
        public void DoTranslate(params LocalizationInsert[] ins)
        {
            DoTranslate(key, ins);
        }
        private void Awake()
        {
            LocalizationUtil.LanugeChanged += OnLanguageChanged;
            DoTranslate(key, inserts);
        }
        private void OnDestroy()
        {
            LocalizationUtil.LanugeChanged -= OnLanguageChanged;
        }
        void OnLanguageChanged(XIHLanguage language)
        {
            DoTranslate(key, inserts);
        }
        public void Refresh()
        {
            DoTranslate(key, inserts);
        }
    }
    [Serializable]
    public struct LocalizationInsert
    {
        public string insert;
        public bool needTranslate;
        public string Translate()
        {
            return needTranslate ? LocalizationUtil.TranslateText(insert) : insert;
        }
    }
    public class LocalizationTextRefAttribute : PropertyAttribute
    {
    }
    public static class LocalizationTextExt
    {
        public static void DoTranslate(this Text self, string k, params LocalizationInsert[] ins)
        {
            self.GetComponent<LocalizationText>()?.DoTranslate(k.ToLower(), ins);
        }
        public static void DoTranslate(this Text self, params LocalizationInsert[] ins)
        {
            self.GetComponent<LocalizationText>()?.DoTranslate(ins);
        }
    }
}