using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace XIHLocalization
{
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class I18NPropertyAttribute : PropertyAttribute
    {
    }
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
    public static class I18NEditorUtil
    {
        public static string[] GetRecommendKeys(string prefix)
        {
            var dmy = root;
            var cs = prefix.ToCharArray();
            int idx = -1;
            int len = cs.Length;
            while (++idx < len)
            {
                var c = cs[idx];
                if (dmy.links.ContainsKey(c))
                {
                    dmy = dmy.links[c];
                    continue;
                }
                return empty;
            }
            StringBuilder sb = new StringBuilder(prefix);
            --sb.Length;
            var ans = new List<string>();
            DFSSearch(dmy, sb, ans);
            return ans.ToArray();
        }
        private static void DFSSearch(KeyLink key, StringBuilder sb, List<string> ans)
        {
            sb.Append(key.val);
            if (key.isEnd)
            {
                ans.Add(sb.ToString());
            }
            foreach (var cd in key.links)
            {
                DFSSearch(cd.Value, sb, ans);
            }
            if(sb.Length>0) --sb.Length;
        }
        private static  KeyLink root;
        private static readonly string[] empty = new string[0];
        private class KeyLink
        {
            public char val;
            public bool isEnd;
            public Dictionary<char, KeyLink> links;
            public KeyLink(char val, bool isEnd, Dictionary<char, KeyLink> links)
            {
                this.val = val;
                this.isEnd = isEnd;
                this.links = links;
            }
        }
        static I18NEditorUtil()
        {
            root = new KeyLink(' ', false, new Dictionary<char, KeyLink>());
            Reset();
        }
        public static void Reset() {
            var wordsAllDics = I18NUtil.wordsDics;
            root.links.Clear();
            foreach (var k in wordsAllDics)
            {
                var keys = k.Key.ToCharArray();
                DFSFill(0, keys, keys.Length - 1, root.links);
            }
        }
        private static void DFSFill(int idx, char[] keys, int maxIdx, Dictionary<char, KeyLink> parLinks)
        {
            var key = keys[idx];
            if (maxIdx == idx)
            {
                if (parLinks.ContainsKey(key)) parLinks[key].isEnd = true;
                else parLinks[key] = new KeyLink(key, true, new Dictionary<char, KeyLink>());
                return;
            }
            Dictionary<char, KeyLink> curLinks = null;
            if (parLinks.ContainsKey(key)) curLinks = parLinks[key].links;
            else
            {
                curLinks = new Dictionary<char, KeyLink>();
                parLinks[key] = new KeyLink(key, false, curLinks);
            }
            DFSFill(++idx, keys, maxIdx, curLinks);
        }
    }
#endif
}