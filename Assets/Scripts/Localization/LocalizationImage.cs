using UnityEngine;
using UnityEngine.UI;
namespace XIHLocalization
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class LocalizationImage : MonoBehaviour
    {
        [LocalizationImageRef]
        [Tooltip("Pic Should In Path At: Resources/Sprite/Localization/{relativePath}")]
        public string relativePath;
        private Image img;
        public void DoTranslate(string newRelativePath)
        {
            if (img == null) img = GetComponent<Image>();
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
    public class LocalizationImageRefAttribute : PropertyAttribute
    {
    }
    public static class LocalizationImageExt
    {
        public static void DoTranslate(this LocalizationImage self, string newRelativePath)
        {
            self.GetComponent<LocalizationImage>()?.DoTranslate(newRelativePath);
        }
    }
}