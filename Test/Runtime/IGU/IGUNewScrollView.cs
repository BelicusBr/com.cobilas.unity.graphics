using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUNewScrollView : IGUObject, IIGUSerializationCallbackReceiver {

        public IGURectClip rectClip;
        public IGUVerticalScrollbar verticalScrollbar;
        public IGUHorizontalScrollbar horizontalScrollbar;

        protected override void Awake() {
            base.Awake();
            rectClip = CreateIGUInstance<IGURectClip>($"--[{name}]RectClip");
            verticalScrollbar = CreateIGUInstance<IGUVerticalScrollbar>($"--[{name}]VerticalScrollbar");
            horizontalScrollbar = CreateIGUInstance<IGUHorizontalScrollbar>($"--[{name}]HorizontalScrollbar");

            myRect = IGURect.DefaultBox.SetSize(Vector2.one * 150f);
            rectClip.RectView = new Rect(Vector2.zero, Vector2.one * 300f);

            rectClip.Parent = verticalScrollbar.Parent = horizontalScrollbar.Parent = this;

            (this as IIGUSerializationCallbackReceiver).Reserialization();
        }

        protected override void LowCallOnIGU() {

            Vector2 vfix = Vector2.zero;
            vfix.x = rectClip.RectView.width > myRect.Width ? verticalScrollbar.SliderObjectStyle.FixedWidth : 0f;
            vfix.y = rectClip.RectView.height > myRect.Height ? horizontalScrollbar.SliderObjectStyle.FixedHeight : 0f;

            rectClip.MyRect = rectClip.MyRect.SetSize(myRect.Size - vfix).SetPosition(Vector2Int.zero);
            verticalScrollbar.MyRect = verticalScrollbar.MyRect.SetSize(15f, rectClip.MyRect.Height)
                .SetPosition(rectClip.MyRect.Width, 0f);
            horizontalScrollbar.MyRect = horizontalScrollbar.MyRect.SetSize(rectClip.MyRect.Width, 15f)
                .SetPosition(0f, rectClip.MyRect.Height);

            verticalScrollbar.MaxMinValue = verticalScrollbar.MaxMinValue.Set(0f, rectClip.RectView.height);
            horizontalScrollbar.MaxMinValue = horizontalScrollbar.MaxMinValue.Set(0f, rectClip.RectView.width);

            Vector2 vsize = rectClip.RectView.size - (rectClip.MyRect.Size + vfix * 2f);
            Vector2 vclip = vsize * .1f;

            vsize.x = vsize.x < vclip.x ? vclip.x : vsize.x;
            vsize.y = vsize.y < vclip.y ? vclip.y : vsize.y;

            verticalScrollbar.ScrollbarThumbSize = vsize.x;
            horizontalScrollbar.ScrollbarThumbSize = vsize.y;

            verticalScrollbar.OnIGU();
            horizontalScrollbar.OnIGU();

            Vector2 scrollView = Vector2.zero;
            scrollView.x = horizontalScrollbar.Value;
            scrollView.y = verticalScrollbar.Value;
            rectClip.ScrollView = scrollView;
            rectClip.OnIGU();
        }

        void IIGUSerializationCallbackReceiver.Reserialization() {
            rectClip.RectClipAction += (r) => {
                GUI.Box(new Rect(Vector2.zero, Vector2.one * 300f), GUIContent.none);
            };
        }
    }
}
