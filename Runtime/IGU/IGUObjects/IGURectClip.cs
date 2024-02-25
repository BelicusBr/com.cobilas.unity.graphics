using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURectClip : IGUObject, IIGUClipping {

        public event Action<Rect> RectClipAction;
        [SerializeField] protected Rect rectView;
        [SerializeField] protected bool isClipping;
        [SerializeField] protected bool autoInvert;
        [SerializeField] protected Vector2 scrollView;

        public bool IsClipping => isClipping;
        public bool AutoInvert { get => autoInvert; set => autoInvert = value; }
        public Rect RectView { get => rectView; set => rectView = value; }
        public Vector2 ScrollView { 
            get => rectView.position = scrollView.Invert(autoInvert, autoInvert);
            set => rectView.position = scrollView = value.Invert(autoInvert, autoInvert);
        }

        protected override void IGUAwake() {
            base.IGUAwake();
            autoInvert = true;
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            rectView = new Rect(Vector2.zero, myRect.Size * 2f);
        }

        protected override void LowCallOnIGU() {
            BackEndIGU.BeginClip(LocalRect, scrollView);
            isClipping = true;
            RectClipAction?.Invoke(RectView);
            isClipping = false;
            BackEndIGU.EndClip();
        }
    }
}
