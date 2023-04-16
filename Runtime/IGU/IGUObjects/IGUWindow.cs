using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUWindow : IGUTextObject, IIGUSerializationCallbackReceiver {
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

        protected override void Awake() {
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultWindow;
            myColor = IGUColor.DefaultBoxColor;
            onMovingWindow = new IGUScrollViewEvent();
            content = new IGUContent(DefaultIGUWindow);
            dragFlap = new Rect(0f, 0f, IGURect.DefaultWindow.Width, 15f);
            (this as IIGUSerializationCallbackReceiver).Reserialization();
        }

        public override void OnIGU() {

            windowStyle = GetDefaultValue(windowStyle, GUI.skin.window);
            GUIContent mycontent = GetGUIContent(DefaultIGUWindow);

            Rect rectTemp = GetRect();
            int ID = GUIUtility.GetControlID(FocusType.Passive);

            Rect rectTemp2 = GUI.Window(ID, rectTemp, internalIndowFunction, mycontent, windowStyle);

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
