using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU {
    public static class TDS_IGUStyle {

        public static void DrawLabel(Rect rect, IGUStyle style, IGUContent content)
            => ((GUIStyle)style).Draw(
                rect, IGUTextObject.GetGUIContentTemp(content), 
                GUIUtility.GetControlID(FocusType.Passive, rect));

        public static bool DrawButton(Rect rect, IGUStyle style, IGUContent content, IGUBasicPhysics phy, bool isFocused) {
            int ID = GUIUtility.GetControlID(FocusType.Passive, rect);
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition) && phy.IsHotPotato;
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
                        rect, IGUTextObject.GetGUIContentTemp(content),
                        isHover, ID == GUIUtility.hotControl,
                        false, isFocused);
                    goto default;
                default:
                    return false;
            }
        }

        public static bool DrawButton(Rect rect, IGUStyle style, IGUContent content, IGUBasicPhysics phy)
            => DrawButton(rect, style, content, phy, false);

        public static bool DrawRepeatButton(Rect rect, IGUStyle style, IGUContent content, out bool onClick, bool isFocused, bool noAction) {
            int ID = GUIUtility.GetControlID(FocusType.Passive, rect);
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition) && !noAction;
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
                        rect, IGUTextObject.GetGUIContentTemp(content),
                        isHover, ID == GUIUtility.hotControl,
                        false, isFocused);
                    return ID == GUIUtility.hotControl && isHover;
                default:
                    return false;
            }
        }

        public static bool DrawRepeatButton(Rect rect, IGUStyle style, IGUContent content, out bool onClick, bool isFocused)
            => DrawRepeatButton(rect, style, content, out onClick, isFocused, false);

        public static bool DrawRepeatButton(Rect rect, IGUStyle style, IGUContent content, out bool onClick)
            => DrawRepeatButton(rect, style, content, out onClick, false, false);

        public static bool DrawRepeatButton(Rect rect, IGUStyle style, IGUContent content)
            => DrawRepeatButton(rect, style, content, out _, false, false);

        public static bool DrawToggle(Rect rect, bool value, IGUStyle style, IGUContent content, out bool onClick, bool isFocused, bool noAction) {
            int ID = GUIUtility.GetControlID(FocusType.Passive, rect);
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition) && !noAction;
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
                        rect, IGUTextObject.GetGUIContentTemp(content),
                        isHover, ID == GUIUtility.hotControl,
                        value, isFocused);
                    goto default;
                default: return value;
            }
        }

        public static bool DrawToggle(Rect rect, bool value, IGUStyle style, IGUContent content, out bool onClick, bool noAction)
            => DrawToggle(rect, value, style, content, out onClick, false, noAction);

        public static bool DrawToggle(Rect rect, bool value, IGUStyle style, IGUContent content, out bool onClick)
            => DrawToggle(rect, value, style, content, out onClick, false, false);

        public static bool DrawToggle(Rect rect, bool value, IGUStyle style, IGUContent content)
            => DrawToggle(rect, value, style, content, out _, false, false);

        public static float DrawSlider(Rect rect, float value, float size, float min, float max, int ID, IGUStyle style, IGUStyle styleThumb, bool isHoriz, bool isFocused, bool noAction) {
            Event @event = Event.current;
            GUIStyle sstyle = (GUIStyle)style;
            GUIStyle sstyleThumb = (GUIStyle)styleThumb;
            SliderStatus slider = (SliderStatus)GUIUtility.GetStateObject(typeof(SliderStatus), ID);

            Rect rectThumb = Rect.zero;
            slider.isHoriz = isHoriz;
            slider.SetMinMax(min, max);
            slider.Size = size;

            if (slider.NormalSize == 1f) {
                rectThumb.size = isHoriz ? Vector2.right * rect.width + Vector2.up * sstyleThumb.fixedHeight :
                    Vector2.right * sstyleThumb.fixedWidth + Vector2.up * rect.height;
            } else {
                Vector2 vecSize = (rect.size - (Vector2.right * sstyleThumb.padding.horizontal +
                    Vector2.up * sstyleThumb.padding.vertical)) * slider.NormalSize;

                rect.size = isHoriz ? Vector2.up * sstyle.fixedHeight + rect.size.Multiplication(Vector2.right) :
                    Vector2.right * sstyle.fixedWidth + rect.size.Multiplication(Vector2.up);

                rectThumb.size = isHoriz ? Vector2.right * (sstyleThumb.padding.horizontal + vecSize.x) + Vector2.up * sstyleThumb.fixedHeight :
                    Vector2.right * sstyleThumb.fixedWidth + Vector2.up * (sstyleThumb.padding.vertical + vecSize.y);
            }

            slider.Value = value;
            slider.rectSize = rect.size - rectThumb.size;

            rectThumb.position = slider.NormalSize == 1f ? rect.position : rect.position + slider.GetThumbPosition();

            bool isHover = rect.Contains(@event.mousePosition) && !noAction;
            bool isThumbHover = rectThumb.Contains(@event.mousePosition) && !noAction;

            switch (@event.type) {
                case EventType.MouseDown:
                    if (noAction)
                        break;
                    else if (slider.NormalSize == 1f) {
                        GUIUtility.hotControl = ID;
                        @event.Use();
                        break;
                    } else if (isHover) {
                        slider.PlaceThumbOnMouse(@event.mousePosition - rect.position - rectThumb.size * .5f);
                        GUIUtility.hotControl = ID;
                        slider.startedPosition = @event.mousePosition;
                        slider.startedThumbPosition = slider.GetThumbPosition();
                        @event.Use();
                    } else if (isThumbHover) {
                        GUIUtility.hotControl = ID;
                        slider.startedPosition = @event.mousePosition;
                        slider.startedThumbPosition = rectThumb.position - rect.position;
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
                    sstyle.Draw(rect, GUIContent.none, ID);
                    sstyleThumb.Draw(rectThumb, isThumbHover, ID == GUIUtility.hotControl,
                        false, false);
                    break;
            }

            return slider.NormalSize == 1f ? slider.Min : Mathf.Clamp(slider.Value, slider.Min, slider.Max);
        }

        public static Rect DrawWindow(Rect rect, Rect rectDrag, int ID, GUI.WindowFunction function, IGUStyle style, IGUContent container) {
            Event @event = Event.current;
            GUIStyle winStyle = (GUIStyle)style;
            SliderStatus window = (SliderStatus)GUIUtility.GetStateObject(typeof(SliderStatus), ID);

            rectDrag.position += rect.position;

            bool isHover = rect.Contains(@event.mousePosition);
            bool isDrag = rectDrag.Contains(@event.mousePosition);

            switch (@event.type) {
                case EventType.MouseDown:
                    if (isHover)
                        GUI.FocusWindow(ID);
                    if (!isHover || !isDrag) break;
                    GUIUtility.hotControl = ID;
                    window.currentPosition = @event.mousePosition - rect.position;
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == ID)
                        GUIUtility.hotControl = 0;
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == ID)
                        rect.position = @event.mousePosition - window.currentPosition;
                    break;
                case EventType.Repaint:
                    winStyle.Draw(rect, IGUTextObject.GetGUIContentTemp(container), ID, GUIUtility.hotControl == ID);
                    break;
            }
            GUI.BeginClip(rect, Vector2.zero, Vector2.zero, false);
            function?.Invoke(ID);
            GUI.EndClip();
            return rect;
        }

        public static IGUContent DrawTextBox(Rect rect, IGUContent text, int ID, IGUStyle style, char maskChar, bool isMultiline, bool isPasswordField) {

            Event @event = Event.current;
            GUIStyle textStyle = (GUIStyle)style;
            TextEditorStatus textEditorStatus = (TextEditorStatus)GUIUtility.GetStateObject(typeof(TextEditorStatus), ID);
            TextEditor textEditor = textEditorStatus.textEditor;

            bool flag = false;
            bool isHover = rect.Contains(@event.mousePosition);

#if UNITY_EDITOR
            flag = textEditor.text != text.Text;
            textEditor.text = text.Text;
#else
            textEditor.text = text.Text;
#endif
            textEditor.SaveBackup();
            textEditor.controlID = ID;
            textEditor.position = rect;
            textEditor.style = textStyle;
            textEditor.multiline = isMultiline;
            textEditor.isPasswordField = isPasswordField;
            textEditor.DetectFocusChange();

            switch (@event.type) {
                case EventType.MouseUp:
                    if (!isHover) {
                        //isFocused = false;
                        GUIUtility.keyboardControl = 0;
                        textEditor.OnLostFocus();
                    }
                    if (GUIUtility.hotControl == ID) {
                        textEditor.MouseDragSelectsWholeWords(false);
                        GUIUtility.hotControl = 0;
                        @event.Use();
                    }
                    break;
                case EventType.MouseDown:
                    if (!isHover) break;
                    GUIUtility.hotControl =
                        GUIUtility.keyboardControl = ID;
                    if (GUIUtility.keyboardControl == ID) {
                        //isFocused = true;
                        textEditor.OnFocus();
                    }
                    textEditor.MoveCursorToPosition(@event.mousePosition);
                    if (@event.clickCount == 2 && GUI.skin.settings.doubleClickSelectsWord) {
                        textEditor.SelectCurrentWord();
                        textEditor.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
                        textEditor.MouseDragSelectsWholeWords(true);
                    }
                    if (@event.clickCount == 3 && GUI.skin.settings.tripleClickSelectsLine) {
                        textEditor.SelectCurrentParagraph();
                        textEditor.MouseDragSelectsWholeWords(true);
                        textEditor.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
                    }
                    @event.Use();
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == ID) {
                        if (@event.shift) textEditor.MoveCursorToPosition(@event.mousePosition);
                        else textEditor.SelectToPosition(@event.mousePosition);
                        @event.Use();
                    }
                    break;
                case EventType.ValidateCommand:
                case EventType.ExecuteCommand:
                    if (GUIUtility.keyboardControl == ID) {
                        if (@event.commandName == "Copy") {
                            textEditor.Copy();
                            @event.Use();
                        } else if (@event.commandName == "Paste") {
                            textEditor.Paste();
                            @event.Use();
                        }
                    }
                    break;
                case EventType.KeyDown:
                    if (GUIUtility.keyboardControl != ID) break;
                    if (textEditor.HandleKeyEvent(@event)) {
                        @event.Use();
                        flag = true;
                        text.Text = textEditor.text;
                        break;
                    }
                    char character = @event.character;
#if UNITY_EDITOR
                    if (@event.keyCode == KeyCode.Tab || character == '\t')
                        break;
#else
                    if (@event.keyCode == KeyCode.Tab || character == '\t') {
                        textEditor.Insert(character);
                        flag = true;
                        break;
                    }
#endif
                    if (character == '\n' && !isMultiline && !@event.alt)
                        break;
                    Font font = textStyle.font;
                    font = !font ? GUI.skin.font : font;

                    if (font.HasCharacter(character) || character == '\n') {
                        textEditor.Insert(character);
                        flag = true;
                        break;
                    }

                    switch (@event.keyCode) {
                        case KeyCode.UpArrow:
                            if (@event.shift) textEditor.SelectUp();
                            else textEditor.MoveUp();
                            break;
                        case KeyCode.DownArrow:
                            if (@event.shift) textEditor.SelectDown();
                            else textEditor.MoveDown();
                            break;
                        case KeyCode.LeftArrow:
                            if (@event.shift) textEditor.SelectLeft();
                            else textEditor.MoveLeft();
                            break;
                        case KeyCode.RightArrow:
                            if (@event.shift) textEditor.SelectRight();
                            else textEditor.MoveRight();
                            break;
                        case KeyCode.Home:
                            if (@event.shift) textEditor.SelectGraphicalLineStart();
                            else textEditor.MoveGraphicalLineStart();
                            break;
                        case KeyCode.End:
                            if (@event.shift) textEditor.SelectGraphicalLineEnd();
                            else textEditor.MoveGraphicalLineEnd();
                            break;
                        case KeyCode.PageUp:
                            if (@event.shift) textEditor.SelectTextStart();
                            else textEditor.MoveTextStart();
                            break;
                        case KeyCode.PageDown:
                            if (@event.shift) textEditor.SelectTextEnd();
                            else textEditor.MoveTextEnd();
                            break;
                    }
                    @event.Use();
                    break;
                case EventType.Repaint:
                    if (GUIUtility.keyboardControl != ID)
                        textEditor.style.Draw(rect, IGUTextObject.GetGUIContentTemp(textEditorStatus.textPasswordField), ID);
                    else textEditor.DrawCursor(textEditorStatus.textPasswordField);
                    break;
            }
            textEditor.UpdateScrollOffsetIfNeeded(@event);

            if (!flag)
                return text;
            textEditorStatus.textPasswordField = string.Empty.PadRight(textEditor.text.Length, maskChar);
            text.Text = textEditor.text;
            return text;
        }

        public static void RectClip(Rect rect, Action action, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset) {
            GUI.BeginClip(rect, scrollOffset, renderOffset, resetOffset);
            action?.Invoke();
            GUI.EndClip();
        }

        public sealed class TextEditorStatus {
            public TextEditor textEditor;
            public string textPasswordField;

            public TextEditorStatus() {
                textEditor = new TextEditor();
                textPasswordField = string.Empty;
            }
        }

        public sealed class IGUObjectInfo {
            public int ID;
            public bool onClick;
            public bool isFocused;
            public bool noAction;

            public IGUObjectInfo() {
                this.ID = 0;
                this.onClick =
                this.isFocused =
                this.noAction = false;
            }
        }

        public sealed class SliderStatus {
            public bool isHoriz;
            public Vector2 rectSize;
            public Vector2 startedPosition;
            public Vector2 currentPosition;
            public Vector2 startedThumbPosition;
            private float min;
            private float max;
            private float size;
            private float value;
            private float normalSize;
            private Vector2 thumbPosX;
            private Vector2 thumbPosY;
            private Vector2 posCompare;

            public float Min => min;
            public float Max => max - size;
            public float NormalSize => normalSize;
            public float MinMaxLength => (float)Math.Abs(max - min);
            public float Size { get => size; set => SetSize(value); }
            public float Value { get => value; set => SetValue(value); }

            public void SetMinMax(float min, float max) {
                if (min > max) {
                    this.max = min;
                    this.min = max;
                } else {
                    this.min = min;
                    this.max = max;
                }
            }

            public void PlaceThumbOnMouse(Vector2 mousePosition)
                => CalculatorThumbPosition(isHoriz ? mousePosition.Multiplication(Vector2.right) : mousePosition.Multiplication(Vector2.up));

            public void CalculatorThumbPosition()
                => CalculatorThumbPosition(startedThumbPosition + (currentPosition - startedPosition));

            public Vector2 GetThumbPosition() => isHoriz ? thumbPosX : thumbPosY;

            private void CalculatorThumbPosition(Vector2 position) {
                if (posCompare == (posCompare = position)) return;
                Vector2 length = (Vector2.one * (MinMaxLength - size)).Division(rectSize);
                posCompare = posCompare.Multiplication(length).Division(rectSize.Multiplication(length));
                posCompare = posCompare.Multiplication(Vector2.one * (MinMaxLength - size)) + Vector2.one * min;
                Value = isHoriz ? posCompare.x : posCompare.y;
            }

            private void SetValue(float value) {
                if (this.value == (this.value = value)) return;
                thumbPosX = rectSize.Multiplication(Vector2.right) * (Mathf.Abs(this.value - min) / (MinMaxLength - size));
                thumbPosY = rectSize.Multiplication(Vector2.up) * (Mathf.Abs(this.value - min) / (MinMaxLength - size));
            }

            private void SetSize(float size) {
                if (this.size == size) return;
                this.size = Mathf.Clamp(size, 0f, MinMaxLength);
                normalSize = this.size / MinMaxLength;
            }
        }
    
        /* 
         * criar rectclip
         * bom criando o rectclip tem que criar uma especie de profundidade para a "fisica" dos
         * elementos igu o rectclip vai ser +/- a base do sistema IGU.
         */

    }

    public class TDS_IGURectClip : IDisposable {

        public IGUPhysics physics;

        public TDS_IGURectClip()
        {
            physics = new IGUPhysics();
        }

        public void Dispose()  {
            physics?.Dispose();
        }

        public void Draw(Rect position, Action function, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset) {
            GUI.BeginClip(position, scrollOffset, renderOffset, resetOffset);
            function?.Invoke();
            GUI.EndClip();
        }

        public void Draw(Rect position, Action function, Vector2 scrollOffset, Vector2 renderOffset)
            => Draw(position, function, scrollOffset, renderOffset, false);

        public void Draw(Rect position, Action function, Vector2 scrollOffset)
            => Draw(position, function, scrollOffset, Vector2.zero);

        public void Draw(Rect position, Action function)
            => Draw(position, function, Vector2.zero);
    }

    public sealed class IGUPhysics : IDisposable {
        public Rect rect;
        public bool onPhysycs;
        public IGUConfig config;

        public Action onDestroy;

        public void Dispose() {
            onDestroy?.Invoke();
        }

        public void IGUPhysicsFeedbackInfo(IGUPhysicsFeedback feedback) {
        
        }
    }

    /*
     * tenho que o quema de batata quente onde o ultimo fica quente
     * bom isso fica da forma onde o rato fica em cima do objeto e o ultimo e selecionado
     * incluindo o rect do beginclip
     */

    public class IGUPhysicsFeedback {

        public static event Action<IGUPhysicsFeedback> _PhysicsFeedback;
        public static IGUPhysicsFeedback feedback = new IGUPhysicsFeedback();

        public static void Add(IGUPhysics physicsItem) {
            if (physicsItem == null) return;
            _PhysicsFeedback?.Invoke(feedback);
            _PhysicsFeedback += physicsItem.IGUPhysicsFeedbackInfo;
        }

        public static void Remove(IGUPhysics physicsItem) {
            if (physicsItem == null) return;
            _PhysicsFeedback -= physicsItem.IGUPhysicsFeedbackInfo;
            _PhysicsFeedback?.Invoke(feedback);
        }
    }
}
