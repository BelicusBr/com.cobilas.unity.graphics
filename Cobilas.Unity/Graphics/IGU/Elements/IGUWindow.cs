using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUWindow : IGUTextObject, IIGUWindow, IIGUClippingPhysics {
        public event GUI.WindowFunction windowFunction;
        public const string DefaultIGUWindow = "IGU Window";

        [SerializeField] protected Rect dragFlap;
        [SerializeField] protected bool isClipping;
        [SerializeField] protected IGUStyle windowStyle;
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected IGUScrollViewEvent onMovingWindow;
        [SerializeField] protected WindowFocusStatus windowFocusStatus;
        [SerializeField] protected IGUPhysicsClippingContainer phyContainer;

        public IGUScrollViewEvent OnMovingWindow => onMovingWindow;
        public Rect DragFlap { get => dragFlap; set => dragFlap = value; }
        public IGUStyle WindowStyle { 
            get => windowStyle;
            set => windowStyle = value ?? (IGUStyle)"Black window border";
        }
        public bool IsClipping => isClipping;
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }

        WindowFocusStatus IIGUWindow.IsFocused { get => windowFocusStatus; }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultWindow;
            myColor = IGUColor.DefaultBoxColor;
            onMovingWindow = new IGUScrollViewEvent();
            windowFocusStatus = WindowFocusStatus.None;
            content = new IGUContent(DefaultIGUWindow);
            windowStyle = (IGUStyle)"Black window border";
            phyContainer = new IGUPhysicsClippingContainer();
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
            dragFlap = new Rect(0f, 0f, IGURect.DefaultWindow.Width, 15f);
        }

        protected override void IGUOnEnable() {
            base.IGUOnEnable();
            phyContainer.RefreshEvents();
        }

        protected override void LowCallOnIGU() {
            WindowFocusStatus focusTemp = windowFocusStatus;

            IGURect myRectTemp = BackEndIGU.SimpleWindow(LocalRect, dragFlap, LocalRect.Position, MyContent,
                    windowStyle, physics, GetInstanceID(), internalIndowFunction, ref focusTemp);

            if (windowFocusStatus != (windowFocusStatus = focusTemp))
                IGUCanvasContainer.RefreshEvents();

            if (myRectTemp != myRect)
                if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType))
                    onMovingWindow.Invoke((myRect = myRectTemp).Position);
        }

        public bool AddOtherPhysics(IGUObject obj)
            => phyContainer.AddOtherPhysics(obj);

        public bool RemoveOtherPhysics(IGUObject obj)
            => phyContainer.RemoveOtherPhysics(obj);

        protected override void DrawTooltip() {
            if (useTooltip && LocalRect.SetSize(dragFlap.size).Contains(IGUDrawer.MousePosition) &&
                LocalConfig.IsVisible && !string.IsNullOrEmpty(ToolTip))
                IGUDrawer.DrawTooltip(ToolTip, tooltipStyle);
        }

        protected virtual void internalIndowFunction(int id, Vector2 scrollView) {
                isClipping = true;
                windowFunction?.Invoke(id);
                isClipping = false;
        }

        protected override void InternalCallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys)
            => phyContainer.CallPhysicsFeedback(mouse, ref phys);

        public static IGUWindow operator +(IGUWindow A, IIGUPhysics B) {
            _ = A.AddOtherPhysics(B.Physics.Target);
            return A;
        }

        public static IGUWindow operator -(IGUWindow A, IIGUPhysics B) {
            _ = A.RemoveOtherPhysics(B.Physics.Target);
            return A;
        }
    }
}
