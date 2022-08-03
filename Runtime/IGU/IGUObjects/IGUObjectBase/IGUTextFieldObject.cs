using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUTextFieldObject : IGUTextObject {

        [SerializeField] protected IGUTextSettings settings;
        /// <summary>Configurações do cursor de inserção de texto.</summary>
        public IGUTextSettings Settings { get => settings; set => settings = value; }

        public override void OnIGU() => base.OnIGU();
        protected override void Awake() => base.Awake();
        protected override void OnEnable() => base.OnEnable();
        protected override void OnDisable() => base.OnDisable();
        protected override void OnDestroy() => base.OnDestroy();

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

        protected void UnfocusControl(Event current) {
            if (!IGUDrawer.Drawer.GetMouseButtonDown(myConfg.MouseType)) {
                GUI.SetNextControlName("IGU_Unfocus_Control");
                GUI.Label(Rect.zero, GUIContent.none, GUIStyle.none);
                GUI.FocusControl("IGU_Unfocus_Control");
            }
        }

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

        protected new static T Internal_CreateIGUInstance<T>(string name, bool useTooltip, IGUContent content) where T : IGUTextFieldObject {
            T textObject = IGUTextObject.Internal_CreateIGUInstance<T>(name, useTooltip, content);
            textObject.settings = new IGUTextSettings();
            return textObject;
        }

        protected new static T Internal_CreateIGUInstance<T>(string name, IGUContent content) where T : IGUTextFieldObject
            => Internal_CreateIGUInstance<T>(name, false, content);
    }
}
