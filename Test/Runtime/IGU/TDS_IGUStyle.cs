using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU {
    public static class TDS_IGUStyle {

        public static void DrawLabel(Rect rect, IGUStyle style, IGUContent content)
            => ((GUIStyle)style).Draw(
                rect, IGUTextObject.GetGUIContentTemp(content), 
                GUIUtility.GetControlID(FocusType.Passive, rect));

        public static bool DrawButton(Rect rect, IGUStyle style, IGUContent content, bool isFocused, bool noAction) {
            int ID = GUIUtility.GetControlID(FocusType.Passive, rect);
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition);
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
                    return true && !noAction;
                case EventType.Repaint:
                    ((GUIStyle)style).Draw(
                        rect, IGUTextObject.GetGUIContentTemp(content),
                        isHover && !noAction, ID == GUIUtility.hotControl && !noAction,
                        false, isFocused);
                    //if (isRepeat)
                    //    return ID == GUIUtility.hotControl && isHover && !noAction;
                    goto default;
                default:
                    return false;
            }
        }

        public static bool DrawButton(Rect rect, IGUStyle style, IGUContent content, bool noAction)
            => DrawButton(rect, style, content, false, noAction);

        public static bool DrawRepeatButton(Rect rect, IGUStyle style, IGUContent content, out bool onClick, bool isFocused, bool noAction) {
            int ID = GUIUtility.GetControlID(FocusType.Passive, rect);
            Event @event = Event.current;
            bool isHover = rect.Contains(@event.mousePosition);
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
                    return true && !noAction;
                case EventType.Repaint:
                    ((GUIStyle)style).Draw(
                        rect, IGUTextObject.GetGUIContentTemp(content),
                        isHover && !noAction, ID == GUIUtility.hotControl && !noAction,
                        false, isFocused);
                    return ID == GUIUtility.hotControl && isHover && !noAction;
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
            bool isHover = rect.Contains(@event.mousePosition);
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
                        isHover && !noAction, ID == GUIUtility.hotControl && !noAction,
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

            bool isHover = rect.Contains(@event.mousePosition);
            bool isThumbHover = rectThumb.Contains(@event.mousePosition);

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

        public sealed class ButtonClick {
            public bool click;
        }
    }
}
