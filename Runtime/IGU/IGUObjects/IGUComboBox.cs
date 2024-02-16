﻿using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUComboBox : IGUObject, IIGUClipping, IIGUSerializationCallbackReceiver {

        private HashCodeCompare compare;
        private bool _onActivatedComboBox;

        [SerializeField] protected int index;
        [SerializeField] protected bool activatedComboBox;
        [SerializeField] protected float scrollViewHeight;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUButton comboBoxButton;
        [SerializeField] protected float comboBoxButtonHeight;
        [SerializeField] protected IGUComboBoxButton[] boxButtons;
        [SerializeField] protected IGUScrollView comboBoxScrollView;
        [SerializeField] protected IGUStyle scrollViewBackgroundStyle;
        [SerializeField] protected bool closeOnClickComboBoxViewButton;
        [SerializeField] protected IGUOnClickEvent onActivatedComboBox;
        [SerializeField] protected IGUComboBoxClickEvent onSelectedIndex;
        [SerializeField] protected bool adjustComboBoxViewAccordingToTheButtonsPresent;
#if UNITY_EDITOR
        [SerializeField] protected IGUContent[] copyList;
        [SerializeField] protected IGUComboBoxButton[] destroyList;
#endif
        protected event Action<float> IGUComboBoxButtonChangeWidth;
        protected event Action<float> IGUComboBoxButtonChangeHeight;
        protected event Action<IGUColor> IGUComboBoxButtonChangeIGUColor;
        protected event Action<IGUConfig> IGUComboBoxButtonChangeIGUConfig;
        protected event Action<IGUStyle, IGUScrollView> IGUComboBoxButtonOnIGU;

        public IGUOnClickEvent OnClick => onClick;
        public IGUComboBoxButton[] BoxButtons => boxButtons;
        public IGUComboBoxClickEvent OnSelectedIndex => onSelectedIndex;
        public IGUOnClickEvent OnActivatedComboBox => onActivatedComboBox;
        public int Index { get => index; set => SetDisplayText(index = value); }
        /// <summary>A altura do scrollView.(130f padrão)</summary>
        public string Tooltip { get => comboBoxButton.ToolTip; set => comboBoxButton.ToolTip = value; }
        public float ScrollViewHeight { get => scrollViewHeight; set => scrollViewHeight = value; }
        public bool UseTooltip { get => comboBoxButton.UseTooltip; set => comboBoxButton.UseTooltip = value; }
        /// <summary>A altura dos botões.(25f padrão)</summary>
        public float ComboBoxButtonHeight { get => comboBoxButtonHeight; set => comboBoxButtonHeight = value; }
        public IGUContent MyContent { get => comboBoxButton.MyContent; set => comboBoxButton.MyContent = value; }
        public IGUStyle TooltipStyle { get => comboBoxButton.TooltipStyle; set => comboBoxButton.TooltipStyle = value; }
        public IGUStyle ComboBoxButtonStyle { get => comboBoxButton.ButtonStyle; set => comboBoxButton.ButtonStyle = value; }
        /// <summary>A propriedade permite o <see cref="IGUComboBox"/> a fechar janela de exibição de botões automaticamente.</summary>
        public bool CloseComboBoxView { get => closeOnClickComboBoxViewButton; set => closeOnClickComboBoxViewButton = value; }
        public IGUStyle ScrollViewBackgroundStyle { get => scrollViewBackgroundStyle; set => scrollViewBackgroundStyle = value; }
        public IGUStyle VerticalScrollbarStyle { get => comboBoxScrollView.VerticalScrollbarStyle; set => comboBoxScrollView.VerticalScrollbarStyle = value; }
        public bool AdjustComboBoxView { get => adjustComboBoxViewAccordingToTheButtonsPresent; set => adjustComboBoxViewAccordingToTheButtonsPresent = value; }
        public IGUStyle HorizontalScrollbarStyle { get => comboBoxScrollView.HorizontalScrollbarStyle; set => comboBoxScrollView.HorizontalScrollbarStyle = value; }
        public IGUStyle VerticalScrollbarThumbStyle { get => comboBoxScrollView.VerticalScrollbarThumbStyle; set => comboBoxScrollView.VerticalScrollbarThumbStyle = value; }
        public IGUStyle HorizontalScrollbarThumbStyle { get => comboBoxScrollView.HorizontalScrollbarThumbStyle; set => comboBoxScrollView.HorizontalScrollbarThumbStyle = value; }
        
        Rect IIGUClipping.RectView { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        protected override void Awake() {
            base.Awake();
            compare = new HashCodeCompare(4);
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultBoxColor;
            boxButtons = new IGUComboBoxButton[0];
            activatedComboBox = false;
            _onActivatedComboBox = true;
            scrollViewHeight = 130f;
            comboBoxButtonHeight = 25f;
            closeOnClickComboBoxViewButton =
            adjustComboBoxViewAccordingToTheButtonsPresent = true;
            onClick = new IGUOnClickEvent();
            scrollViewBackgroundStyle = IGUSkins.GetIGUStyle("Black box border");
            onActivatedComboBox = new IGUOnClickEvent();
            onSelectedIndex = new IGUComboBoxClickEvent();
            comboBoxButton = CreateIGUInstance<IGUButton>($"--[{name}]ComboBoxButton");
            comboBoxScrollView = CreateIGUInstance<IGUScrollView>($"--[{name}]ComboBoxScrollView");
            comboBoxButton.Parent = comboBoxScrollView.Parent = this;
            InitScrollViewAction();
            SetIGUComboBoxButtonList("Item1", "Item2", "Item3");
            Index = 0;
        }
        //LowCallOnIGU
        protected override void LowCallOnIGU() {

            comboBoxButton.MyRect = comboBoxButton.MyRect.SetSize(myRect.Size);
            comboBoxButton.MyColor = myColor;

            float rwWidth = myRect.Width;
            float rwHeight = comboBoxButtonHeight * ArrayManipulation.ArrayLength(boxButtons);
            if (rwHeight > scrollViewHeight)
                rwWidth -= comboBoxScrollView.VerticalScrollbarStyle.FixedWidth + 1f;

            float newScrollViewHeight = adjustComboBoxViewAccordingToTheButtonsPresent ?
                rwHeight > scrollViewHeight ? scrollViewHeight : rwHeight :
                scrollViewHeight;

            Rect rectView = new Rect(0, 0, rwWidth, rwHeight);
            Rect rectTemp = GetRect();
            rectTemp.width = myRect.Width;
            rectTemp.height = scrollViewHeight + myRect.Height;

            comboBoxScrollView.MyRect = comboBoxScrollView.MyRect.SetPosition(0, comboBoxButton.MyRect.Height);
            comboBoxScrollView.MyRect = comboBoxScrollView.MyRect.SetSize(myRect.Width, newScrollViewHeight);
            comboBoxScrollView.MyColor = myColor;
            comboBoxScrollView.ViewRect = rectView;

            comboBoxButton.OnIGU();
            rectTemp.position = GetRect(true).position;
            if (!rectTemp.Contains(IGUDrawer.Drawer.GetMousePosition()))
                if (IGUDrawer.Drawer.GetMouseButtonUp(myConfg.MouseType))
                    activatedComboBox = false;

            if (activatedComboBox) {
                if (_onActivatedComboBox) {
                    _onActivatedComboBox = false;
                    onActivatedComboBox.Invoke();
                }
                if (!compare.HashCodeEqual(3, rwWidth.GetHashCode()))
                    IGUComboBoxButtonChangeWidth?.Invoke(rwWidth);

                GUI.Box(new Rect(rectTemp.position + Vector2.up * comboBoxButton.MyRect.Height, comboBoxScrollView.MyRect.Size),
                    IGUTextObject.GetGUIContentTemp(""), IGUStyle.GetGUIStyleTemp(scrollViewBackgroundStyle));

                comboBoxScrollView.OnIGU();

                if (!compare.HashCodeEqual(0, myConfg.GetHashCode()))
                    IGUComboBoxButtonChangeIGUConfig?.Invoke(myConfg);

                if (!compare.HashCodeEqual(1, myColor.GetHashCode()))
                    IGUComboBoxButtonChangeIGUColor?.Invoke(myColor);

                if (!compare.HashCodeEqual(2, comboBoxButtonHeight.GetHashCode()))
                    IGUComboBoxButtonChangeHeight?.Invoke(comboBoxButtonHeight);
            } else _onActivatedComboBox = true;
        }

        public void SetIGUComboBoxButtonList(params string[] contents) {
            if (!ArrayManipulation.EmpytArray(contents))
                SetIGUComboBoxButtonList(Array.ConvertAll<string, IGUContent>(contents, (s) => new IGUContent(s)));
        }

        public void SetIGUComboBoxButtonList(params IGUContent[] contents) {
            DestroyList(ref boxButtons);
            PopulateEvents(boxButtons = CreateIGUComboBoxButtonList(contents));
        }

        private void SetDisplayText(int index) {
            if (MyContent == null) MyContent = new IGUContent();
            if (index < 0 || index >= ArrayManipulation.ArrayLength(boxButtons)) {
                MyContent.Text = "";
                return;
            }
            MyContent.Text = boxButtons[index].Content.Text;
            MyContent.Image = boxButtons[index].Content.Image;
        }

        private void ChangeSelectedIndex(IGUComboBoxButton selectedIndex) {
            if (closeOnClickComboBoxViewButton)
                activatedComboBox = false;
            SetDisplayText(index = selectedIndex.Index);
            onSelectedIndex.Invoke(selectedIndex);
        }

        private void OnClickEvent()
            => onClick.Invoke();

        private IGUComboBoxButton[] CreateIGUComboBoxButtonList(params IGUContent[] contents) {
#if UNITY_EDITOR
            if (ArrayManipulation.EmpytArray(contents)) copyList = (IGUContent[])null;
            else contents = (IGUContent[])(copyList = contents).Clone();
#endif
            IGUComboBoxButton[] Res = new IGUComboBoxButton[0];
            for (int I = 0; I < ArrayManipulation.ArrayLength(contents); I++)
                ArrayManipulation.Add(new IGUComboBoxButton(I, GetInstanceID(), contents[I]), ref Res);
            return Res;
        }

        private void PopulateEvents(IGUComboBoxButton[] boxButtons) {
            this.IGUComboBoxButtonChangeWidth = (Action<float>)null;
            this.IGUComboBoxButtonChangeHeight = (Action<float>)null;
            this.IGUComboBoxButtonChangeIGUColor = (Action<IGUColor>)null;
            this.IGUComboBoxButtonChangeIGUConfig = (Action<IGUConfig>)null;
            this.IGUComboBoxButtonOnIGU = (Action<IGUStyle, IGUScrollView>)null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(boxButtons); I++) {
                this.IGUComboBoxButtonOnIGU += boxButtons[I].OnIGU;
                this.IGUComboBoxButtonChangeWidth += boxButtons[I].ChangeWidth;
                this.IGUComboBoxButtonChangeHeight += boxButtons[I].ChangeHeight;
                this.IGUComboBoxButtonChangeIGUColor += boxButtons[I].ChangeIGUColor;
                this.IGUComboBoxButtonChangeIGUConfig += boxButtons[I].ChangeIGUConfig;
                boxButtons[I].AddOnClick(OnClickEvent);
                boxButtons[I].AddSelectedIndex(ChangeSelectedIndex);
            }
        }

        private void DestroyList(ref IGUComboBoxButton[] boxButtons) {
            if (!ArrayManipulation.EmpytArray(boxButtons)) {
                for (int I = 0; I < boxButtons.Length; I++)
                    boxButtons[I].OnDestroy();
                ArrayManipulation.ClearArraySafe(ref boxButtons);
            }
        }

        private void InitScrollViewAction() {
            comboBoxButton.OnClick.AddListener(() => {
                activatedComboBox = !activatedComboBox;
            });
            comboBoxScrollView.ScrollViewAction += (sv) => {
                IGUComboBoxButtonOnIGU?.Invoke(comboBoxButton.ButtonStyle, sv);
            };
        }

        protected override void OnIGUDestroy() {
            base.OnIGUDestroy();
            DestroyList(ref boxButtons);
        }

        void IIGUSerializationCallbackReceiver.Reserialization() {
#if UNITY_EDITOR
            compare = new HashCodeCompare(4);
            destroyList = boxButtons;
            DestroyList(ref destroyList);
            InitScrollViewAction();
            PopulateEvents(boxButtons = CreateIGUComboBoxButtonList(copyList));
#endif
        }

        [Serializable]
        public class IGUComboBoxClickEvent : UnityEngine.Events.UnityEvent<IGUComboBoxButton> { }
    }
}
