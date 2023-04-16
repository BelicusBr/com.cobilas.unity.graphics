using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUTextFieldObject : IGUTextObject {

        [SerializeField] protected IGUTextSettings settings;
        /// <summary>Configurações do cursor de inserção de texto.</summary>
        public IGUTextSettings Settings { get => settings; set => settings = value; }

        protected override void Awake() {
            base.Awake();
            settings = new IGUTextSettings();
        }

        protected override void LowCallOnIGU() => base.OnIGU();
        protected override void OnEnable() => base.OnEnable();
        protected override void OnDisable() => base.OnDisable();
        protected override void OnIGUDestroy() => base.OnIGUDestroy();

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

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
