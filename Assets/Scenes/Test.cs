using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XIHLocalization;
public class Test : MonoBehaviour
{
    public Text text;
    private void OnEnable()
    {
        LocalizationUtil.LanugeChanged += OnLanguageChanged;
    }
    private void OnDisable()
    {
        LocalizationUtil.LanugeChanged -= OnLanguageChanged;
    }
    void OnLanguageChanged(XIHLanguage language) {
        text.text = string.Format(LocalizationConsts.CUR_LAG,LocalizationConsts.LANGUAGE);
    }
    public void SetLanguageCN() {
        LocalizationUtil.SetLanguage(XIHLanguage.cn);
    }
    public void SetLanguageZH_TW() {
        LocalizationUtil.SetLanguage(XIHLanguage.zh_TW);
    }
    public void SetLanguageEN()
    {
        LocalizationUtil.SetLanguage(XIHLanguage.en);
    }
}
