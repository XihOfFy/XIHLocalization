using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace XIHLocalization
{
    [RequireComponent(typeof(Text))]
    public class I18NText : I18NBase<Text>
    {
        static readonly string[] SZ = new string[0];
        [I18NText]
        public string key;
        [SerializeField]
        public I18NInsert[] inserts;//无法多层次嵌套，最大深度为2
        public override string Key => key;

        public override void DoTranslate(string newKey)
        {
            base.DoTranslate(newKey);
            key = newKey;
            string[] insertV = SZ;
            if (inserts != null)
            {
                int len = inserts.Length;
                insertV = new string[len];
                for (int i = 0; i < len; ++i)
                {
                    insertV[i] = inserts[i].Translate();
                }
            }
            trsCom.text = string.Format(I18NUtil.TranslateText(key), insertV);
        }
    }
    [Serializable]
    public struct I18NInsert
    {
        public string insert;
        public bool needTranslate;
        //public LocalizationInsert[] children;
        public string Translate()
        {
            return needTranslate ? I18NUtil.TranslateText(insert) : insert;
        }
    }
    public class I18NTextAttribute : I18NPropertyAttribute
    {
    }
    public static class I18NTextExt
    {
        public static void DoTranslate(this Text self, string k, params I18NInsert[] ins)
        {
            var trs = self.GetComponent<I18NText>();
            if (trs != null) {
                trs.inserts = ins;
                trs.DoTranslate(k);
            }
        }
        public static void DoTranslate(this Text self, params I18NInsert[] ins)
        {
            var trs = self.GetComponent<I18NText>();
            if (trs != null)
            {
                trs.inserts = ins;
                trs.Refresh();
            }
        }
    }
}