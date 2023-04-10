using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [CreateAssetMenu(fileName = "IGUStyle custom", menuName = "IGUSkin/IGUStyleCustom")]
    public class IGUStyleCustom : ScriptableObject {
        [SerializeField] private IGUStyle style;

        public GUIStyle Style => (GUIStyle)style;

        [System.Serializable]
        public sealed class IGUStyle {
            [SerializeField] private string             name;
            [SerializeField] private float              fixedWidth;
            [SerializeField] private float              fixedHeight;
            [SerializeField] private bool               stretchWidth;
            [SerializeField] private bool               stretchHeight;
            [SerializeField] private Font               font;
            [SerializeField] private int                fontSize;
            [SerializeField] private FontStyle          fontStyle;
            [SerializeField] private bool               wordWrap;
            [SerializeField] private bool               richText;
            //[SerializeField] private Vector2            clipOffset;
            [SerializeField] private TextAnchor         alignment;
            [SerializeField] private ImagePosition      imagePosition;
            [SerializeField] private TextClipping       clipping;
            [SerializeField] private Vector2            contentOffset;
            [SerializeField] private IGUStyleStatus     normal;
            [SerializeField] private IGUStyleStatus     hover;
            [SerializeField] private IGUStyleStatus     active;
            [SerializeField] private IGUStyleStatus     focused;
            [SerializeField] private IGUStyleStatus     onNormal;
            [SerializeField] private IGUStyleStatus     onHover;
            [SerializeField] private IGUStyleStatus     onActive;
            [SerializeField] private IGUStyleStatus     onFocused;
            [SerializeField] private IGUStyleRectOffSet border;
            [SerializeField] private IGUStyleRectOffSet margin;
            [SerializeField] private IGUStyleRectOffSet padding;
            [SerializeField] private IGUStyleRectOffSet overflow;

            public IGUStyle() {
                normal = new IGUStyleStatus();
                hover = new IGUStyleStatus();
                active = new IGUStyleStatus();
                focused = new IGUStyleStatus();
                onNormal = new IGUStyleStatus();
                onHover = new IGUStyleStatus();
                onActive = new IGUStyleStatus();
                onFocused = new IGUStyleStatus();
                border = new IGUStyleRectOffSet();
                margin = new IGUStyleRectOffSet();
                padding = new IGUStyleRectOffSet();
                overflow = new IGUStyleRectOffSet();
            }

            public static explicit operator GUIStyle(IGUStyle A)
                => new GUIStyle() {
                    name            = A.name,
                    fixedWidth      = A.fixedWidth,
                    fixedHeight     = A.fixedHeight,
                    stretchWidth    = A.stretchWidth,
                    stretchHeight   = A.stretchHeight,
                    font            = A.font,  
                    fontSize        = A.fontSize,
                    fontStyle       = A.fontStyle,
                    wordWrap        = A.wordWrap,
                    richText        = A.richText,
                    //clipOffset      = A.clipOffset,
                    alignment       = A.alignment,
                    imagePosition   = A.imagePosition,
                    clipping        = A.clipping,
                    contentOffset   = A.contentOffset,
                    normal          = (GUIStyleState)A.normal,
                    hover           = (GUIStyleState)A.hover,
                    active          = (GUIStyleState)A.active,
                    focused         = (GUIStyleState)A.focused,
                    onNormal        = (GUIStyleState)A.onNormal,
                    onHover         = (GUIStyleState)A.onHover,
                    onActive        = (GUIStyleState)A.onActive,
                    onFocused       = (GUIStyleState)A.onFocused,
                    border          = (RectOffset)A.border,
                    margin          = (RectOffset)A.margin,
                    padding         = (RectOffset)A.padding,
                    overflow        = (RectOffset)A.overflow
                };
        }

        [System.Serializable]
        public sealed class IGUStyleStatus {
            [SerializeField]
            private Texture2D background;
            [SerializeField]
            private Color textColor;
            [SerializeField, HideInInspector]
            private bool foldout;
            [SerializeField]
            private Texture2D[] scaledBackgrounds;

            public static explicit operator GUIStyleState(IGUStyleStatus A)
                => new GUIStyleState() {
                    background = A.background,
                    textColor = A.textColor,
                    scaledBackgrounds = A.scaledBackgrounds
                };
        }

        [System.Serializable]
        public sealed class IGUStyleRectOffSet {
            [SerializeField, HideInInspector] 
            private bool foldout;
            //x = left
            //y = right
            [SerializeField] 
            private Vector2Int rectOffSet_xy;
            //x = top
            //y = bottom
            [SerializeField]
            private Vector2Int rectOffSet_zw;

            public static explicit operator RectOffset(IGUStyleRectOffSet A)
                => new RectOffset(
                    A.rectOffSet_xy.x, A.rectOffSet_xy.y,
                    A.rectOffSet_zw.x, A.rectOffSet_zw.y
                    );
        }
    }
}