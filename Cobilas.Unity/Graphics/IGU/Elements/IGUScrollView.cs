using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;
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
        [SerializeField] protected Rect rectView;
        [SerializeField] protected IGURectClip rectClip;
        [SerializeField] protected bool alwaysShowVertical;
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected bool alwaysShowHorizontal;
        [SerializeField] protected IGUScrollViewEvent onScrollView;
        [SerializeField] protected IGUVerticalScrollbar verticalScrollbar;
        [SerializeField] protected IGUHorizontalScrollbar horizontalScrollbar;

        //public bool IsClipping => rectClip.IsClipping;
        public Vector2 RectClipSize => rectClip.MyRect.Size;
        public IGUScrollViewEvent OnScrollView => onScrollView;
        public Rect RectView { get => rectView; set => rectView = value; }
        public bool VerticalScrollbarIsVisible => verticalScrollbar.MyConfig.IsVisible;
        public bool HorizontalScrollbarIsVisible => horizontalScrollbar.MyConfig.IsVisible;
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }
        public Vector2 ScrollView { get => rectView.position; set => rectView.position = value; }
        public bool AlwaysShowVertical { get => alwaysShowVertical; set => alwaysShowVertical = value; }
        public bool AlwaysShowHorizontal { get => alwaysShowHorizontal; set => alwaysShowHorizontal = value; }
        public IGUStyle VerticalScrollbarStyle { get => verticalScrollbar.SliderObjectStyle; set => verticalScrollbar.SliderObjectStyle = value; }
        public IGUStyle HorizontalScrollbarStyle { get => horizontalScrollbar.SliderObjectStyle; set => horizontalScrollbar.SliderObjectStyle = value; }
        public IGUStyle VerticalScrollbarThumbStyle { get => verticalScrollbar.SliderObjectThumbStyle; set => verticalScrollbar.SliderObjectThumbStyle = value; }
        public IGUStyle HorizontalScrollbarThumbStyle { get => horizontalScrollbar.SliderObjectThumbStyle; set => horizontalScrollbar.SliderObjectThumbStyle = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            isPhysicalElement = false;
            rectClip = Create<IGURectClip>($"--[{name}]RectClip");
            verticalScrollbar = Create<IGUVerticalScrollbar>($"--[{name}]VerticalScrollbar");
            horizontalScrollbar = Create<IGUHorizontalScrollbar>($"--[{name}]HorizontalScrollbar");
            physics = IGUBasicPhysics.Create<IGUCollectionPhysics>(this);
            (physics as IGUCollectionPhysics).OnCollision = true;
            myRect = IGURect.DefaultTextArea;
            myColor = IGUColor.DefaultBoxColor;
            onScrollView = new IGUScrollViewEvent();
            rectView = new Rect(0f, 0f, 250f, 250f);
            alwaysShowVertical =
            alwaysShowHorizontal =
            rectClip.RenderOffSet = false;
            rectClip.Parent = verticalScrollbar.Parent = horizontalScrollbar.Parent = this;
            (this as IIGUSerializationCallbackReceiver).Reserialization();
        }

        protected override void LowCallOnIGU() {

            Vector2 vfix = Vector2.zero;
            vfix.x = rectView.height > myRect.Height || alwaysShowHorizontal ? 
                horizontalScrollbar.SliderObjectStyle.FixedHeight : 0f;
            vfix.y = rectView.width > myRect.Width || alwaysShowVertical ?
                verticalScrollbar.SliderObjectStyle.FixedWidth : 0f;

            rectClip.MyRect = rectClip.MyRect.SetSize(myRect.Size - vfix).SetPosition(Vector2Int.zero);
            Vector2 vsize = rectView.size - (rectView.size - (rectClip.MyRect.Size + vfix));
            
            vsize.x = rectView.width < myRect.Width ? rectView.width : vsize.x;
            vsize.y = rectView.height < myRect.Height ? rectView.height : vsize.y;
            verticalScrollbar.MaxMinValue = verticalScrollbar.MaxMinValue.Set(0f, rectView.height);
            horizontalScrollbar.MaxMinValue = horizontalScrollbar.MaxMinValue.Set(0f, rectView.width);

            if (vfix.x != 0f || alwaysShowHorizontal) {
                if (!verticalScrollbar.MyConfig.IsVisible)
                    verticalScrollbar.MyConfig = verticalScrollbar.MyConfig.SetVisible(true);

                verticalScrollbar.MyRect = verticalScrollbar.MyRect.SetSize(vfix.x, rectClip.MyRect.Height)
                    .SetPosition(rectClip.MyRect.Width, 0f);
                verticalScrollbar.ScrollbarThumbSize = vsize.y;
                if (alwaysShowVertical)
                    verticalScrollbar.MyConfig = verticalScrollbar.MyConfig.SetEnabled(rectView.height > myRect.Height);
            } else {
                if (verticalScrollbar.MyConfig.IsVisible)
                    verticalScrollbar.MyConfig = verticalScrollbar.MyConfig.SetVisible(false);
            }
            
            if (vfix.y != 0f || alwaysShowVertical) {
                horizontalScrollbar.MyRect = horizontalScrollbar.MyRect.SetSize(rectClip.MyRect.Width, vfix.y)
                    .SetPosition(0f, rectClip.MyRect.Height);
                horizontalScrollbar.ScrollbarThumbSize = vsize.x;
                if (alwaysShowHorizontal)
                    horizontalScrollbar.MyConfig = horizontalScrollbar.MyConfig.SetEnabled(rectView.width > myRect.Width);
            } else {
                if (horizontalScrollbar.MyConfig.IsVisible)
                    horizontalScrollbar.MyConfig = horizontalScrollbar.MyConfig.SetVisible(false);
            }
            
            verticalScrollbar.OnIGU();
            horizontalScrollbar.OnIGU();

            Vector2 scrollView = Vector2.zero;
            Vector2 scrollPositiontemp = rectView.position;
            if (horizontalScrollbar.MyConfig.IsVisible)
                scrollView.x = horizontalScrollbar.Value;
            if (verticalScrollbar.MyConfig.IsVisible)
                scrollView.y = verticalScrollbar.Value;
            rectView.position = scrollView;
            //rectClip.ScrollView = scrollView;
            rectClip.OnIGU();

            if (scrollPositiontemp != scrollView)
                if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType)) 
                    onScrollView.Invoke(scrollView);
        }

        void IIGUSerializationCallbackReceiver.Reserialization() {
            rectClip.RectClipAction += (r) => {
                ScrollViewAction?.Invoke(this);
            };
        }
    }
}
