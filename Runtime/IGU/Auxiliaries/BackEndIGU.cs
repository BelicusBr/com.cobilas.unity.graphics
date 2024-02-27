using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    public static class BackEndIGU {

        public static void BeginClip(IGURect position, Vector2 scrollOffset,
             Vector2 renderOffset, bool resetOffset, Action<Vector2, Vector2> clipFunc) {
            Rect recttemp = (Rect)position;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(position.Rotation, position.ModifiedRect.Position);
            GUI.BeginClip(recttemp, scrollOffset, renderOffset, resetOffset);
            clipFunc(scrollOffset, renderOffset);
            GUI.EndClip();
            GUI.matrix = oldMatrix;
        }

        public static void BeginClip(IGURect position, Vector2 scrollOffset, Action<Vector2, Vector2> clipFunc)
            => BeginClip(position, scrollOffset, Vector2.zero, false, clipFunc);

        public static void Box(IGURect rect, IGUContent content, IGUStyle style) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            GUI.Box(recttemp, IGUTextObject.GetGUIContentTemp(content), (GUIStyle)style);
            GUI.matrix = oldMatrix;
        }

        public static string PasswordField(IGURect rect, string password, char maskChar, int maxLength, IGUStyle style) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            string res = GUI.PasswordField(recttemp, password, maskChar, maxLength, (GUIStyle)style);
            GUI.matrix = oldMatrix;
            return res;
        }

        public static string TextField(IGURect rect, string password, int maxLength, IGUStyle style) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            string res = GUI.TextField(recttemp, password, maxLength, (GUIStyle)style);
            GUI.matrix = oldMatrix;
            return res;
        }

        public static string TextArea(IGURect rect, string password, int maxLength, IGUStyle style) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            string res = GUI.TextArea(recttemp, password, maxLength, (GUIStyle)style);
            GUI.matrix = oldMatrix;
            return res;
        }
        
        public static IGURect Window(int id, IGURect clientRect, GUI.WindowFunction func, IGUContent content, IGUStyle style) {
            Rect recttemp = (Rect)clientRect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(clientRect.Rotation, clientRect.ModifiedRect.Position);
            recttemp = GUI.Window(id, recttemp, func, IGUTextObject.GetGUIContentTemp(content), (GUIStyle)style);
            GUI.matrix = oldMatrix;
            return clientRect.SetPosition(recttemp.position);
        }
        
        public static void DrawTexture(IGURect rect, Texture image, ScaleMode scaleMode, bool alphaBlend, 
            float imageAspect, Color color, Vector4 borderWidths, Vector4 borderRadiuses) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            GUI.DrawTexture(recttemp, image, scaleMode, alphaBlend, imageAspect, color, borderWidths, borderRadiuses);
            GUI.matrix = oldMatrix;
        }

        public static void Label(IGURect rect, IGUContent content, IGUStyle style) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            GUI.Label(recttemp, IGUTextObject.GetGUIContentTemp(content), (GUIStyle)style);
            GUI.matrix = oldMatrix;
        }

        public static float Slider(IGURect rect, float value, float size, MaxMinSlider length, bool horiz, IGUStyle slider, IGUStyle thumb) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            float res = GUI.Slider(recttemp, value, size, length.Min, length.Max, 
                (GUIStyle)slider, (GUIStyle)thumb, horiz, GUIUtility.GetControlID(FocusType.Passive, recttemp));
            GUI.matrix = oldMatrix;
            return res;
        }

        public static float Slider(IGURect rect, float value, MaxMinSlider length, bool horiz, IGUStyle slider, IGUStyle thumb)
            => Slider(rect, value, 0f, length, horiz, slider, thumb);
        
        public static bool Button(IGURect rect, IGUContent content, IGUStyle style) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            bool res = GUI.Button(recttemp, IGUTextObject.GetGUIContentTemp(content), (GUIStyle)style);
            GUI.matrix = oldMatrix;
            return res;
        }

        public static bool RepeatButton(IGURect rect, IGUContent content, IGUStyle style) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            bool res = GUI.RepeatButton(recttemp, IGUTextObject.GetGUIContentTemp(content), (GUIStyle)style);
            GUI.matrix = oldMatrix;
            return res;
        }

        public static bool Toggle(IGURect rect, bool value, IGUContent content, IGUStyle style) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
            bool res = GUI.Toggle(recttemp, value, IGUTextObject.GetGUIContentTemp(content), (GUIStyle)style);
            GUI.matrix = oldMatrix;
            return res;
        }

        public static void SelectableText(IGURect rect, IGUContent content, IGUStyle style, ref bool isFocused) {
            Rect recttemp = (Rect)rect;
            Matrix4x4 oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedRect.Position);
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
                        editor.style.Draw(editor.position, IGUTextObject.GetGUIContentTemp(content), ID);
                    else editor.DrawCursor(content.Text);
                    break;
            }
            editor.UpdateScrollOffsetIfNeeded(current);
            GUI.matrix = oldMatrix;
        }
    }
}