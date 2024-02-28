using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUWindow : IGUTextObject, IIGUClipping, IIGUSerializationCallbackReceiver {
        public event GUI.WindowFunction windowFunction;
        public const string DefaultIGUWindow = "IGU Window";

        [SerializeField] protected Rect dragFlap;
        [SerializeField] protected bool isClipping;
        [SerializeField] protected IGUStyle windowStyle;
        protected GUI.WindowFunction internalIndowFunction;
        [SerializeField] protected IGUScrollViewEvent onMovingWindow;

        public IGUScrollViewEvent OnMovingWindow => onMovingWindow;
        /// <summary>O <see cref="Rect"/> da aba de arrasto da janela.</summary>
        public Rect DragFlap { get => dragFlap; set => dragFlap = value; }
        public IGUStyle WindowStyle { 
            get => windowStyle;
            set => windowStyle = value ?? (IGUStyle)"Black window border";
        }
        public bool IsClipping => isClipping;

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
            dragFlap = new Rect(0f, 0f, IGURect.DefaultWindow.Width, 15f);
            (this as IIGUSerializationCallbackReceiver).Reserialization();
        }

        protected override void LowCallOnIGU() {

            IGURect myRectTemp = BackEndIGU.Window(GUIUtility.GetControlID(FocusType.Passive, (Rect)LocalRect.ModifiedRect), LocalRect,
                    internalIndowFunction, MyContent, windowStyle);

            if (myRectTemp != myRect)
                if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType))
                    onMovingWindow.Invoke((myRect = myRectTemp).Position);
        }

        protected override void DrawTooltip() {
            if (useTooltip && LocalRect.SetSize(dragFlap.size).Contains(IGUDrawer.MousePosition))
                IGUDrawer.DrawTooltip(ToolTip, tooltipStyle);
        }

        void IIGUSerializationCallbackReceiver.Reserialization() {
            internalIndowFunction = (id) => {
                GUI.DragWindow(dragFlap);
                isClipping = true;
                windowFunction?.Invoke(id);
                isClipping = false;
            };
        }
    }
}
