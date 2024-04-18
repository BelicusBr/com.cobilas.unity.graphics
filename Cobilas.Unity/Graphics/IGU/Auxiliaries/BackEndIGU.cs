using System;
using UnityEngine;
using System.Text;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    public static class BackEndIGU {

        private static Rect rectTemp = Rect.zero;

        public static void Label(IGURect rect, IGUContent content, IGUStyle style, int ID)
            => ((GUIStyle)style).DrawRepaint((Rect)rect, (GUIContent)content, ID);

        public static bool Button(IGURect rect, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, bool isFocused) {
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;
            switch (@event.GetTypeForControl(ID)) {
                case EventType.MouseDown:
                    if (isHover) {
                        GUIUtility.hotControl = ID;
                        @event.Use();
                    }
                    goto default;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl != ID)
                        goto default;
                    else if (!isHover) {
                        GUIUtility.hotControl = 0;
                        goto default;
                    }
                    GUIUtility.hotControl = 0;
                    @event.Use();
                    return true;
                case EventType.Repaint:
                    ((GUIStyle)style).Draw(
                        GetRectTemp(rect), IGUTextObject.GetGUIContentTemp(content),
                        isHover, ID == GUIUtility.hotControl,
                        false, isFocused);
                    goto default;
                default:
                    return false;
            }
        }

        public static bool Button(IGURect rect, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID)
            => Button(rect, content, style, phy, ID, false);

        public static bool RepeatButton(IGURect rect, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, bool isFocused, out bool onClick) {
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;
            onClick = false;
            switch (@event.GetTypeForControl(ID)) {
                case EventType.MouseDown:
                    if (isHover) {
                        onClick = true;
                        GUIUtility.hotControl = ID;
                        @event.Use();
                    }
                    goto default;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl != ID)
                        goto default;
                    else if (!isHover) {
                        GUIUtility.hotControl = 0;
                        goto default;
                    }
                    GUIUtility.hotControl = 0;
                    @event.Use();
                    return true;
                case EventType.Repaint:
                    ((GUIStyle)style).Draw(
                        GetRectTemp(rect), IGUTextObject.GetGUIContentTemp(content),
                        isHover, ID == GUIUtility.hotControl,
                        false, isFocused);
                    return ID == GUIUtility.hotControl && isHover;
                default:
                    return false;
            }
        }

        public static bool RepeatButton(IGURect rect, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, out bool onClick)
            => RepeatButton(rect, content, style, phy, ID, false, out onClick);

        public static bool Toggle(IGURect rect, bool value, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, bool isFocused, out bool onClick) {
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;
            onClick = false;
            switch (@event.GetTypeForControl(ID)) {
                case EventType.MouseDown:
                    if (isHover) {
                        GUIUtility.hotControl = ID;
                        onClick = true;
                        @event.Use();
                    }
                    goto default;
                case EventType.MouseUp:
                    if (!isHover) {
                        GUIUtility.hotControl = 0;
                        goto default;
                    } else if (GUIUtility.hotControl != ID)
                        goto default;
                    GUIUtility.hotControl = 0;
                    value = !value;
                    @event.Use();
                    goto default;
                case EventType.Repaint:
                    ((GUIStyle)style).Draw(
                        GetRectTemp(rect), IGUTextObject.GetGUIContentTemp(content),
                        isHover, ID == GUIUtility.hotControl,
                        value, isFocused);
                    goto default;
                default: 
                    return value;
            }
        } 

        public static bool Toggle(IGURect rect, bool value, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, out bool onClick)
            => Toggle(rect, value, content, style, phy, ID, false, out onClick);

        public static float Slider(IGURect rect, float value, float size, MaxMinSlider maxMin, int ID, IGUBasicPhysics phy, IGUStyle style, IGUStyle styleThumb, bool isHoriz, bool isFocused) {
            Event @event = Event.current;
            GUIStyle sstyle = (GUIStyle)style;
            GUIStyle sstyleThumb = (GUIStyle)styleThumb;
            SliderStatus slider = (SliderStatus)GUIUtility.GetStateObject(typeof(SliderStatus), ID);

            Rect rectThumb = Rect.zero;
            slider.isHoriz = isHoriz;
            slider.SetMinMax(maxMin);
            slider.Size = size;

            if (slider.NormalSize == 1f) {
                rectThumb.size = isHoriz ? Vector2.right * rect.Width + Vector2.up * sstyleThumb.fixedHeight :
                    Vector2.right * sstyleThumb.fixedWidth + Vector2.up * rect.Height;
            } else {
                Vector2 vecSize = (rect.Size - (Vector2.right * sstyleThumb.padding.horizontal +
                    Vector2.up * sstyleThumb.padding.vertical)) * slider.NormalSize;

                rect = rect.SetSize(isHoriz ? Vector2.up * sstyle.fixedHeight + rect.Size.Multiplication(Vector2.right) :
                    Vector2.right * sstyle.fixedWidth + rect.Size.Multiplication(Vector2.up));

                rectThumb.size = isHoriz ? Vector2.right * (sstyleThumb.padding.horizontal + vecSize.x) + Vector2.up * sstyleThumb.fixedHeight :
                    Vector2.right * sstyleThumb.fixedWidth + Vector2.up * (sstyleThumb.padding.vertical + vecSize.y);
            }

            slider.Value = value;
            slider.rectSize = rect.Size - rectThumb.size;

            rectThumb.position = slider.NormalSize == 1f ? rect.Position : rect.Position + slider.GetThumbPosition();

            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;
            bool isThumbHover = rectThumb.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;

            switch (@event.type) {
                case EventType.MouseDown:
                    if (!phy.IsHotPotato)
                        break;
                    else if (slider.NormalSize == 1f) {
                        GUIUtility.hotControl = ID;
                        @event.Use();
                        break;
                    } else if (isHover) {
                        slider.PlaceThumbOnMouse(@event.mousePosition - rect.Position - rectThumb.size * .5f);
                        GUIUtility.hotControl = ID;
                        slider.startedPosition = @event.mousePosition;
                        slider.startedThumbPosition = slider.GetThumbPosition();
                        @event.Use();
                    } else if (isThumbHover) {
                        GUIUtility.hotControl = ID;
                        slider.startedPosition = @event.mousePosition;
                        slider.startedThumbPosition = rectThumb.position - rect.Position;
                        @event.Use();
                    }
                    break;
                case EventType.MouseUp:
                    GUIUtility.hotControl = 0;
                    break;
                case EventType.MouseDrag:
                    if (ID == GUIUtility.hotControl && slider.NormalSize != 1f) {
                        slider.currentPosition = @event.mousePosition;
                        slider.CalculatorThumbPosition();
                        @event.Use();
                    }
                    break;
                case EventType.Repaint:
                    sstyle.Draw(GetRectTemp(rect), GUIContent.none, ID);
                    sstyleThumb.Draw(rectThumb, isThumbHover, ID == GUIUtility.hotControl,
                        false, false);
                    break;
            }
            return slider.NormalSize == 1f ? slider.Min : Mathf.Clamp(slider.Value, slider.Min, slider.Max);
        }

        public static float Slider(IGURect rect, float value, float size, MaxMinSlider maxMin, int ID, IGUBasicPhysics phy, IGUStyle style, IGUStyle styleThumb, bool isHoriz)
            => Slider(rect, value, size, maxMin, ID, phy, style, styleThumb, isHoriz, false);

        public static float Slider(IGURect rect, float value, MaxMinSlider maxMin, int ID, IGUBasicPhysics phy, IGUStyle style, IGUStyle styleThumb, bool isHoriz)
            => Slider(rect, value, 0f, maxMin, ID, phy, style, styleThumb, isHoriz, false);

        public static IGURect SimpleWindow(IGURect rect, Rect rectDrag, Vector2 clippingScrollOffset, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, Action<int, Vector2> function, ref WindowFocusStatus focusStatus) {
            Event @event = Event.current;
            GUIStyle winStyle = (GUIStyle)style;
            WindowStatus window = (WindowStatus)GUIUtility.GetStateObject(typeof(WindowStatus), ID);
            window.IDFocus = ID;
            window.winFunc = function;
            rectDrag.position += rect.Position;

            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;
            bool isDrag = rectDrag.Contains(@event.mousePosition);

            switch (@event.type) {
                case EventType.MouseDown:
                    if (isHover) {
                        focusStatus = WindowFocusStatus.Focused;
                        GUI.FocusWindow(ID);
                    }
                    if (!isHover || !isDrag) { 
                        window.CurrentID = 0;
                        if (focusStatus == WindowFocusStatus.Focused)
                            focusStatus = WindowFocusStatus.Unfocused;
                        break;
                    }
                    GUIUtility.hotControl = window.CurrentID = ID;
                    window.CurrentPosition = @event.mousePosition - rect.Position;
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == ID)
                        GUIUtility.hotControl = 0;
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == ID) {
                        rect = rect.SetPosition(@event.mousePosition - window.CurrentPosition);
                        GUI.changed = true;
                    }
                    break;
                case EventType.Repaint:
                    winStyle.Draw(GetRectTemp(rect), IGUTextObject.GetGUIContentTemp(content), ID, window.CurrentID == ID);
                    break;
            }
            IGURect rectClip = rect;
            Clipping(rectClip.SetRotation(0f), clippingScrollOffset, window.ClippingFunc);
            return rect;
        }

        public static void TextBox(IGURect rect, IGUContent content, int ID, int maxLength, IGUBasicPhysics phy, IGUStyle style, bool isMultiline, ref bool isFocused)
            => InternalTextBox(rect, content, ID, maxLength, phy, style, char.MinValue, isMultiline, false, ref isFocused);

        public static void PasswordField(IGURect rect, IGUContent content, int ID, int maxLength, char mask, IGUBasicPhysics phy, IGUStyle style, ref bool isFocused)
            => InternalTextBox(rect, content, ID, maxLength, phy, style, mask, false, true, ref isFocused);

        public static void SelectableText(IGURect rect, IGUContent content, int ID, IGUBasicPhysics phy, IGUStyle style, ref bool isFocused) {
            Event current = Event.current;
            TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), ID);
            editor.text = content.Text;
            editor.SaveBackup();
            editor.position = (Rect)rect;
            editor.controlID = ID;
            editor.multiline = true;
            editor.isPasswordField = false;
            editor.style = (GUIStyle)style;
            editor.DetectFocusChange();

            bool isHover = editor.position.Contains(current.mousePosition) && phy.IsHotPotato && GUI.enabled;

            switch (current.type) {
                case EventType.MouseUp:
                    if (!isHover) {
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
                    if (!isHover) break;
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
        }

        private static void InternalTextBox(IGURect rect, IGUContent content, int ID, int maxLength, IGUBasicPhysics phy, IGUStyle style, char mask, bool isMultiline, bool isPasswordField, ref bool isFocused) {
            Event @event = Event.current;
            GUIStyle textStyle = (GUIStyle)style;
            TextEditorStatus textEditorStatus = (TextEditorStatus)GUIUtility.GetStateObject(typeof(TextEditorStatus), ID);
            textEditorStatus.CharMask = mask;
            textEditorStatus.MaxLength = maxLength;

            bool flag = false;
            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;

            textEditorStatus.isPasswordField = isPasswordField;
#if UNITY_EDITOR
            if (textEditorStatus.isPasswordField) {
                flag = textEditorStatus.TextMod != content.Text;
                textEditorStatus.TextMod = content.Text;
                if (textEditorStatus.MaxLength != content.Text.Length)
                    textEditorStatus.text = string.Empty.PadLeft(content.Text.Length, mask);
                // flag = textEditorStatus.text != content.Text;
                // textEditorStatus.text = content.Text;
            } else {
                flag = textEditorStatus.text != content.Text;
                textEditorStatus.text = content.Text;
            }
#else
            if (textEditorStatus.isPasswordField) {
                textEditorStatus.TextMod = content.Text;
                if (textEditorStatus.MaxLength != content.Text.Length)
                    textEditorStatus.text = string.Empty.PadLeft(content.Text.Length, mask);
            } else {
                textEditorStatus.text = content.Text;
            }
#endif
            textEditorStatus.SaveBackup();
            textEditorStatus.controlID = ID;
            textEditorStatus.position = GetRectTemp(rect);
            textEditorStatus.style = textStyle;
            textEditorStatus.multiline = isMultiline;
            textEditorStatus.DetectFocusChange();

            switch (@event.type) {
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == ID) {
                        textEditorStatus.MouseDragSelectsWholeWords(isFocused = false);
                        GUIUtility.hotControl = 0;
                        @event.Use();
                    }
                    break;
                case EventType.MouseDown:
                    if (!isHover) break;
                    GUIUtility.hotControl =
                        GUIUtility.keyboardControl = ID;
                    isFocused = true;
                    textEditorStatus.OnFocus();
                    textEditorStatus.MoveCursorToPosition(@event.mousePosition);
                    if (@event.clickCount == 2 && GUI.skin.settings.doubleClickSelectsWord) {
                        textEditorStatus.SelectCurrentWord();
                        textEditorStatus.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
                        textEditorStatus.MouseDragSelectsWholeWords(true);
                    }
                    if (@event.clickCount == 3 && GUI.skin.settings.tripleClickSelectsLine) {
                        textEditorStatus.SelectCurrentParagraph();
                        textEditorStatus.MouseDragSelectsWholeWords(true);
                        textEditorStatus.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
                    }
                    @event.Use();
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == ID) {
                        if (@event.shift) textEditorStatus.MoveCursorToPosition(@event.mousePosition);
                        else textEditorStatus.SelectToPosition(@event.mousePosition);
                        @event.Use();
                    }
                    break;
                case EventType.ValidateCommand:
                case EventType.ExecuteCommand:
                    if (GUIUtility.keyboardControl == ID) {
                        if (@event.commandName == "Copy") {
                            textEditorStatus.Copy();
                            @event.Use();
                        } else if (@event.commandName == "Paste") {
                            flag = textEditorStatus.Paste();
                            @event.Use();
                        }
                    }
                    break;
                case EventType.KeyDown:
                    if (GUIUtility.keyboardControl != ID) break;
                    if (IsCopyOrPasteEvent(@event))
                        break;
                    char character = @event.character;
                    if (@event.keyCode == KeyCode.Backspace && textEditorStatus.isPasswordField) {
                        StringBuilder builder = new StringBuilder(textEditorStatus.TextMod);
                        int slecIndex = textEditorStatus.SelectionSize;
                        int curPos = textEditorStatus.CursorPosition;
                        if (curPos != 0 && slecIndex == 0) {
                            --curPos;
                            slecIndex = 1;
                        }

                        textEditorStatus.TextMod = builder.Remove(curPos, slecIndex).ToString();
                    }
                    if (textEditorStatus.HandleKeyEvent(@event)) {
                        @event.Use();
                        flag =
                            GUI.changed = true;
                        content.Text = textEditorStatus.TextMod;
                        break;
                    }

                    Font font = textStyle.font;
                    font = !font ? GUI.skin.font : font;
#if UNITY_EDITOR
                    if (@event.keyCode == KeyCode.Tab || character == '\t')
                        break;
#else
                    if (@event.keyCode == KeyCode.Tab || character == '\t') {
                        if (textEditorStatus.isPasswordField) {
                            StringBuilder builder = new StringBuilder(textEditorStatus.TextMod);
                            textEditorStatus.TextMod = builder.Insert(textEditorStatus.CursorPosition, character).ToString();
                            textEditorStatus.Insert(mask);
                        } else textEditorStatus.Insert(character);
                        flag =
                            GUI.changed = true;
                        break;
                    }
#endif
                    if (character == '\n' && !isMultiline && !@event.alt)
                        break;

                    if (font.HasCharacter(character) || character == '\n') {
                        if (textEditorStatus.isPasswordField) {
                            StringBuilder builder = new StringBuilder(textEditorStatus.TextMod);
                            textEditorStatus.TextMod = builder.Insert(textEditorStatus.CursorPosition, character).ToString();
                            textEditorStatus.Insert(mask);
                        } else textEditorStatus.Insert(character);
                        flag =
                            GUI.changed = true;
                        break;
                    }

                    switch (@event.keyCode) {
                        case KeyCode.UpArrow:
                            if (@event.shift) textEditorStatus.SelectUp();
                            else textEditorStatus.MoveUp();
                            break;
                        case KeyCode.DownArrow:
                            if (@event.shift) textEditorStatus.SelectDown();
                            else textEditorStatus.MoveDown();
                            break;
                        case KeyCode.LeftArrow:
                            if (@event.shift) textEditorStatus.SelectLeft();
                            else textEditorStatus.MoveLeft();
                            break;
                        case KeyCode.RightArrow:
                            if (@event.shift) textEditorStatus.SelectRight();
                            else textEditorStatus.MoveRight();
                            break;
                        case KeyCode.Home:
                            if (@event.shift) textEditorStatus.SelectGraphicalLineStart();
                            else textEditorStatus.MoveGraphicalLineStart();
                            break;
                        case KeyCode.End:
                            if (@event.shift) textEditorStatus.SelectGraphicalLineEnd();
                            else textEditorStatus.MoveGraphicalLineEnd();
                            break;
                        case KeyCode.PageUp:
                            if (@event.shift) textEditorStatus.SelectTextStart();
                            else textEditorStatus.MoveTextStart();
                            break;
                        case KeyCode.PageDown:
                            if (@event.shift) textEditorStatus.SelectTextEnd();
                            else textEditorStatus.MoveTextEnd();
                            break;
                    }
                    @event.Use();
                    break;
                case EventType.Repaint:
                    if (GUIUtility.keyboardControl != ID)
                        textEditorStatus.style.Draw(GetRectTemp(rect), IGUTextObject.GetGUIContentTemp(textEditorStatus.text), ID);
                    else textEditorStatus.DrawCursor(textEditorStatus.text);
                    break;
            }
            textEditorStatus.UpdateScrollOffsetIfNeeded(@event);
            if (textEditorStatus.IsMaxLength) {
                GUI.changed = false;
                return;
            } else if (!flag) return;
            content.Text = textEditorStatus.TextMod;
        }

        public static void Clipping(IGURect rect, Vector2 scrollOffset, Action<Vector2> clippinFunc) {
            scrollOffset = Vector2.zero - rect.Position + scrollOffset;
            GUI.BeginClip(GetRectTemp(rect), scrollOffset, Vector2.zero, false);
            clippinFunc(scrollOffset);
            GUI.EndClip();
        }

        public static void TextureBox(IGURect rect, Texture image, ScaleMode scaleMode, bool alphaBlend, float imageAspect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
            => GUI.DrawTexture(GetRectTemp(rect), image, scaleMode, alphaBlend, imageAspect, color, borderWidths, borderRadiuses);

        private static Rect GetRectTemp(IGURect rect) {
            rectTemp.position = rect.Position;
            rectTemp.size = rect.Size;
            return rectTemp;
        }

        private static void FocusWindow(int id) {
            GUI.FocusWindow(id);
            IGUCanvasContainer.FocusWindow(id);
        }

        private static bool IsCopyOrPasteEvent(Event @event) {
            bool result;
            if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX) {
                result = IsCopyOrPasteControl(@event, Event.KeyboardEvent("%c")) || IsCopyOrPasteControl(@event, Event.KeyboardEvent("$v"));
            } else {
                result = IsCopyOrPasteControl(@event, Event.KeyboardEvent("^c")) || IsCopyOrPasteControl(@event, Event.KeyboardEvent("^v")) ||
                IsCopyOrPasteControl(@event, Event.KeyboardEvent("^insert")) || IsCopyOrPasteControl(@event, Event.KeyboardEvent("#insert"));
            }
            return result;
        }

        private static bool IsCopyOrPasteControl(Event A, Event B)
            => A.modifiers == B.modifiers && A.keyCode == B.keyCode;
    }
}