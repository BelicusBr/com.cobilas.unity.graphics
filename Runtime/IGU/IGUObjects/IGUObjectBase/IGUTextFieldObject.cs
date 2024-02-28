using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUTextFieldObject : IGUTextObject {

        [SerializeField] protected IGUTextSettings settings;
        
        public IGUTextSettings Settings { get => settings; set => settings = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            settings = new IGUTextSettings();
        }

        protected override void IGUStart() => base.IGUStart();
        protected override void PreOnIGU() => base.PreOnIGU();
        protected override void PostOnIGU() => base.PostOnIGU();
        protected override void IGUOnEnable() => base.IGUOnEnable();
        protected override void IGUOnDisable() => base.IGUOnDisable();
        protected override void LowCallOnIGU() => base.LowCallOnIGU();
        protected override void IGUOnDestroy() => base.IGUOnDestroy();

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected virtual void SetGUISettings(GUISettings settings)
            => SetGUISettings(settings.cursorColor, settings.selectionColor, settings.cursorFlashSpeed,
                settings.doubleClickSelectsWord, settings.tripleClickSelectsLine);

        protected virtual void SetGUISettings(IGUTextSettings settings)
            => SetGUISettings(settings.CursorColor, settings.SelectionColor, settings.CursorFlashSpeed,
                settings.DoubleClickSelectsWord, settings.TripleClickSelectsLine);

        private void SetGUISettings(
            Color cursorColor, Color selectionColor, float cursorFlashSpeed,
            bool doubleClickSelectsWord, bool tripleClickSelectsLine
            ) {
            GUISettings settings1 = GUI.skin.settings;
            settings1.cursorColor = cursorColor;
            settings1.selectionColor = selectionColor;
            settings1.cursorFlashSpeed = cursorFlashSpeed;
            settings1.doubleClickSelectsWord = doubleClickSelectsWord;
            settings1.tripleClickSelectsLine = tripleClickSelectsLine;
        }
    }
}
