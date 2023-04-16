using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    /*
     *  carácteres de escape
     *  (<) => &lt;
     *  (>) => &gt;
     *  (&) => &amp;
     *  (") => &quot;
     *  (') => &apos;
     */
    public class IGUScrollView : IGUObject {

        public event Action<IGUScrollView> ScrollViewAction;
        protected Vector2 scrollPosition;
        [SerializeField] protected Rect viewRect;
        [SerializeField] protected bool alwaysShowVertical;
        [SerializeField] protected bool alwaysShowHorizontal;
        [SerializeField] protected GUIStyle verticalScrollbarStyle;
        [SerializeField] protected GUIStyle horizontalScrollbarStyle;
        [SerializeField] protected IGUScrollViewEvent onScrollView;

        public IGUScrollViewEvent OnScrollView => onScrollView;
        public Rect ViewRect { get => viewRect; set => viewRect = value; }
        public Vector2 ScrollPosition { get => scrollPosition; set => scrollPosition = value; }
        public bool AlwaysShowVertical { get => alwaysShowVertical; set => alwaysShowVertical = value; }
        public GUIStyle VerticalScrollbarStyle { get => verticalScrollbarStyle; set => verticalScrollbarStyle = value; }
        public bool AlwaysShowHorizontal { get => alwaysShowHorizontal; set => alwaysShowHorizontal = value; }
        public GUIStyle HorizontalScrollbarStyle { get => horizontalScrollbarStyle; set => horizontalScrollbarStyle = value; }

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultTextArea;
            myColor = IGUColor.DefaultBoxColor;
            onScrollView = new IGUScrollViewEvent();
            viewRect = new Rect(0, 0, 250f, 250f);
            alwaysShowVertical =
            alwaysShowHorizontal = false;
        }

        public override void OnIGU() {

            verticalScrollbarStyle = GetDefaultValue(verticalScrollbarStyle, GUI.skin.verticalScrollbar);
            horizontalScrollbarStyle = GetDefaultValue(horizontalScrollbarStyle, GUI.skin.horizontalScrollbar);

            Vector2 scrollPositiontemp = GUI.BeginScrollView(GetRect(), scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbarStyle, verticalScrollbarStyle);
            doNots = DoNotModifyRect.True;
            ScrollViewAction?.Invoke(this);
            doNots = DoNotModifyRect.False;
            GUI.EndScrollView();

            if (scrollPositiontemp != scrollPosition)
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) 
                    onScrollView.Invoke(scrollPosition = scrollPositiontemp);
        }

        /// <summary>
        /// ScrollTo(<seealso cref="IGUObject"/>) deve ser chamado dentro do evento 
        /// <seealso cref="Action"/>&lt;<seealso cref="IGUScrollView"/>&gt; 
        /// ScrollViewAction
        /// </summary>
        public void ScrollTo(IGUObject iGUObject)
            => ScrollTo(iGUObject.MyRect);

        /// <summary>
        /// ScrollTo(<seealso cref="IGURect"/>) deve ser chamado dentro do evento 
        /// <seealso cref="Action"/>&lt;<seealso cref="IGUScrollView"/>&gt; 
        /// ScrollViewAction
        /// </summary>
        public void ScrollTo(IGURect rect)
            => ScrollTo(new Rect(rect.ModifiedPosition, rect.Size));

        /// <summary>
        /// ScrollTo(<seealso cref="Rect"/>) deve ser chamado dentro do evento 
        /// <seealso cref="Action"/>&lt;<seealso cref="IGUScrollView"/>&gt; 
        /// ScrollViewAction
        /// </summary>
        public void ScrollTo(Rect rect)
            => GUI.ScrollTo(rect);
    }
}
