using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class IGUToolTip {
        private string tooltip;
        private bool close;
        private GUIStyle style;
        private IGUColor color;
        private readonly GUIContent gUIContent;

        public IGUToolTip() {
            color = IGUColor.DefaultBoxColor;
            style = null;
            tooltip = string.Empty;
            gUIContent = new GUIContent();
            close = true;
        }

        public void SetMSM(string txt) => this.tooltip = txt;

        public void Close() => close = true;

        public void Open() => close = false;

        public void SetGuiStyle(GUIStyle style) => this.style = style;

        public void Draw(Vector2 position, Vector2 scaleFactor) {
            if (close) return;
            gUIContent.text = tooltip;
            style = style ?? GUI.skin.box;
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
