﻿using System;
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

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();
            if (!config.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = config.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            verticalScrollbarStyle = GetDefaultValue(verticalScrollbarStyle, GUI.skin.verticalScrollbar);
            horizontalScrollbarStyle = GetDefaultValue(horizontalScrollbarStyle, GUI.skin.horizontalScrollbar);

            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            Vector2 scrollPositiontemp = GUI.BeginScrollView(rectTemp, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbarStyle, verticalScrollbarStyle);
            BeginDoNotModifyRect(true);
            ScrollViewAction?.Invoke(this);
            EndDoNotModifyRect();
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

        protected override void SetDefaultValue(IGUDefaultValue value) {
            if (value == null) value = IGUScrollViewDefault.DefaultValue;
            else if (value.GetType() == typeof(IGUScrollViewDefault))
                throw new IGUException();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultTextArea;
            myColor = IGUColor.DefaultBoxColor;
            onScrollView = new IGUScrollViewEvent();
            name = value.GetValue<string>(0L);
            viewRect = value.GetValue<Rect>(1L);
            alwaysShowVertical = value.GetValue<bool>(2L);
            alwaysShowHorizontal = value.GetValue<bool>(3L);
            verticalScrollbarStyle = value.GetValue<GUIStyle>(4L);
            horizontalScrollbarStyle = value.GetValue<GUIStyle>(5L);
        }

        public static IGUScrollView CreateIGUInstance(string name, Rect viewRect, bool alwaysShowVertical, bool alwaysShowHorizontal) {
            IGUScrollView scrollView = Internal_CreateIGUInstance<IGUScrollView>(name);
            scrollView.viewRect = viewRect;
            scrollView.myConfg = IGUConfig.Default;
            scrollView.myRect = IGURect.DefaultTextArea;
            scrollView.myColor = IGUColor.DefaultBoxColor;
            scrollView.onScrollView = new IGUScrollViewEvent();
            scrollView.alwaysShowVertical = alwaysShowVertical;
            scrollView.alwaysShowHorizontal = alwaysShowHorizontal;
            return scrollView;
        }

        public static IGUScrollView CreateIGUInstance(string name, Rect viewRect)
            => CreateIGUInstance(name, viewRect, false, false);

        public static IGUScrollView CreateIGUInstance(string name)
            => CreateIGUInstance(name, new Rect(0, 0, 250f, 250f));
    }
}
