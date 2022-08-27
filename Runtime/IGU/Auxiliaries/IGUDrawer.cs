using System;
using UnityEngine;
using System.Collections;
using Cobilas.Collections;
using Cobilas.Unity.Management.Container;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    [AddToPermanentContainer]
    public class IGUDrawer : IGUBehaviour, ISerializationCallbackReceiver {
        private Action onIGU;
        private Coroutine EndOfFrameCoroutine = null;
        private IGUToolTip toolTip = new IGUToolTip();
        [SerializeField] private IGUMouseInput[] mouses;
        [SerializeField] private IGUContainer[] containers;
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private Vector2 editor_ScaleFactor;
        [SerializeField, HideInInspector] private Vector2Int editor_CurrentResolution;

#endif

        private static Event iGUEvent;
        private static IGUDrawer drawer;
        public static Event IGUEvent => iGUEvent;
        public static Vector2Int BaseResolution => new Vector2Int(1024, 768);
        public static Vector2Int CurrentResolution => new Vector2Int(Screen.width, Screen.height);
        public static Vector2 ScaleFactor => ((Vector2)CurrentResolution).Division(BaseResolution);

        public static IGUDrawer Drawer => drawer;
        public static event Action EventEndOfFrame = (Action)null;

        protected override void Awake() {
            drawer = this;
            mouses = new IGUMouseInput[3];
            iGUEvent = new Event();
        }

        private void LateUpdate() {
            if (EndOfFrameCoroutine == null)
                EndOfFrameCoroutine = StartCoroutine(EndOfFrame());
#if UNITY_EDITOR
            editor_CurrentResolution = CurrentResolution;
            editor_ScaleFactor = ScaleFactor;
#endif
        }

        private IEnumerator EndOfFrame() {
            while (true) {
                yield return new WaitForEndOfFrame();
                UseMouseDown();
                ResetMouseInput();
                EventEndOfFrame?.Invoke();
            }
        }

        protected override void OnGUI() {
            Event.PopEvent(iGUEvent);
            switch (iGUEvent.type) {
                case EventType.MouseDown:
                    if (iGUEvent.button < 3)
                        mouses[iGUEvent.button] = IGUMouseInput.MouseDown;
                    break;
                case EventType.MouseUp:
                    if (iGUEvent.button < 3)
                        mouses[iGUEvent.button] = IGUMouseInput.MouseUp;
                    break;
            }
            toolTip.Close();
            onIGU?.Invoke();
            toolTip.SetScaleFactor(ScaleFactor);
            toolTip.Draw();
        }

        public void OpenTooltip() => toolTip.Open();

        public void GUIStyleTootip(GUIStyle style) => toolTip.SetGuiStyle(style);

        public void SetTootipText(string txt) => toolTip.SetMSM(txt);

        public void SetTootipPosition(Vector2 pos) => toolTip.SetPosition(pos);

        public bool GetMouseButton(MouseButtonType type) {
            if (type == MouseButtonType.All) return true;
            return mouses[(int)type].Press;
        }

        public bool GetMouseButtonDown(MouseButtonType type) {
            if (type == MouseButtonType.All) return true;
            return mouses[(int)type].Down;
        }

        public bool GetMouseButtonUp(MouseButtonType type) {
            if (type == MouseButtonType.All) return true;
            return mouses[(int)type].Up;
        }

        public bool Contains(IGUContainer container) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(containers); I++)
                if (containers[I] == container)
                    return true;
            return false;
        }

        private void UseMouseDown() {
            if (mouses[0].Down) mouses[0] = IGUMouseInput.UseMouseDown;
            if (mouses[1].Down) mouses[1] = IGUMouseInput.UseMouseDown;
            if (mouses[2].Down) mouses[2] = IGUMouseInput.UseMouseDown;
        }

        private void ResetMouseInput() {
            if (mouses[0].Up) mouses[0] = IGUMouseInput.MouseNone;
            if (mouses[1].Up) mouses[1] = IGUMouseInput.MouseNone;
            if (mouses[2].Up) mouses[2] = IGUMouseInput.MouseNone;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            drawer = this;
            iGUEvent = new Event();
            for (int I = 0; I < ArrayManipulation.ArrayLength(containers); I++)
                onIGU += (containers[I] as IIGUContainer).OnIGU;
        }

        internal void Add(IGUContainer container) {
            if (Contains(container)) return;
            onIGU += (container as IIGUContainer).OnIGU;
            ArrayManipulation.Add(container, ref containers);
        }

        internal void Remove(IGUContainer container) {
            if (!Contains(container)) return;
            onIGU -= (container as IIGUContainer).OnIGU;
            ArrayManipulation.Remove(container, ref containers);
        }

        private sealed class IGUToolTip {
            private string tooltip;
            private GUIContent gUIContent;
            private bool close;
            private Vector2 position;
            private Vector2 scaleFactor;
            private GUIStyle style;
            private IGUColor color;

            public IGUToolTip() {
                color = IGUColor.DefaultBoxColor;
                tooltip = string.Empty;
                gUIContent = new GUIContent();
                close = true;
                position = scaleFactor = Vector2.zero;
            }

            public void SetMSM(string txt) => this.tooltip = txt;

            public void Close() => close = true;

            public void Open() => close = false;

            public void SetPosition(Vector2 pos) => this.position = pos;

            public void SetScaleFactor(Vector2 sf) => this.scaleFactor = sf;

            public void SetGuiStyle(GUIStyle style) => this.style = style;

            public void Draw() {
                if (close) return;
                gUIContent.text = tooltip;
                style = style == null ? GUI.skin.box : style;
                Vector2 size = style.CalcSize(gUIContent);
                Matrix4x4 oldMatrix = GUI.matrix;
                GUI.color = color.MyColor;
                GUI.contentColor = color.TextColor;
                GUI.backgroundColor = color.BackgroundColor;
                GUIUtility.RotateAroundPivot(0, position);
                GUIUtility.ScaleAroundPivot(scaleFactor, position);
                GUI.Box(new Rect(position + Vector2.right * 15f, size), gUIContent, style);
                GUI.matrix = oldMatrix;
            }
        }
    }
}
