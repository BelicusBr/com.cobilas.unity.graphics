using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUSelectableText : IGUTextFieldObject {

        [SerializeField, HideInInspector] 
        protected bool isFocused;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUStyle selectableTextStyle;

        /// <summary>Indica sé o <see cref="IGUSelectableText"/> está focado.</summary>
        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
        public IGUStyle SelectableTextStyle { get => selectableTextStyle; set => selectableTextStyle = value; }

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultTextArea;
            myColor = IGUColor.DefaultBoxColor;
            selectableTextStyle = IGUSkins.GetIGUStyle("Black text field border");
        }

        protected override void LowCallOnIGU() {

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);

            Event current = Event.current;

            Text = ModifyText(GetGUIContent(string.Empty), current, GetRect(), IGUStyle.GetGUIStyleTemp(selectableTextStyle)).text;

            SetGUISettings(oldSettings);

            if (useTooltip)
                if (GetRect(true).Contains(current.mousePosition))
                    DrawTooltip();
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        protected override void SetGUISettings(GUISettings settings)
            => base.SetGUISettings(settings);

        protected override void SetGUISettings(IGUTextSettings settings)
            => base.SetGUISettings(settings);

        protected virtual GUIContent ModifyText(GUIContent content, Event current, Rect position, GUIStyle style) {
            int ID = GUIUtility.GetControlID(FocusType.Keyboard, position);
            TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), ID);
            editor.text = content.text;
            editor.SaveBackup();
            editor.position = position;
            editor.controlID = ID;
            editor.multiline = true;
            editor.isPasswordField = false;
            editor.style = style;
            editor.DetectFocusChange();
            MouseClick(ID, current, position, editor);
            CopyPaste(ID, current, editor);
            KeyPress(ID, current, editor);
            DrawText(ID, current, position, content, editor);
            editor.UpdateScrollOffsetIfNeeded(current);
            return content;
        }

        protected virtual void DrawText(int ID, Event current, Rect position, GUIContent content, TextEditor editor) {
            if (current.type == EventType.Repaint) {
                if (GUIUtility.keyboardControl != ID)
                    editor.style.Draw(position, content, ID);
                else editor.DrawCursor(content.text);
            }
        }

        protected virtual void MouseClick(int ID, Event current, Rect position, TextEditor editor) {
            bool DentroDoRect = position.Contains(current.mousePosition);
            switch (current.type) {
                case EventType.MouseUp:
                    if (!DentroDoRect) {
                        isFocused = false;
                        editor.OnLostFocus();
                    }
                    if (GUIUtility.hotControl == ID) {
                        editor.MouseDragSelectsWholeWords(false);
                        GUIUtility.hotControl = 0;
                        current.Use();
                    }
                    break;
                case EventType.MouseDown:
                    if (!DentroDoRect) break;
                    GUIUtility.hotControl =
                        GUIUtility.keyboardControl = ID;
                    if (!isFocused) {
                        isFocused = true;
                        editor.OnFocus();
                    }
                    editor.MoveCursorToPosition(current.mousePosition);
                    if (current.clickCount == 2 && GUI.skin.settings.doubleClickSelectsWord) {
                        editor.SelectCurrentWord();
                        editor.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
                        editor.MouseDragSelectsWholeWords(true);
                    }
                    if (current.clickCount == 3 && GUI.skin.settings.tripleClickSelectsLine) {
                        editor.SelectCurrentParagraph();
                        editor.MouseDragSelectsWholeWords(true);
                        editor.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
                    }
                    current.Use();
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == ID) {
                        if (current.shift) editor.MoveCursorToPosition(current.mousePosition);
                        else editor.SelectToPosition(current.mousePosition);
                        current.Use();
                    }
                    break;
            }
        }

        protected virtual void KeyPress(int ID, Event current, TextEditor editor) {
            if (GUIUtility.keyboardControl != ID) return;
            if (current.type == EventType.KeyDown) {
                switch (current.keyCode) {
                    case KeyCode.UpArrow:
                        if (current.shift) editor.SelectUp();
                        else editor.MoveUp();
                        break;
                    case KeyCode.DownArrow:
                        if (current.shift) editor.SelectDown();
                        else editor.MoveDown();
                        break;
                    case KeyCode.LeftArrow:
                        if (current.shift) editor.SelectLeft();
                        else editor.MoveLeft();
                        break;
                    case KeyCode.RightArrow:
                        if (current.shift) editor.SelectRight();
                        else editor.MoveRight();
                        break;
                    case KeyCode.Home:
                        if (current.shift) editor.SelectGraphicalLineStart();
                        else editor.MoveGraphicalLineStart();
                        break;
                    case KeyCode.End:
                        if (current.shift) editor.SelectGraphicalLineEnd();
                        else editor.MoveGraphicalLineEnd();
                        break;
                    case KeyCode.PageUp:
                        if (current.shift) editor.SelectTextStart();
                        else editor.MoveTextStart();
                        break;
                    case KeyCode.PageDown:
                        if (current.shift) editor.SelectTextEnd();
                        else editor.MoveTextEnd();
                        break;
                }
                current.Use();
            }
        }

        protected virtual void CopyPaste(int ID, Event current, TextEditor editor) {
            if (current.type == EventType.ValidateCommand ||
                current.type == EventType.ExecuteCommand) {
                if (GUIUtility.keyboardControl == ID)
                    if (current.commandName == "Copy") {
                        editor.Copy();
                        current.Use();
                    }
            }
        }
    }
}
