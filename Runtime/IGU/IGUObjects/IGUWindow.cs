﻿using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUWindow : IGUTextObject, IIGUClipping, IIGUSerializationCallbackReceiver {
        public event GUI.WindowFunction windowFunction;
        public const string DefaultIGUWindow = "IGU Window";

        [SerializeField] protected Rect dragFlap;
        [SerializeField] protected IGUStyle windowStyle;
        protected GUI.WindowFunction internalIndowFunction;
        [SerializeField] protected IGUScrollViewEvent onMovingWindow;

        public IGUScrollViewEvent OnMovingWindow => onMovingWindow;
        /// <summary>O <see cref="Rect"/> da aba de arrasto da janela.</summary>
        public Rect DragFlap { get => dragFlap; set => dragFlap = value; }
        public IGUStyle WindowStyle { get => windowStyle; set => windowStyle = value; }

        Rect IIGUClipping.RectView { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        protected override void Awake() {
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultWindow;
            myColor = IGUColor.DefaultBoxColor;
            onMovingWindow = new IGUScrollViewEvent();
            content = new IGUContent(DefaultIGUWindow);
            windowStyle = IGUSkins.GetIGUStyle("Black window border");
            dragFlap = new Rect(0f, 0f, IGURect.DefaultWindow.Width, 15f);
            (this as IIGUSerializationCallbackReceiver).Reserialization();
        }

        protected override void LowCallOnIGU() {

            GUIContent mycontent = GetGUIContent(DefaultIGUWindow);

            Rect rectTemp = GetRect();

            Rect rectTemp2 = GUI.Window(GUIUtility.GetControlID(FocusType.Passive, rectTemp), rectTemp,
                internalIndowFunction, mycontent, IGUStyle.GetGUIStyleTemp(windowStyle));

            if (rectTemp != rectTemp2) {
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                    onMovingWindow.Invoke(rectTemp2.position);
                    _ = myRect.SetModifiedPosition(rectTemp2.position);
                    _ = myRect.SetSize(rectTemp2.size);
                }
            }

            if (useTooltip) {
                rectTemp = GetRect(true);
                rectTemp.size = dragFlap.size;
                if (rectTemp.Contains(IGUDrawer.Drawer.GetMousePosition()))
                    DrawTooltip();
            }
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        void IIGUSerializationCallbackReceiver.Reserialization() {
            internalIndowFunction = (id) => {
                GUI.DragWindow(dragFlap);
                doNots = DoNotModifyRect.True;
                windowFunction?.Invoke(id);
                doNots = DoNotModifyRect.False;
            };
        }
    }
}
