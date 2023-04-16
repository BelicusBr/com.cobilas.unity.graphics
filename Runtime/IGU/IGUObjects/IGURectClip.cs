using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURectClip : IGUObject {

        public event Action<Rect> RectClipAction;
        [SerializeField] protected Rect rectView;

        public Rect RectView { get => GetRectView(); set => rectView = AdjustRectview(value); }
        public Vector2 ScrollView { get => rectView.position; set => rectView = AdjustRectview(value); }

        protected override void Awake() {
            base.Awake();
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            rectView = new Rect(Vector2.zero, myRect.Size * 2f);
        }

        protected override void LowCallOnIGU() {
            GUI.BeginClip(GetRect(), rectView.position, Vector2.zero, false);
            doNots = DoNotModifyRect.True;
            RectClipAction?.Invoke(RectView);
            doNots = DoNotModifyRect.False;
            GUI.EndClip();
        }

        private Rect AdjustRectview(Vector2 pos) {
            Rect rect = rectView;
            rect.position = pos;
            return AdjustRectview(rect);
        }

        private Rect GetRectView() {
            rectView.position = -rectView.position;
            return rectView;
        }

        private Rect AdjustRectview(Rect rect) {
            rect.position = -rect.position;
            rect.x = rect.x > 0f ? 0f : rect.x;
            rect.y = rect.y > 0f ? 0f : rect.y;
            Rect rect2 = GetRect();
            float resx = rect.width - rect2.width;
            if (resx < 0) rect.x = 0f;
            else rect.x = Mathf.Abs(rect.x) > resx ? -resx : rect.x;
            float resy = rect.height - rect2.height;
            if (resy < 0) rect.y = 0f;
            else rect.y = Mathf.Abs(rect.y) > resy ? -resy : rect.y;
            return rect;
        }
    }
}
