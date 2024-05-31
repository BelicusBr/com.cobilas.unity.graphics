using System;
using UnityEngine;
using System.Text;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    public static class BackEndIGU {
        private static Rect rectTemp = Rect.zero;
        private readonly static List<int> controlList = new List<int>();

        public static void Label(IGURect rect, IGUContent content, IGUStyle style, int ID)
            => ((GUIStyle)style).DrawRepaint((Rect)rect, (GUIContent)content, ID);

        public static bool Button(IGURect rect, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, bool isFocused, out bool onClick) {
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;
            onClick = false;
            switch (@event.type) {
                case EventType.MouseDown:
                    if (isHover) {
                        PullID(ID);
                        onClick = true;
                        @event.Use();
                    }
                    goto default;
                case EventType.MouseUp:
                    if (!IDInControlList(ID))
                        goto default;
                    else if (!isHover) {
                        PushID(ID);
                        goto default;
                    }
                    PushID(ID);
                    @event.Use();
                    return GUI.enabled = true;
                case EventType.Repaint:
                    ((GUIStyle)style).Draw(
                        GetRectTemp(rect), IGUTextObject.GetGUIContentTemp(content),
                        isHover, IDInControlList(ID),
                        false, isFocused);
                    goto default;
                default:
                    return false;
            }
        }

        public static bool Button(IGURect rect, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, out bool onClick)
            => Button(rect, content, style, phy, ID, false, out onClick);

        public static bool Button(IGURect rect, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID)
            => Button(rect, content, style, phy, ID, false, out _);

        public static bool RepeatButton(IGURect rect, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, bool isFocused, out bool onClick) {
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;
            onClick = false;
            switch (@event.GetTypeForControl(ID)) {
                case EventType.MouseDown:
                    if (isHover) {
                        onClick = true;
                        PullID(ID);
                        @event.Use();
                    }
                    goto default;
                case EventType.MouseUp:
                    if (!IDInControlList(ID))
                        goto default;
                    else if (!isHover) {
                        PushID(ID);
                        goto default;
                    }
                    PushID(ID);
                    @event.Use();
                    return GUI.enabled = true;
                case EventType.Repaint:
                    ((GUIStyle)style).Draw(
                        GetRectTemp(rect), IGUTextObject.GetGUIContentTemp(content),
                        isHover, IDInControlList(ID),
                        false, isFocused);
                    return IDInControlList(ID) && isHover;
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
                        PullID(ID);
                        onClick = true;
                        @event.Use();
                    }
                    goto default;
                case EventType.MouseUp:
                    if (!IDInControlList(ID))
                        goto default;
                    else if (!isHover) {
                        PushID(ID);
                        goto default;
                    }
                    PushID(ID);
                    value = !value;
                    GUI.enabled = true;
                    @event.Use();
                    goto default;
                case EventType.Repaint:
                    ((GUIStyle)style).Draw(
                        GetRectTemp(rect), IGUTextObject.GetGUIContentTemp(content),
                        isHover, IDInControlList(ID),
                        value, isFocused);
                    goto default;
                default: 
                    return value;
            }
        } 

        public static bool Toggle(IGURect rect, bool value, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, out bool onClick)
            => Toggle(rect, value, content, style, phy, ID, false, out onClick);

        public static float Slider(IGURect rect, float value, float size, MaxMinSlider maxMin, IGUBasicPhysics phy, int id, bool useScrollWheel, bool isHoriz, IGUStyle slider, IGUStyle thumb) {
            Rect rect_slider = (Rect)rect;
            Event current = Event.current;
            Rect rect_slider_thumb = Rect.zero;
            GUIStyle i_thumb = (GUIStyle)thumb;
            GUIStyle i_slider = (GUIStyle)slider;
            SliderStatus sliderStatus = (SliderStatus)GUIUtility.GetStateObject(typeof(SliderStatus), id);

            sliderStatus.Size = size;
            sliderStatus.Value = value;
            sliderStatus.MaxMin = maxMin;
            sliderStatus.isHoriz = isHoriz;

            Vector2 snum = Vector2.right * i_thumb.padding.horizontal + Vector2.up * i_thumb.padding.vertical;

            size *= (isHoriz ? rect.Width - snum.x : rect.Height - snum.y) / Mathf.Abs(maxMin.Max - maxMin.Min);

            rect_slider_thumb.position = rect_slider.position +
                Vector2.right * (isHoriz ? i_thumb.margin.right + i_slider.padding.right : i_thumb.margin.left + i_slider.padding.left) + 
                Vector2.up * (isHoriz ? i_thumb.margin.bottom + i_slider.padding.bottom : i_thumb.margin.top + i_slider.padding.top);
            rect_slider_thumb.size = snum + (isHoriz ? size * Vector2.right : size * Vector2.up);

            sliderStatus.rectSize = rect_slider.size - rect_slider_thumb.size -
                (isHoriz ? Vector2.right * i_slider.padding.horizontal : Vector2.up * i_slider.padding.vertical);
            sliderStatus.startThumbPosition = rect_slider_thumb.position;

            float clamp_s_x = i_slider.fixedWidth <= 0 ? rect.Width : Mathf.Clamp(rect.Width, 0f, i_slider.fixedWidth);
            float clamp_s_y = i_slider.fixedHeight <= 0 ? rect.Height : Mathf.Clamp(rect.Height, 0f, i_slider.fixedHeight);

            bool isHover = rect.SetSize(clamp_s_x, clamp_s_y).Contains(current.mousePosition) && phy.IsHotPotato && GUI.enabled;

            switch (current.type) {
                case EventType.MouseDown:
                    if (isHover) {
                        PullID(id);
                        sliderStatus.startPosition = current.mousePosition;
                        sliderStatus.CalculatorThumbPosition(current.mousePosition, rect_slider_thumb.size, rect_slider);
                        current.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (IDInControlList(id)) {
                        PushID(id);
                        current.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (!IDInControlList(id)) break;
                    sliderStatus.CalculatorThumbPosition(current.mousePosition, rect_slider_thumb.size, rect_slider);
                    current.Use();
                    break;
                case EventType.ScrollWheel:
                    if (isHover && useScrollWheel) {
                        sliderStatus.Value += current.delta.y;
                        current.Use();
                    }
                    break;
                case EventType.Repaint:
                    rect_slider_thumb.position = sliderStatus.GetThumbPosition();
                    i_slider.Draw(rect_slider, GUIContent.none, id);
                    i_thumb.Draw(rect_slider_thumb, rect_slider_thumb.Contains(current.mousePosition), 
                        GUIUtility.hotControl == id, false, false);
                    break;
            }

            return sliderStatus.Value;
        }

        public static float Slider(IGURect rect, float value, float size, MaxMinSlider maxMin, IGUBasicPhysics phy, int id, bool isHoriz, IGUStyle slider, IGUStyle thumb)
            => Slider(rect, value, size, maxMin, phy, id, false, isHoriz, slider, thumb);

        public static float Slider(IGURect rect, float value, MaxMinSlider maxMin, IGUBasicPhysics phy, int id, bool useScrollWheel, bool isHoriz, IGUStyle slider, IGUStyle thumb)
            => Slider(rect, value, 0f, maxMin, phy, id, useScrollWheel, isHoriz, slider, thumb);

        public static float Slider(IGURect rect, float value, MaxMinSlider maxMin, IGUBasicPhysics phy, int id, bool isHoriz, IGUStyle slider, IGUStyle thumb)
            => Slider(rect, value, 0f, maxMin, phy, id, false, isHoriz, slider, thumb);

        public static IGURect SimpleWindow(IGURect rect, Rect rectDrag, Vector2 clippingScrollOffset, IGUContent content, IGUStyle style, IGUBasicPhysics phy, int ID, Action<int, Vector2> function, ref WindowFocusStatus focusStatus) {
            Event @event = Event.current;
            GUIStyle winStyle = (GUIStyle)style;
            WindowStatus window = (WindowStatus)GUIUtility.GetStateObject(typeof(WindowStatus), ID);
            window.IDFocus = ID;
            window.winFunc = function;
            rectDrag.position += rect.Position;

            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato && GUI.enabled;

            switch (@event.type) {
                case EventType.MouseDown:
                    if (!isHover) { 
                        PushID(window.CurrentID);
                        window.CurrentID = 0;
                        if (focusStatus == WindowFocusStatus.Focused)
                            focusStatus = WindowFocusStatus.Unfocused;
                        break;
                    }
                    focusStatus = WindowFocusStatus.Focused;
                    PullID(window.CurrentID = ID);
                    window.CurrentPosition = @event.mousePosition - rect.Position;
                    break;
                case EventType.MouseUp:
                    if (IDInControlList(ID))
                        PushID(ID);
                    break;
                case EventType.MouseDrag:
                    if (IDInControlList(ID)) {
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
                    if (IDInControlList(ID)) {
                        editor.MouseDragSelectsWholeWords(false);
                        PushID(ID);
                        current.Use();
                    }
                    break;
                case EventType.MouseDown:
                    if (!isHover) break;
                    PullID(GUIUtility.keyboardControl = ID);
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
                    if (IDInControlList(ID)) {
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
                    if (IDInControlList(ID)) {
                        textEditorStatus.MouseDragSelectsWholeWords(isFocused = false);
                        PushID(ID);
                        @event.Use();
                    }
                    break;
                case EventType.MouseDown:
                    if (!isHover) break;
                    PullID(GUIUtility.keyboardControl = ID);
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
                    if (IDInControlList(ID)) {
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

        /// <summary>Will add the id in the hot controls list.</summary>
        private static void PullID(int id) {
            if (!controlList.Contains(id))
                controlList.Add(id);
        }

        /// <summary>Checks if the ID is in the hot controls list.</summary>
        /// <param name="id">The id that will be checked.</param>
        /// <returns>Will return if the id is in the hot controls list.</returns>
        private static bool IDInControlList(int id)
            => controlList.Contains(id);

        /// <summary>Will remove the id in the hot controls list.</summary>
        private static void PushID(int id) {
            if (controlList.Contains(id))
                controlList.Remove(id);
        }
    }
}