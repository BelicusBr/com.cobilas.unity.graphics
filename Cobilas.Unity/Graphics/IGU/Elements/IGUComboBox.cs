using System;
using UnityEngine;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUComboBox : IGUObject, IEnumerable<IGUComboBoxButton>, IIGUToolTip, IIGUEndOfFrame {

        private Action onToolTip;
        [SerializeField] protected int index;
        [SerializeField] protected IGUButton cbx_button;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected float comboBoxButtonHeight;
        [SerializeField] protected IGUScrollView cbx_scrollview;
        [SerializeField] protected IGUOnClickEvent onActivatedComboBox;
        [SerializeField] protected IGUVerticalLayout cbx_verticalLayout;
        [SerializeField] protected IGUComboBoxClickEvent onSelectedIndex;
        [SerializeField] protected bool adjustComboBoxViewAccordingToTheButtonsPresent;
        private bool isIgnition;

        public string Text => cbx_button.Text;
        public Texture Image => cbx_button.Image;
        public IGUOnClickEvent OnClick => onClick;
        public int ButtonCount => cbx_verticalLayout.Count;
        //public bool IsClipping => cbx_scrollview.IsClipping;
        public int Index { get => index; set => SetIndex(value); }
        public IGUComboBoxClickEvent OnSelectedIndex => onSelectedIndex;
        public IGUOnClickEvent OnActivatedComboBox => onActivatedComboBox;
        public string ToolTip { get => cbx_button.ToolTip; set => cbx_button.ToolTip = value; }
        public Rect RectView { get => cbx_scrollview.RectView; set => cbx_scrollview.RectView = value; }
        public Vector2 ScrollView { get => cbx_scrollview.ScrollView; set => cbx_scrollview.ScrollView = value; }
        
        public float ComboBoxButtonHeight { 
            get => comboBoxButtonHeight;
            set {
                AdjustComboBoxView = adjustComboBoxViewAccordingToTheButtonsPresent;
                if (comboBoxButtonHeight != value) {
                    comboBoxButtonHeight = value;
                    ChangeVisibility(-ScrollView);
                }
            }
        }
        public IGUStyle TooltipStyle {
            get => cbx_button.TooltipStyle;
            set {
                cbx_button.TooltipStyle = value;
                if (ButtonCount != 0)
                    RecursiveList((c, i) => {
                        c.TooltipStyle = value;
                    }, 0, cbx_verticalLayout);
            }
        }
        public bool UseTooltip { 
            get => cbx_button.UseTooltip; 
            set { cbx_button.UseTooltip = value;
                if (cbx_verticalLayout.Count != 0)
                    RecursiveList((c, i) => {
                        c.UseTooltip = value;
                    }, 0, cbx_verticalLayout);
            }
        }
        public bool AdjustComboBoxView { 
            get => adjustComboBoxViewAccordingToTheButtonsPresent; 
            set {
                if (adjustComboBoxViewAccordingToTheButtonsPresent = value)
                    RectView = new Rect(
                        Vector2.zero, 
                        Vector2.right * myRect.Width + comboBoxButtonHeight * cbx_verticalLayout.Count * Vector2.up
                    );
            }
        }
        public float ScrollViewHeight { 
            get => cbx_scrollview.MyRect.Height; 
            set => cbx_scrollview.MyRect = cbx_scrollview.MyRect.SetSize(myRect.Width, value);
        }
        public bool CloseComboBoxView { 
            get => cbx_scrollview.MyConfig.IsVisible;
            set => cbx_scrollview.MyConfig = cbx_scrollview.MyConfig.SetVisible(value);
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
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }

        public IGUComboBoxButton this[int index] 
            => cbx_verticalLayout[index] as IGUComboBoxButton;

        protected override void IGUAwake() {
            base.IGUAwake();
            isPhysicalElement = false;
            cbx_button = IGUObject.Create<IGUButton>($"[{name}]--{nameof(IGUButton)}");
            cbx_scrollview = IGUObject.Create<IGUScrollView>($"[{name}]--{nameof(IGUScrollView)}");
            cbx_verticalLayout = IGUObject.Create<IGUVerticalLayout>($"[{name}]--{nameof(IGUVerticalLayout)}");
            physics = IGUBasicPhysics.Create<IGUCollectionPhysics>(this);
            (physics as IGUCollectionPhysics).OnCollision = true;
            ScrollViewHeight = 150f;
            AdjustComboBoxView = true;
            CloseComboBoxView = false;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            cbx_verticalLayout.Spacing = 0f;
            myColor = IGUColor.DefaultBoxColor;
            comboBoxButtonHeight = myRect.Height;
            cbx_verticalLayout.UseCellSize = true;
            onActivatedComboBox = new IGUOnClickEvent();
            onSelectedIndex = new IGUComboBoxClickEvent();
            cbx_button.Parent = cbx_scrollview.Parent = this;
            cbx_verticalLayout.Parent = cbx_scrollview;

            for (int I = 0; I < 10; I++) {
                Add($"Item[{I}]");
                //Debug.Log($"CH[{cbx_verticalLayout[I].name}]{cbx_verticalLayout[I].Parent is null}");
            }
            isIgnition = true;
            SetIndex(0);
        }

        protected override void IGUOnEnable() {
            base.IGUOnEnable();
            cbx_scrollview.ScrollViewAction += (scv) => {
                cbx_verticalLayout.MyRect = cbx_verticalLayout.MyRect.SetPosition(-scv.ScrollView);
                cbx_verticalLayout.OnIGU();
            };
            cbx_scrollview.OnScrollView.AddListener(ChangeVisibility);
            cbx_button.OnClick.AddListener(ChangeCbxScrollviewVisibility);
            #if UNITY_EDITOR
            onToolTip = (Action)null;
            #endif
            onToolTip += (cbx_button as IIGUToolTip).InternalDrawToolTip;
            for (int I = 0; I < ButtonCount && !isIgnition; I++) {
                IGUComboBoxButton boxButton = cbx_verticalLayout[I] as IGUComboBoxButton;
                onToolTip += (boxButton as IIGUToolTip).InternalDrawToolTip;
                boxButton.OnClick.AddListener(() => {
                    boxButton.OnClick.RemoveAllListeners();
                    SetIndex(boxButton.Index);
                    OnSelectedIndex.Invoke(boxButton);
                    CloseComboBoxView = false;
                });
            }
            isIgnition = false;
        }

        public void Add(string text, Texture image, string toolTip) {
            IGUComboBoxButton button = 
                IGUObject.Create<IGUComboBoxButton>($"Item[{ButtonCount}]");
            button.Text = text;
            button.Image = image;
            button.ToolTip = toolTip;
            button.Index = ButtonCount;
            button.UseTooltip = UseTooltip;
            button.Style = ComboBoxButtonStyle;
            button.TooltipStyle = TooltipStyle;
            onToolTip += (button as IIGUToolTip).InternalDrawToolTip;
            button.OnClick.AddListener(() => {
                SetIndex(button.Index);
                OnSelectedIndex.Invoke(button);
                CloseComboBoxView = false;
            });
            cbx_verticalLayout.Add(button);
            ComboBoxButtonHeight = comboBoxButtonHeight;
        }

        public void Add(string text, Texture image)
            => Add(text, image, string.Empty);

        public void Add(Texture image, string toolTip)
            => Add(string.Empty, image, toolTip);

        public void Add(string text, string toolTip)
            => Add(text, (Texture)null, toolTip);

        public void Add(string text)
            => Add(text, string.Empty);

        public void Add(params ValueTuple<string, Texture, string>[] itens) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(itens); I++)
                Add(itens[I].Item1, itens[I].Item2, itens[I].Item3);
        }

        public void Remove(int index) {
            if (cbx_verticalLayout.Remove(index, true))
                if (ButtonCount != 0) {
                    Rect rect = new Rect(ScrollView, cbx_scrollview.MyRect.Size);
                    onToolTip = (Action)null;
                    RecursiveList((c, i) => { 
                        c.Index = i;
                        onToolTip += (c as IIGUToolTip).InternalDrawToolTip;
                        Vector2 center = c.MyRect.Center;
                        Vector2 up = .5f * c.MyRect.Width * Vector2.right + Vector2.up * c.MyRect.Up;
                        Vector2 down = .5f * c.MyRect.Width * Vector2.right + Vector2.up * c.MyRect.Donw;
                        bool visible = rect.Contains(center) || rect.Contains(up) || rect.Contains(down);
                        c.MyConfig = c.MyConfig.SetVisible(visible);
                    }, 0, cbx_verticalLayout);
                }
        }

        public void Clear() => cbx_verticalLayout.Clear(true);

        public IEnumerator<IGUComboBoxButton> GetEnumerator() {
            for (int I = 0; I < ButtonCount; I++)
                yield return cbx_verticalLayout[I] as IGUComboBoxButton;
        }

        protected override void LowCallOnIGU() {
            cbx_button.MyRect = cbx_button.MyRect.SetPosition(Vector2.zero).SetSize(myRect.Size);
            cbx_scrollview.MyRect = cbx_scrollview.MyRect.SetPosition(Vector2.up * myRect.Height).SetSize(myRect.Width, ScrollViewHeight);
            float num1 = cbx_scrollview.VerticalScrollbarIsVisible ? cbx_scrollview.VerticalScrollbarStyle.FixedWidth : 0f;
            cbx_verticalLayout.CellSize = Vector2.right * (myRect.Width - num1) + Vector2.up * ComboBoxButtonHeight;

            cbx_button.OnIGU();
            cbx_scrollview.OnIGU();

            IGURect rect_scv = myRect;
            rect_scv = rect_scv.SetSize(rect_scv.Size + Vector2.up * (CloseComboBoxView ? cbx_scrollview.MyRect.Height: 0f));
            if (!rect_scv.Contains(IGUDrawer.MousePosition))
                if (IGUDrawer.GetMouseButtonDown(LocalConfig.MouseType) && CloseComboBoxView)
                    CloseComboBoxView = false;
        }

        private void ChangeVisibility(Vector2 vector) {
            Rect rect = new Rect(vector, cbx_scrollview.MyRect.Size);
            if (cbx_verticalLayout.Count != 0)
                RecursiveList((c, i) => {
                    Vector2 center = c.MyRect.Center;
                    Vector2 up = .5f * c.MyRect.Width * Vector2.right + Vector2.up * c.MyRect.Up;
                    Vector2 down = .5f * c.MyRect.Width * Vector2.right + Vector2.up * c.MyRect.Donw;
                    bool visible = rect.Contains(center) || rect.Contains(up) || rect.Contains(down);
                    c.MyConfig = c.MyConfig.SetVisible(visible);
                }, 0, cbx_verticalLayout);
        }

        private void ChangeCbxScrollviewVisibility() {
            OnClick.Invoke();
            CloseComboBoxView = !CloseComboBoxView;
            if (CloseComboBoxView)
                OnActivatedComboBox.Invoke();
        }

        void IIGUToolTip.InternalDrawToolTip() => onToolTip?.Invoke();

        private void SetComboBoxStyle(IGUStyle style) {
            cbx_button.ButtonStyle = style;
            if (cbx_verticalLayout.Count != 0)
                RecursiveList((c, i) => { c.Style = style; }, 0, cbx_verticalLayout);
        }

        private void SetIndex(int index) {
            this.index = index;
            if (cbx_verticalLayout.Count == 0) return;
            IGUComboBoxButton bt = cbx_verticalLayout[index] as IGUComboBoxButton;
            cbx_button.Text = bt.Text;
            cbx_button.Image = bt.Image;
        }
        
        IEnumerator IEnumerable.GetEnumerator() {
            for (int I = 0; I < ButtonCount; I++)
                yield return cbx_verticalLayout[I] as IGUComboBoxButton;
        }

        void IIGUEndOfFrame.EndOfFrame() => ((IIGUEndOfFrame)cbx_button).EndOfFrame();

        private static void RecursiveList(Action<IGUComboBoxButton, int> action, int index, IGUVerticalLayout verticalLayout) {
            action(verticalLayout[index] as IGUComboBoxButton, index++);
            if (index < verticalLayout.Count)
                RecursiveList(action, index, verticalLayout);
        }

        [Serializable]
        public class IGUComboBoxClickEvent : UnityEngine.Events.UnityEvent<IGUComboBoxButton> { }
    }
}
