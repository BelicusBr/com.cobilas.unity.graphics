using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUButton : IGUTextObject, IIGUEndOfFrame {
        public const string DefaultContentIGUButton = "IGU Button";
        private readonly List<string> stackTraceCount = new List<string>();
        [SerializeField] protected bool[] clicked;
        [SerializeField] protected IGUStyle buttonStyle;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUBasicPhysics physics;

        public IGUOnClickEvent OnClick => onClick;
        public virtual bool Clicked => GetClicked();
        public IGUStyle ButtonStyle { 
            get => buttonStyle;
            set => buttonStyle = value ?? (IGUStyle)"Black button border";
        }
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            clicked = new bool[2];
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            myColor = IGUColor.DefaultBoxColor;
            buttonStyle = (IGUStyle)"Black button border";
            content = new IGUContent(DefaultContentIGUButton);
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
        }

        protected override void LowCallOnIGU() {
            buttonStyle.RichText = richText;
            if (BackEndIGU.Button(LocalRect, MyContent, buttonStyle, physics, GetInstanceID(), out bool _))
                if (IGUDrawer.GetMouseButtonUp(LocalConfig.MouseType)) {
                    onClick.Invoke();
                    clicked[1] = true;
                }
        }

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

        void IIGUEndOfFrame.EndOfFrame() => Reset();

        public static string PickUpWhereItWasCalled(int skipFrames = 1)
            => new StackTrace(skipFrames).GetFrame(0).GetMethod().Name;
    }
}
