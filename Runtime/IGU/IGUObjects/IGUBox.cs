﻿using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUBox : IGUTextObject {
        public const string DefaultIGUBox = "IGU Box";
        [SerializeField] private GUIStyle boxStyle;

        public GUIStyle BoxStyle { get => boxStyle; set => boxStyle = value; }

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            content = new IGUContent(DefaultIGUBox);
        }

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();

            if (!config.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = config.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            boxStyle = GetDefaultValue(boxStyle, GUI.skin.box);
            GUIContent mycontent = GetGUIContent(DefaultIGUBox);

            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            GUI.Box(rectTemp, mycontent, boxStyle);

            if (useTooltip)
                if (rectTemp.Contains(Event.current.mousePosition))
                    DrawTooltip();
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);
    }
}
