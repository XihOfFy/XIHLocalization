using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace XIHLocalization
{
    [CustomPropertyDrawer(typeof(I18NTextAttribute))]
    class I18NTextDrawer : I18NDrawer<I18NText, Text>
    {
        public override float DrawContent(Rect position, string key, XIHLanguage lg)
        {
            GUI.Label(new Rect(position.x, position.y + height, position.width, baseHeight), new GUIContent(I18NUtil.TranslateText(key, lg)), gs);
            return baseHeight;
        }
    }
}