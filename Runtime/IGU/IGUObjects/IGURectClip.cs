﻿using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURectClip : IGUObject, IIGUClipping {

        public event Action<Vector2> RectClipAction;
        [SerializeField] protected bool isClipping;
        [SerializeField] protected bool autoInvert;
        [SerializeField] protected Vector2 scrollView;

        public bool IsClipping => isClipping;
        public bool AutoInvert { get => autoInvert; set => autoInvert = value; }
        public Vector2 ScrollView { 
            get => scrollView.Invert(autoInvert, autoInvert);
            set => scrollView = value.Invert(autoInvert, autoInvert);
        }

        Rect IIGUClipping.RectView { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override void Ignition() {
            base.Ignition();
            autoInvert = true;
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
        }

        protected override void LowCallOnIGU() {
            GUI.BeginClip(GetRect(), scrollView, Vector2.zero, false);
            isClipping = true;
            RectClipAction?.Invoke(scrollView);
            isClipping = false;
            GUI.EndClip();
        }

        // private Rect AdjustRectview(Vector2 pos) {
        //     Rect rect = rectView;
        //     rect.position = pos;
        //     return AdjustRectview(rect);
        // }

        // private Rect GetRectView() {
        //     rectView.position = -rectView.position;
        //     return rectView;
        // }

        // private Rect AdjustRectview(Rect rect) {
        //     rect.position = -rect.position;
        //     rect.x = rect.x > 0f ? 0f : rect.x;
        //     rect.y = rect.y > 0f ? 0f : rect.y;
        //     Rect rect2 = GetRect();
        //     float resx = rect.width - rect2.width;
        //     if (resx < 0) rect.x = 0f;
        //     else rect.x = Mathf.Abs(rect.x) > resx ? -resx : rect.x;
        //     float resy = rect.height - rect2.height;
        //     if (resy < 0) rect.y = 0f;
        //     else rect.y = Mathf.Abs(rect.y) > resy ? -resy : rect.y;
        //     return rect;
        // }
    }
}
