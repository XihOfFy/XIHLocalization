using UnityEngine;
using UnityEngine.UI;
namespace XIHLocalization
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class LocalizationSpriteRender : MonoBehaviour
    {
        [LocalizationSpriteRenderRef]
        [Tooltip("Pic Should In Path At: Resources/Sprite/Localization/{relativePath}")]
        public string relativePath;
        private SpriteRenderer img;
        public void DoTranslate(string newRelativePath)
        {
            if (img == null) img = GetComponent<SpriteRenderer>();
            relativePath = newRelativePath;
            img.sprite = LocalizationUtil.TranslateImg(relativePath);
        }
        void OnLanguageChanged(XIHLanguage language)
        {
            DoTranslate(relativePath);
        }
        private void Awake()
        {
            DoTranslate(relativePath);
            LocalizationUtil.LanugeChanged += OnLanguageChanged;
        }
        private void OnDestroy()
        {
            LocalizationUtil.LanugeChanged -= OnLanguageChanged;
        }
    }
    public class LocalizationSpriteRenderRefAttribute : PropertyAttribute
    {
    }
    public static class LocalizationSpriteRenderExt
    {
        public static void DoTranslate(this LocalizationSpriteRender self, string newRelativePath)
        {
            self.GetComponent<LocalizationSpriteRender>()?.DoTranslate(newRelativePath);
        }
    }
}