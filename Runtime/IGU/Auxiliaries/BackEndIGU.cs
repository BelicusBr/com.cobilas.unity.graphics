using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    public static class BackEndIGU {

        public static void BeginClip(IGURect position, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset) {
            Rect rect = IGURect.rectTemp;
            rect.position = position.ModifiedPosition;
            rect.size = position.ModifiedSize;
            GUI.BeginClip(rect, scrollOffset, renderOffset, resetOffset);
        }

        public static void BeginClip(IGURect position, Vector2 scrollOffset)
            => BeginClip(position, scrollOffset, Vector2.zero, false);

        public static void EndClip() => GUI.EndClip();

        public static void Box(IGURect rect, IGUContent content, IGUStyle style) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            GUI.Box(recttemp, (GUIContent)content, (GUIStyle)style);
            IGUUtilityDistortion.End();
        }

        public static string PasswordField(IGURect rect, string password, char maskChar, int maxLength, IGUStyle style) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            string res = GUI.PasswordField(recttemp, password, maskChar, maxLength, (GUIStyle)style);
            IGUUtilityDistortion.End();
            return res;
        }

        public static string TextField(IGURect rect, string password, int maxLength, IGUStyle style) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            string res = GUI.TextField(recttemp, password, maxLength, (GUIStyle)style);
            IGUUtilityDistortion.End();
            return res;
        }

        public static string TextArea(IGURect rect, string password, int maxLength, IGUStyle style) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            string res = GUI.TextArea(recttemp, password, maxLength, (GUIStyle)style);
            IGUUtilityDistortion.End();
            return res;
        }
        
        public static IGURect Window(int id, IGURect clientRect, GUI.WindowFunction func, IGUContent content, IGUStyle style) {
            IGUUtilityDistortion.Begin(clientRect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = clientRect.ModifiedPosition;
            recttemp.size = clientRect.Size;
            recttemp = GUI.Window(id, recttemp, func, (GUIContent)content, (GUIStyle)style);
            IGUUtilityDistortion.End();
            return clientRect.SetModifiedPosition(recttemp.position);
        }
        
        public static void DrawTexture(IGURect rect, Texture image, ScaleMode scaleMode, bool alphaBlend, 
            float imageAspect, Color color, Vector4 borderWidths, Vector4 borderRadiuses) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            GUI.DrawTexture(recttemp, image, scaleMode, alphaBlend, imageAspect, color, borderWidths, borderRadiuses);
            IGUUtilityDistortion.End();
        }

        public static void Label(IGURect rect, IGUContent content, IGUStyle style) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            GUI.Label(recttemp, (GUIContent)content, (GUIStyle)style);
            IGUUtilityDistortion.End();
        }

        public static float Slider(IGURect rect, float value, float size, MaxMinSlider length, bool horiz, IGUStyle slider, IGUStyle thumb) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            float res = GUI.Slider(recttemp, value, size, length.Min, length.Max, 
                (GUIStyle)slider, (GUIStyle)thumb, horiz, GUIUtility.GetControlID(FocusType.Passive, recttemp));
            IGUUtilityDistortion.End();
            return res;
        }

        public static float Slider(IGURect rect, float value, MaxMinSlider length, bool horiz, IGUStyle slider, IGUStyle thumb)
            => Slider(rect, value, 0f, length, horiz, slider, thumb);
        
        public static bool Button(IGURect rect, IGUContent content, IGUStyle style) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            bool res = GUI.Button(recttemp, (GUIContent)content, (GUIStyle)style);
            IGUUtilityDistortion.End();
            return res;
        }

        public static bool RepeatButton(IGURect rect, IGUContent content, IGUStyle style) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            bool res = GUI.RepeatButton(recttemp, (GUIContent)content, (GUIStyle)style);
            IGUUtilityDistortion.End();
            return res;
        }

        public static bool Toggle(IGURect rect, bool value, IGUContent content, IGUStyle style) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            bool res = GUI.Toggle(recttemp, value, (GUIContent)content, (GUIStyle)style);
            IGUUtilityDistortion.End();
            return res;
        }

        public static void SelectableText(IGURect rect, IGUContent content, IGUStyle style, ref bool isFocused) {
            IGUUtilityDistortion.Begin(rect);
            Rect recttemp = IGURect.rectTemp;
            recttemp.position = rect.ModifiedPosition;
            recttemp.size = rect.Size;
            Event current = Event.current;
            int ID = GUIUtility.GetControlID(FocusType.Keyboard, recttemp);
            TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), ID);
            editor.text = content.Text;
            editor.SaveBackup();
            editor.position = recttemp;
            editor.controlID = ID;
            editor.multiline = true;
            editor.isPasswordField = false;
            editor.style = (GUIStyle)style;
            editor.DetectFocusChange();

            bool DentroDoRect = editor.position.Contains(current.mousePosition);

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
                case EventType.KeyDown:
                    if (GUIUtility.keyboardControl != ID) break;
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
                    break;
                case EventType.ValidateCommand:
                case EventType.ExecuteCommand:
                    if (GUIUtility.keyboardControl != ID) break;
                    if (current.commandName == "Copy") {
                        editor.Copy();
                        current.Use();
                    }
                    break;
                case EventType.Repaint:
                    if (GUIUtility.keyboardControl != ID)
                        editor.style.Draw(editor.position, (GUIContent)content, ID);
                    else editor.DrawCursor(content.Text);
                    break;
            }
            editor.UpdateScrollOffsetIfNeeded(current);
            IGUUtilityDistortion.End();
        }
    }
}