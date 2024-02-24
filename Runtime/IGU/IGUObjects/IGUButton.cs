using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUButton : IGUTextObject, IIGUSerializationCallbackReceiver {
        public const string DefaultContentIGUButton = "IGU Button";
        private readonly List<string> stackTraceCount = new List<string>();
        [SerializeField] protected bool[] clicked;
        [SerializeField] protected IGUStyle buttonStyle;
        [SerializeField] protected IGUOnClickEvent onClick;

        public IGUOnClickEvent OnClick => onClick;
        public virtual bool Clicked => GetClicked();
        public IGUStyle ButtonStyle { get => buttonStyle; set => buttonStyle = value; }

        protected override void Ignition() {
            base.Ignition();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultBoxColor;
            buttonStyle = IGUSkins.GetIGUStyle("Black button border");
            content = new IGUContent(DefaultContentIGUButton);
            onClick = new IGUOnClickEvent();
            clicked = new bool[2];
            (this as IIGUSerializationCallbackReceiver).Reserialization();
        }

        protected override void LowCallOnIGU() {

            if (BackEndIGU.Button(LocalRect, MyContent, buttonStyle))
                if (IGUDrawer.Drawer.GetMouseButtonUp(myConfg.MouseType)) {
                    onClick.Invoke();
                    clicked[1] = true;
                }

            if (useTooltip)
                if (LocalRect.ModifiedRect.Contains(IGUDrawer.Drawer.GetMousePosition()))
                    DrawTooltip();
        }

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected virtual void Reset() {
            clicked[0] = clicked[1];
            clicked[1] = false;
            stackTraceCount.Clear();
        }

        private bool GetClicked() {
            string methodName = PickUpWhereItWasCalled(3);
            if (clicked[0])
                if (!stackTraceCount.Contains(methodName)) {
                    stackTraceCount.Add(methodName);
                    return clicked[0];
                }
            return false;
        }

        public static string PickUpWhereItWasCalled(int skipFrames = 1)
            => new StackTrace(skipFrames).GetFrame(0).GetMethod().Name;

        void IIGUSerializationCallbackReceiver.Reserialization()
            => IGUDrawer.EventEndOfFrame += Reset;
    }
}
