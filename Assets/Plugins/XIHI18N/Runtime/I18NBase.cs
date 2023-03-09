using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XIHLocalization {
    [DisallowMultipleComponent]
    public abstract class I18NBase<C> : MonoBehaviour where C : Component
    {
        public abstract string Key { get; }
        protected C trsCom;
        protected virtual void Awake() {
            I18NUtil.LanugeChanged += OnLanguageChanged;
            DoTranslate(Key);
        }
        public virtual void DoTranslate(string newKey) {
            if (trsCom == null) trsCom = GetComponent<C>();
        }
        void OnLanguageChanged(XIHLanguage language)
        {
            DoTranslate(Key);
        }
        private void OnDestroy()
        {
            I18NUtil.LanugeChanged -= OnLanguageChanged;
        }

        public void Refresh()
        {
            DoTranslate(Key);
        }
    }
}