using UnityEngine;
using UnityEngine.UI;
namespace XIHLocalization
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class I18NSpriteRender :  I18NBase<SpriteRenderer>
    {
        [I18NSpriteRender]
        public string key;
        public override string Key => key;
        public override void DoTranslate(string newKey)
        {
            base.DoTranslate(newKey);
            key = newKey;
            trsCom.sprite = I18NUtil.TranslateSprite(newKey);
        }
    }
    public class I18NSpriteRenderAttribute : I18NPropertyAttribute
    {
    }
    public static class I18NSpriteRenderExt
    {
        public static void DoTranslate(this I18NSpriteRender self, string newRelativePath)
        {
            self.GetComponent<I18NSpriteRender>()?.DoTranslate(newRelativePath);
        }
    }
}