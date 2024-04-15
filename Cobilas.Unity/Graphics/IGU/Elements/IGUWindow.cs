using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUWindow : IGUTextObject, IIGUClipping {
        public event GUI.WindowFunction windowFunction;
        public const string DefaultIGUWindow = "IGU Window";

        [SerializeField] protected Rect dragFlap;
        [SerializeField] protected bool isClipping;
        [SerializeField] protected IGUStyle windowStyle;
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected IGUScrollViewEvent onMovingWindow;
        [SerializeField] protected WindowFocusStatus windowFocusStatus;

        public IGUScrollViewEvent OnMovingWindow => onMovingWindow;
        public WindowFocusStatus WindowFocusStatus => windowFocusStatus;
        public Rect DragFlap { get => dragFlap; set => dragFlap = value; }
        public IGUStyle WindowStyle { 
            get => windowStyle;
            set => windowStyle = value ?? (IGUStyle)"Black window border";
        }
        public bool IsClipping => isClipping;
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }

        Rect IIGUClipping.RectView { 
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        Vector2 IIGUClipping.ScrollView {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultWindow;
            myColor = IGUColor.DefaultBoxColor;
            onMovingWindow = new IGUScrollViewEvent();
            content = new IGUContent(DefaultIGUWindow);
            windowStyle = (IGUStyle)"Black window border";
            physics = IGUBasicPhysics.Create<IGUCollectionPhysics>(this);
            (physics as IGUCollectionPhysics).SetTriangle(Triangle.Box);
            dragFlap = new Rect(0f, 0f, IGURect.DefaultWindow.Width, 15f);
        }

        protected override void LowCallOnIGU() {

            IGURect myRectTemp = BackEndIGU.SimpleWindow(LocalRect, dragFlap, LocalRect.Position, MyContent,
                    windowStyle, physics, GetInstanceID(), internalIndowFunction, ref windowFocusStatus);

            if (myRectTemp != myRect)
                if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType))
                    onMovingWindow.Invoke((myRect = myRectTemp).Position);
        }

        protected override void DrawTooltip() {
            if (useTooltip && LocalRect.SetSize(dragFlap.size).Contains(IGUDrawer.MousePosition) &&
                LocalConfig.IsVisible && !string.IsNullOrEmpty(ToolTip))
                IGUDrawer.DrawTooltip(ToolTip, tooltipStyle);
        }

        protected virtual void internalIndowFunction(int id, Vector2 scrollView) {
                //GUI.DragWindow(dragFlap);
                isClipping = true;
                windowFunction?.Invoke(id);
                isClipping = false;
        }
    }
}
