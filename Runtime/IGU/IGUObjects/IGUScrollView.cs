using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    /* carácteres de escape
     * (<) => &lt;
     * (>) => &gt;
     * (&) => &amp;
     * (") => &quot;
     * (') => &apos;
     */
    public class IGUScrollView : IGUObject, IIGUSerializationCallbackReceiver {

        public event Action<IGUScrollView> ScrollViewAction;
        [SerializeField] protected IGURectClip rectClip;
        [SerializeField] protected bool alwaysShowVertical;
        [SerializeField] protected bool alwaysShowHorizontal;
        [SerializeField] protected IGUScrollViewEvent onScrollView;
        [SerializeField] protected IGUVerticalScrollbar verticalScrollbar;
        [SerializeField] protected IGUHorizontalScrollbar horizontalScrollbar;

        public Vector2 RectClipSize => rectClip.MyRect.Size;
        public IGUScrollViewEvent OnScrollView => onScrollView;
        public Rect ViewRect { get => rectClip.RectView; set => rectClip.RectView = value; }
        public bool AlwaysShowVertical { get => alwaysShowVertical; set => alwaysShowVertical = value; }
        public Vector2 ScrollPosition { get => rectClip.ScrollView; set => rectClip.ScrollView = value; }
        public bool AlwaysShowHorizontal { get => alwaysShowHorizontal; set => alwaysShowHorizontal = value; }
        public IGUStyle VerticalScrollbarStyle { get => verticalScrollbar.SliderObjectStyle; set => verticalScrollbar.SliderObjectStyle = value; }
        public IGUStyle HorizontalScrollbarStyle { get => horizontalScrollbar.SliderObjectStyle; set => horizontalScrollbar.SliderObjectStyle = value; }
        public IGUStyle VerticalScrollbarThumbStyle { get => verticalScrollbar.SliderObjectThumbStyle; set => verticalScrollbar.SliderObjectThumbStyle = value; }
        public IGUStyle HorizontalScrollbarThumbStyle { get => horizontalScrollbar.SliderObjectThumbStyle; set => horizontalScrollbar.SliderObjectThumbStyle = value; }

        protected override void Awake() {
            base.Awake();
            rectClip = CreateIGUInstance<IGURectClip>($"--[{name}]RectClip");
            verticalScrollbar = CreateIGUInstance<IGUVerticalScrollbar>($"--[{name}]VerticalScrollbar");
            horizontalScrollbar = CreateIGUInstance<IGUHorizontalScrollbar>($"--[{name}]HorizontalScrollbar");
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultTextArea;
            myColor = IGUColor.DefaultBoxColor;
            onScrollView = new IGUScrollViewEvent();
            rectClip.RectView = new Rect(0f, 0f, 250f, 250f);
            alwaysShowVertical =
            alwaysShowHorizontal = false;
            rectClip.Parent = verticalScrollbar.Parent = horizontalScrollbar.Parent = this;
            (this as IIGUSerializationCallbackReceiver).Reserialization();
        }

        protected override void LowCallOnIGU() {

            Vector2 vfix = Vector2.zero;
            //horizontalScrollbar
            vfix.x = rectClip.RectView.height > myRect.Height || alwaysShowHorizontal ? 
                horizontalScrollbar.SliderObjectStyle.FixedHeight : 0f;
            vfix.y = rectClip.RectView.width > myRect.Width || alwaysShowVertical ?
                verticalScrollbar.SliderObjectStyle.FixedWidth : 0f;

            rectClip.MyRect = rectClip.MyRect.SetSize(myRect.Size - vfix).SetPosition(Vector2Int.zero);
            Vector2 vsize = rectClip.RectView.size - (rectClip.RectView.size - (rectClip.MyRect.Size + vfix));
            
            vsize.x = rectClip.RectView.width < myRect.Width ? rectClip.RectView.width : vsize.x;
            vsize.y = rectClip.RectView.height < myRect.Height ? rectClip.RectView.height : vsize.y;

            if (vfix.x != 0f) {
                verticalScrollbar.MyRect = verticalScrollbar.MyRect.SetSize(15f, rectClip.MyRect.Height)
                    .SetPosition(rectClip.MyRect.Width, 0f);
                verticalScrollbar.MaxMinValue = verticalScrollbar.MaxMinValue.Set(0f, rectClip.RectView.height);
                verticalScrollbar.ScrollbarThumbSize = vsize.y;
                if (alwaysShowVertical)
                    verticalScrollbar.MyConfg = verticalScrollbar.MyConfg.SetEnabled(rectClip.RectView.height > myRect.Height);
                verticalScrollbar.OnIGU();
            }
            if (vfix.y != 0f) {
                horizontalScrollbar.MyRect = horizontalScrollbar.MyRect.SetSize(rectClip.MyRect.Width, 15f)
                    .SetPosition(0f, rectClip.MyRect.Height);
                horizontalScrollbar.MaxMinValue = horizontalScrollbar.MaxMinValue.Set(0f, rectClip.RectView.width);
                horizontalScrollbar.ScrollbarThumbSize = vsize.x;
                if (alwaysShowHorizontal)
                    horizontalScrollbar.MyConfg = horizontalScrollbar.MyConfg.SetEnabled(rectClip.RectView.width > myRect.Width);
                horizontalScrollbar.OnIGU();
            }

            Vector2 scrollView = Vector2.zero;
            Vector2 scrollPositiontemp = rectClip.ScrollView;
            scrollView.x = horizontalScrollbar.Value;
            scrollView.y = verticalScrollbar.Value;
            rectClip.ScrollView = scrollView;
            rectClip.OnIGU();

            if (scrollPositiontemp != scrollView)
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) 
                    onScrollView.Invoke(scrollView);
        }

        void IIGUSerializationCallbackReceiver.Reserialization() {
            rectClip.RectClipAction += (r) => ScrollViewAction?.Invoke(this);
        }
    }
}
