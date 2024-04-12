﻿using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUSelectableText : IGUTextFieldObject {

        [SerializeField, HideInInspector] 
        protected bool isFocused;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUStyle selectableTextStyle;

        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
        public IGUStyle SelectableTextStyle { 
            get => selectableTextStyle;
            set => selectableTextStyle = value ?? (IGUStyle)"Black text field border";
        }
        public override IGUBasicPhysics Physics { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultTextArea;
            myColor = IGUColor.DefaultBoxColor;
            selectableTextStyle = (IGUStyle)"Black text field border";
        }

        protected override void LowCallOnIGU() {

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);

            BackEndIGU.SelectableText(LocalRect, MyContent, GetInstanceID(), IGUNonePhysics.None, selectableTextStyle, ref isFocused);
            SetGUISettings(oldSettings);
        }

        protected override void SetGUISettings(GUISettings settings)
            => base.SetGUISettings(settings);

        protected override void SetGUISettings(IGUTextSettings settings)
            => base.SetGUISettings(settings);
    }
}
