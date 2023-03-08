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
        I18NUtil.LanugeChanged += OnLanguageChanged;
    }
    private void OnDisable()
    {
        I18NUtil.LanugeChanged -= OnLanguageChanged;
    }
    void OnLanguageChanged(XIHLanguage language) {
        text.text = string.Format(I18NConsts.CUR_LAG,I18NConsts.LANGUAGE);
    }
    public void SetLanguageCN() {
        I18NUtil.SetLanguage(XIHLanguage.cn);
    }
    public void SetLanguageZH_TW() {
        I18NUtil.SetLanguage(XIHLanguage.zh_TW);
    }
    public void SetLanguageEN()
    {
        I18NUtil.SetLanguage(XIHLanguage.en);
    }
}
