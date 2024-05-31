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
    public class IGUScrollView : IGUObject, IIGUScrollView, IIGUClippingPhysics {

        public event Action<IGUScrollView> ScrollViewAction;
        [SerializeField] protected Rect rectView;
        [SerializeField] protected IGURectClip rectClip;
        [SerializeField] protected bool alwaysShowVertical;
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected bool alwaysShowHorizontal;
        [SerializeField] protected bool useFullScrollbarSize;
        [SerializeField] protected IGUScrollViewEvent onScrollView;
        [SerializeField] protected IGUVerticalScrollbar verticalScrollbar;
        [SerializeField] protected IGUHorizontalScrollbar horizontalScrollbar;

        public Vector2 RectClipSize => rectClip.MyRect.Size;
        public IGUScrollViewEvent OnScrollView => onScrollView;
        public Rect RectView { get => rectView; set => rectView = value; }
        public bool VerticalScrollbarIsVisible => verticalScrollbar.MyConfig.IsVisible;
        public bool HorizontalScrollbarIsVisible => horizontalScrollbar.MyConfig.IsVisible;
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }
        public Vector2 ScrollView { get => rectView.position; set => rectView.position = value; }
        public bool AutoInvert { get => rectClip.AutoInvert; set => rectClip.AutoInvert = value; }
        public bool RenderOffSet { get => rectClip.RenderOffSet; set => rectClip.RenderOffSet = value; }
        public bool AlwaysShowVertical { get => alwaysShowVertical; set => alwaysShowVertical = value; }
        public bool UseFullScrollbarSize { get => useFullScrollbarSize; set => useFullScrollbarSize = value; }
        public bool AlwaysShowHorizontal { get => alwaysShowHorizontal; set => alwaysShowHorizontal = value; }
        public IGUStyle VerticalScrollbarStyle { get => verticalScrollbar.SliderObjectStyle; set => verticalScrollbar.SliderObjectStyle = value; }
        public IGUStyle HorizontalScrollbarStyle { get => horizontalScrollbar.SliderObjectStyle; set => horizontalScrollbar.SliderObjectStyle = value; }
        public IGUStyle VerticalScrollbarThumbStyle { get => verticalScrollbar.SliderObjectThumbStyle; set => verticalScrollbar.SliderObjectThumbStyle = value; }
        public IGUStyle HorizontalScrollbarThumbStyle { get => horizontalScrollbar.SliderObjectThumbStyle; set => horizontalScrollbar.SliderObjectThumbStyle = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            rectClip = Create<IGURectClip>($"--[{name}]RectClip");
            verticalScrollbar = Create<IGUVerticalScrollbar>($"--[{name}]VerticalScrollbar");
            horizontalScrollbar = Create<IGUHorizontalScrollbar>($"--[{name}]HorizontalScrollbar");
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
            myRect = IGURect.DefaultTextArea;
            myColor = IGUColor.DefaultBoxColor;
            onScrollView = new IGUScrollViewEvent();
            rectView = new Rect(0f, 0f, 250f, 250f);
            alwaysShowVertical =
            alwaysShowHorizontal = false;
            rectClip.AutoInvert =
            rectClip.RenderOffSet = true;
            rectClip.Parent = verticalScrollbar.Parent = horizontalScrollbar.Parent = this;
        }

        protected override void IGUOnEnable() {
            rectClip.RectClipAction += (r) => {
                ScrollViewAction?.Invoke(this);
            };
        }

        protected override void LowCallOnIGU() {

            Vector2 vfix = Vector2.zero;
            vfix.x = rectView.height > myRect.Height || alwaysShowHorizontal ? 
                horizontalScrollbar.SliderObjectStyle.FixedHeight : 0f;
            vfix.y = rectView.width > myRect.Width || alwaysShowVertical ?
                verticalScrollbar.SliderObjectStyle.FixedWidth : 0f;

            rectClip.MyRect = rectClip.MyRect.SetSize(myRect.Size - vfix).SetPosition(Vector2Int.zero);
            Vector2 vsize = (rectView.size - (rectView.size - rectClip.MyRect.Size)).ABS();
            
            vsize.x = rectView.width < myRect.Width ? rectView.width : vsize.x;
            vsize.y = rectView.height < myRect.Height ? rectView.height : vsize.y;
            verticalScrollbar.MaxMinValue = verticalScrollbar.MaxMinValue.Set(0f, rectView.height);
            horizontalScrollbar.MaxMinValue = horizontalScrollbar.MaxMinValue.Set(0f, rectView.width);

            verticalScrollbar.MyConfig = verticalScrollbar.MyConfig.SetVisible(vfix.x != 0f || alwaysShowHorizontal);
            horizontalScrollbar.MyConfig = horizontalScrollbar.MyConfig.SetVisible(vfix.y != 0f || alwaysShowVertical);

            verticalScrollbar.MyRect = verticalScrollbar.MyRect.SetSize(vfix.x, rectClip.MyRect.Height)
                .SetPosition(rectClip.MyRect.Width, 0f);
            verticalScrollbar.ScrollbarThumbSize = vsize.y;
            if (alwaysShowVertical)
                verticalScrollbar.MyConfig = verticalScrollbar.MyConfig.SetEnabled(rectView.height > myRect.Height);
            
            horizontalScrollbar.MyRect = horizontalScrollbar.MyRect.SetSize(rectClip.MyRect.Width, vfix.y)
                .SetPosition(0f, rectClip.MyRect.Height);
            horizontalScrollbar.ScrollbarThumbSize = vsize.x;
            if (alwaysShowHorizontal)
                horizontalScrollbar.MyConfig = horizontalScrollbar.MyConfig.SetEnabled(rectView.width > myRect.Width);
            
            verticalScrollbar.OnIGU();
            horizontalScrollbar.OnIGU();

            Vector2 scrollView = Vector2.zero;
            Vector2 scrollPositiontemp = rectView.position;
            if (horizontalScrollbar.MyConfig.IsVisible)
                scrollView.x = useFullScrollbarSize ? horizontalScrollbar.MaxMinValue.Max * (horizontalScrollbar.Value / (horizontalScrollbar.MaxMinValue.Max - vsize.x)) :
                    horizontalScrollbar.Value;
            if (verticalScrollbar.MyConfig.IsVisible)
                scrollView.y = useFullScrollbarSize ? verticalScrollbar.MaxMinValue.Max * (verticalScrollbar.Value / (verticalScrollbar.MaxMinValue.Max - vsize.y)) : 
                    verticalScrollbar.Value;
            rectView.position = scrollView;
            rectClip.ScrollView = scrollView;
            rectClip.OnIGU();

            if (scrollPositiontemp != scrollView)
                if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType)) 
                    onScrollView.Invoke(scrollView);
        }

        protected override void InternalCallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys) {
            (verticalScrollbar as IIGUPhysics).CallPhysicsFeedback(mouse, ref phys);
            (horizontalScrollbar as IIGUPhysics).CallPhysicsFeedback(mouse, ref phys);
            (rectClip as IIGUPhysics).CallPhysicsFeedback(mouse, ref phys);
        }

        public bool AddOtherPhysics(IGUObject obj)
            => rectClip.AddOtherPhysics(obj);

        public bool RemoveOtherPhysics(IGUObject obj)
            => rectClip.RemoveOtherPhysics(obj);

        public static IGUScrollView operator +(IGUScrollView A, IIGUPhysics B) {
            _ = A.AddOtherPhysics(B.Physics.Target);
            return A;
        }

        public static IGUScrollView operator -(IGUScrollView A, IIGUPhysics B) {
            _ = A.RemoveOtherPhysics(B.Physics.Target);
            return A;
        }
    }
}
