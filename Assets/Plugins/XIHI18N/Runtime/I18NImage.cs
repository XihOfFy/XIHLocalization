using UnityEngine;
using UnityEngine.UI;
namespace XIHLocalization
{
    [RequireComponent(typeof(Image))]
    public class I18NImage : I18NBase<Image>
    {
        [I18NImage]
        public string key;
        public override string Key => key;
        public override void DoTranslate(string newKey)
        {
            base.DoTranslate(newKey);
            key = newKey;
            trsCom.sprite = I18NUtil.TranslateSprite(newKey);
        }
    }
    public class I18NImageAttribute : I18NPropertyAttribute
    {
    }
    public static class I18NImageExt
    {
        public static void DoTranslate(this I18NImage self, string newRelativePath)
        {
            self.GetComponent<I18NImage>()?.DoTranslate(newRelativePath);
        }
    }
}