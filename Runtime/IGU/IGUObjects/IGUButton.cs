using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUButton : IGUTextObject, ISerializationCallbackReceiver {
        public const string DefaultContentIGUButton = "IGU Button";
        private List<string> stackTraceCount = new List<string>();
        [SerializeField] protected bool[] clicked;
        [SerializeField] protected GUIStyle buttonStyle;
        [SerializeField] protected IGUOnClickEvent onClick;

        public IGUOnClickEvent OnClick => onClick;
        public virtual bool Clicked => GetClicked();
        public GUIStyle ButtonStyle { get => buttonStyle; set => buttonStyle = value; }
        
        public override void OnIGU() {
            if (!myConfg.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = myConfg.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            buttonStyle = GetDefaultValue(buttonStyle, GUI.skin.button);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            Event current = IGUDrawer.IGUEvent;

            if (GUI.Button(rectTemp, GetGUIContent(DefaultContentIGUButton), buttonStyle)) {
                if (IGUDrawer.Drawer.GetMouseButtonUp(myConfg.MouseType)) {
                    onClick.Invoke();
                    clicked[1] = true;
                }
            }

            if (useTooltip)
                if (rectTemp.Contains(current.mousePosition))
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

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
            => IGUDrawer.EventEndOfFrame += Reset;

        public static string PickUpWhereItWasCalled(int skipFrames = 1)
            => new StackTrace(skipFrames).GetFrame(0).GetMethod().Name;

        public static IGUButton CreateIGUInstance(string name, IGUContent content) {
            IGUButton button = Internal_CreateIGUInstance<IGUButton>(name, content);
            button.onClick = new IGUOnClickEvent();
            button.myConfg = IGUConfig.Default;
            button.myRect = IGURect.DefaultButton;
            button.myColor = IGUColor.DefaultBoxColor;
            button.clicked = new bool[2];
            (button as ISerializationCallbackReceiver).OnAfterDeserialize();
            return button;
        }

        public static IGUButton CreateIGUInstance(string name, string text)
            => CreateIGUInstance(name, new IGUContent(text));

        public static IGUButton CreateIGUInstance(string name)
            => CreateIGUInstance(name, DefaultContentIGUButton);
    }
}
