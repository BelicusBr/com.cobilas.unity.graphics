using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUWindow : IGUTextObject, ISerializationCallbackReceiver {
        public event GUI.WindowFunction windowFunction;
        public const string DefaultIGUWindow = "IGU Window";

        [SerializeField] protected Rect dragFlap;
        [SerializeField] protected GUIStyle windowStyle;
        protected GUI.WindowFunction internalIndowFunction;
        [SerializeField] protected IGUScrollViewEvent onMovingWindow;

        public IGUScrollViewEvent OnMovingWindow => onMovingWindow;
        /// <summary>O <see cref="Rect"/> da aba de arrasto da janela.</summary>
        public Rect DragFlap { get => dragFlap; set => dragFlap = value; }
        public GUIStyle WindowStyle { get => windowStyle; set => windowStyle = value; }

        public override void OnIGU() {
            if (!myConfg.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = myConfg.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            windowStyle = GetDefaultValue(windowStyle, GUI.skin.window);
            GUIContent mycontent = GetGUIContent(DefaultIGUWindow);

            Rect rectTemp = new Rect(GetPosition(), myRect.Size);
            int ID = GUIUtility.GetControlID(FocusType.Passive);

            Rect rectTemp2 = GUI.Window(ID, rectTemp, internalIndowFunction, mycontent, windowStyle);

            if (rectTemp != rectTemp2) {
                onMovingWindow.Invoke(rectTemp2.position);
                _ = myRect.SetModifiedPosition(rectTemp2.position);
                _ = myRect.SetSize(rectTemp2.size);
            }

            if (useTooltip)
                if (dragFlap.Contains(Event.current.mousePosition))
                    DrawTooltip();
        }

        private void InitInternalIndowFunction() {
            internalIndowFunction = (id) => {
                windowFunction?.Invoke(id);
                GUI.DragWindow(dragFlap);
            };
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
            => InitInternalIndowFunction();

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        public static IGUWindow CreateIGUInstance(string name, Rect dragFlap, IGUContent content) {
            IGUWindow window = Internal_CreateIGUInstance<IGUWindow>(name);
            window.content = content;
            window.dragFlap = dragFlap;
            window.myConfg = IGUConfig.Default;
            window.myRect = IGURect.DefaultWindow;
            window.myColor = IGUColor.DefaultBoxColor;
            window.onMovingWindow = new IGUScrollViewEvent();
            window.InitInternalIndowFunction();
            return window;
        }

        public static IGUWindow CreateIGUInstance(string name, IGUContent content)
            => CreateIGUInstance(name, new Rect(0, 0, IGURect.DefaultWindow.Width, 15f), content);

        public static IGUWindow CreateIGUInstance(string name, Rect dragFlap, string text)
            => CreateIGUInstance(name, dragFlap, new IGUContent(text));

        public static IGUWindow CreateIGUInstance(string name, string text)
            => CreateIGUInstance(name, new IGUContent(text));

        public static IGUWindow CreateIGUInstance(string name)
            => CreateIGUInstance(name, DefaultIGUWindow);
    }
}
