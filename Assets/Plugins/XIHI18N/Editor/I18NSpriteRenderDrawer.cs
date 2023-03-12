using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XIHLocalization
{
    [CustomPropertyDrawer(typeof(I18NSpriteRenderAttribute))]
    class I18NSpriteRenderDrawer : I18NDrawer<I18NSpriteRender, SpriteRenderer>
    {
        public override float DrawContent(Rect position, string key, XIHLanguage lg)
        {
            Texture2D t2d = I18NUtil.TranslateSprite(key, lg)?.texture;
            if (t2d != null)
            {
                var rc = GetTextureShowRect(t2d);
                GUI.Label(new Rect(position.x, position.y + height, rc.x, rc.y), t2d);
                return rc.y;
            }
            else
            {
                GUI.Label(new Rect(position.x, position.y + height, position.width, baseHeight), $"{lg} Pic is Not Exist");
                return baseHeight;
            }
        }
    }

}