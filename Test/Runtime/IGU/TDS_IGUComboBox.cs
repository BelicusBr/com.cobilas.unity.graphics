using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using Cobilas.Unity.Graphics.IGU.Elements.Auxiliary;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class TDS_IGUComboBox : IGUObject, IIGUClipping, IIGUSerializationCallbackReceiver {

        [SerializeField] private int index;
        [SerializeField] private IGUOnClickEvent onClick;
        [SerializeField] private float comboBoxButtonHeight;
        [SerializeField] private IGUOnClickEvent onActivatedComboBox;
        [SerializeField] protected TDS_IGUComboBoxButton[] cbx_buttons;
        [SerializeField] private IGUComboBoxClickEvent onSelectedIndex;
        [SerializeField, HideInInspector] protected IGUButton cbx_button;
        [SerializeField, HideInInspector] protected IGUScrollView cbx_scrollview;
        [SerializeField] private bool adjustComboBoxViewAccordingToTheButtonsPresent;

        protected Action IGUComboBoxButtonOnIGU;

        public string Text => cbx_button.Text;
        public Texture Image => cbx_button.Image;
        public IGUOnClickEvent OnClick => onClick;
        public int Index { get => index; set => SetIndex(value); }
        public IGUComboBoxClickEvent OnSelectedIndex => onSelectedIndex;
        public IGUOnClickEvent OnActivatedComboBox => onActivatedComboBox;
        public bool IsClipping => ((IIGUClipping)cbx_scrollview).IsClipping;
        public int ButtonCount => ArrayManipulation.ArrayLength(cbx_buttons);
        public string ToolTip { get => cbx_button.ToolTip; set => cbx_button.ToolTip = value; }
        public float ComboBoxButtonHeight { get => comboBoxButtonHeight; set => ComboBoxButtonHeightFunc(value); }
        public Rect RectView { get => ((IIGUClipping)cbx_scrollview).RectView; set => ((IIGUClipping)cbx_scrollview).RectView = value; }
        public Vector2 ScrollView { get => ((IIGUClipping)cbx_scrollview).ScrollView; set => ((IIGUClipping)cbx_scrollview).ScrollView = value; }
        public IGUStyle TooltipStyle {
            get => cbx_button.TooltipStyle;
            set {
                cbx_button.TooltipStyle = value;
                if (ButtonCount != 0)
                    RecursiveList((c, i) => {
                        c.TooltipStyle = value;
                    }, 0, cbx_buttons);
            }
        }
        public bool UseTooltip { 
            get => cbx_button.UseTooltip; 
            set { cbx_button.UseTooltip = value;
                if (!ArrayManipulation.EmpytArray(cbx_buttons))
                    RecursiveList((c, i) => {
                        c.UseTooltip = value;
                    }, 0, cbx_buttons);
            }
        }
        public bool AdjustComboBoxView { 
            get => adjustComboBoxViewAccordingToTheButtonsPresent; 
            set { 
                adjustComboBoxViewAccordingToTheButtonsPresent = value;
                ComboBoxButtonHeightFunc(comboBoxButtonHeight);
            }
        }
        public float ScrollViewHeight { 
            get => cbx_scrollview.MyRect.Height; 
            set => cbx_scrollview.MyRect = cbx_scrollview.MyRect.SetSize(myRect.Width, value);
        }
        public bool CloseComboBoxView { 
            get => cbx_scrollview.MyConfg.IsVisible;
            set => cbx_scrollview.MyConfg = cbx_scrollview.MyConfg.SetVisible(value);
        }
        public IGUStyle ComboBoxButtonStyle { 
            get => cbx_button.ButtonStyle;
            set => SetComboBoxStyle(value);
        }
        public IGUStyle VerticalScrollbarStyle { 
            get => cbx_scrollview.VerticalScrollbarStyle;
            set => cbx_scrollview.VerticalScrollbarStyle = value;
        }
        public IGUStyle HorizontalScrollbarStyle { 
            get => cbx_scrollview.HorizontalScrollbarStyle;
            set => cbx_scrollview.HorizontalScrollbarStyle = value;
        }
        public IGUStyle VerticalScrollbarThumbStyle { 
            get => cbx_scrollview.VerticalScrollbarThumbStyle;
            set => cbx_scrollview.VerticalScrollbarThumbStyle = value;
        }
        public IGUStyle HorizontalScrollbarThumbStyle { 
            get => cbx_scrollview.HorizontalScrollbarThumbStyle;
            set => cbx_scrollview.HorizontalScrollbarThumbStyle = value;
        }

        public TDS_IGUComboBoxButton this[int index] 
            => cbx_buttons[index];

        protected override void Start() {
            base.Start();
            cbx_button = IGUObject.CreateIGUInstance<IGUButton>($"[{name}]--IGUButton");
            cbx_scrollview = IGUObject.CreateIGUInstance<IGUScrollView>($"[{name}]--IGUScrollView");
            ScrollViewHeight = 150f;
            AdjustComboBoxView = true;
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            myColor = IGUColor.DefaultBoxColor;
            comboBoxButtonHeight = myRect.Height;
            onActivatedComboBox = new IGUOnClickEvent();
            onSelectedIndex = new IGUComboBoxClickEvent();
            cbx_button.Parent = cbx_scrollview.Parent = this;

            for (int I = 0; I < 10; I++)
                Add($"Item[{I}]");
            SetIndex(0);
        }

        protected override void IgnitionEnable() {
            base.IgnitionEnable();
            cbx_scrollview.ScrollViewAction += (scv)=>{
                IGUComboBoxButtonOnIGU?.Invoke();
            };
            cbx_scrollview.OnScrollView.AddListener(ChangeVisibility);
            cbx_button.OnClick.AddListener(ChangeCbxScrollviewVisibility);
            for (int I = 0; I < ButtonCount; I++) {
                TDS_IGUComboBoxButton boxButton = cbx_buttons[I];
                IGUComboBoxButtonOnIGU += cbx_buttons[I].OnIGU;
                cbx_buttons[I].OnClick.AddListener(() => {
                    SetIndex(boxButton.Index);
                    OnSelectedIndex.Invoke(boxButton);
                    CloseComboBoxView = false;
                });
            }
        }

        public void Add(string text, Texture image, string toolTip) {
            TDS_IGUComboBoxButton button = 
                IGUObject.CreateIGUInstance<TDS_IGUComboBoxButton>($"Item[{ButtonCount}]");
            button.Text = text;
            button.Image = image;
            button.ToolTip = toolTip;
            button.UseTooltip = UseTooltip;
            button.Style = ComboBoxButtonStyle;
            button.TooltipStyle = TooltipStyle;
            IGUComboBoxButtonOnIGU += button.OnIGU;
            ArrayManipulation.Add(button, ref cbx_buttons);
            ComboBoxButtonHeightFunc(comboBoxButtonHeight);
        }

        public void Add(string text, Texture image)
            => Add(text, image, string.Empty);

        public void Add(Texture image, string toolTip)
            => Add(string.Empty, image, toolTip);

        public void Add(string text, string toolTip)
            => Add(text, (Texture)null, toolTip);

        public void Add(string text)
            => Add(text, string.Empty);

        public void Remove(int index) {
            IGUComboBoxButtonOnIGU += cbx_buttons[index].OnIGU;
            ArrayManipulation.Remove(index, ref cbx_buttons);
            if (ButtonCount != 0)
                ComboBoxButtonHeightFunc(comboBoxButtonHeight);
        }

        public void Clear() {
            for (int I = 0; I < ButtonCount; I++)
                Destroy(cbx_buttons[I]);
            IGUComboBoxButtonOnIGU = null;
            ArrayManipulation.ClearArraySafe(ref cbx_buttons);
        }

        //LowCallOnIGU
        protected override void LowCallOnIGU() {
            cbx_button.MyRect = cbx_button.MyRect.SetPosition(Vector2.zero).SetSize(myRect.Size);
            cbx_scrollview.MyRect = cbx_scrollview.MyRect.SetPosition(Vector2.up * myRect.Size.y).SetSize(myRect.Size.x, 150f);

            cbx_button.OnIGU();
            cbx_scrollview.OnIGU();
        }

        private void ChangeVisibility(Vector2 vector) {
            Vector2 scv_size = cbx_scrollview.MyRect.Size;
            if (!ArrayManipulation.EmpytArray(cbx_buttons))
                RecursiveList((c, i) => {
                    bool visible = c.MyRect.Donw >= vector.y &&
                    c.MyRect.Up <= vector.y + scv_size.y;
                    c.MyConfg = c.MyConfg.SetVisible(visible);
                }, 0, cbx_buttons);
        }

        private void ChangeCbxScrollviewVisibility() {
            OnClick.Invoke();
            CloseComboBoxView = !CloseComboBoxView;
            if (CloseComboBoxView)
                OnActivatedComboBox.Invoke();
        }

        private void SetComboBoxStyle(IGUStyle style) {
            cbx_button.ButtonStyle = style;
            if (!ArrayManipulation.EmpytArray(cbx_buttons))
                RecursiveList((c, i) => { c.Style = style; }, 0, cbx_buttons);
        }

        private void SetIndex(int index) {
            this.index = index;
            cbx_button.Text = cbx_buttons[index].Text;
            cbx_button.Image = cbx_buttons[index].Image;
        }

        void IIGUSerializationCallbackReceiver.Reserialization() {
#if UNITY_EDITOR

#endif
        }

        private void ComboBoxButtonHeightFunc(float comboBoxButtonHeight) {
                this.comboBoxButtonHeight = comboBoxButtonHeight;
                if (!ArrayManipulation.EmpytArray(cbx_buttons)) {
                    if (adjustComboBoxViewAccordingToTheButtonsPresent)
                        RectView = new Rect(
                            Vector2.zero, 
                            Vector2.right * myRect.Width + comboBoxButtonHeight * cbx_buttons.Length * Vector2.up
                        );
                    RecursiveList((c, i) => {
                        c.Index = i;
                        c.MyRect = c.MyRect.SetPosition(comboBoxButtonHeight * i * Vector2.up)
                            .SetSize(c.MyRect.Width, comboBoxButtonHeight);
                    }, 0, cbx_buttons);
                }
        }

        private static void RecursiveList(Action<TDS_IGUComboBoxButton, int> action, int index, TDS_IGUComboBoxButton[] cbx_buttons) {
            action(cbx_buttons[index], index++);
            if (index < ArrayManipulation.ArrayLength(cbx_buttons))
                RecursiveList(action, index, cbx_buttons);
        }

        [Serializable]
        public class IGUComboBoxClickEvent : UnityEngine.Events.UnityEvent<TDS_IGUComboBoxButton> { }
    }
}
