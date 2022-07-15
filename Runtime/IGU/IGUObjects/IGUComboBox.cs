using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUComboBox : IGUObject, ISerializationCallbackReceiver {

        private HashCodeCompare compare = new HashCodeCompare(4);
        private bool _onActivatedComboBox;

        [SerializeField] protected int index;
        [SerializeField] protected bool activatedComboBox;
        [SerializeField] protected float scrollViewHeight;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUButton comboBoxButton;
        [SerializeField] protected float comboBoxButtonHeight;
        [SerializeField] protected IGUComboBoxButton[] boxButtons;
        [SerializeField] protected IGUScrollView comboBoxScrollView;
        [SerializeField] protected GUIStyle scrollViewBackgroundStyle;
        [SerializeField] protected bool closeOnClickComboBoxViewButton;
        [SerializeField] protected IGUOnClickEvent onActivatedComboBox;
        [SerializeField] protected IGUComboBoxClickEvent onSelectedIndex;
        [SerializeField] protected bool adjustComboBoxViewAccordingToTheButtonsPresent;
#if UNITY_EDITOR
        private bool afterDeserialize = false;
        [SerializeField] protected IGUContent[] copyList;
        [SerializeField] protected IGUComboBoxButton[] destroyList;
#endif
        protected event Action<float> IGUComboBoxButtonChangeWidth;
        protected event Action<float> IGUComboBoxButtonChangeHeight;
        protected event Action<IGUColor> IGUComboBoxButtonChangeIGUColor;
        protected event Action<IGUConfig> IGUComboBoxButtonChangeIGUConfig;
        protected event Action<GUIStyle, IGUScrollView> IGUComboBoxButtonOnIGU;

        public IGUOnClickEvent OnClick => onClick;
        public IGUComboBoxClickEvent OnSelectedIndex => onSelectedIndex;
        public IGUOnClickEvent OnActivatedComboBox => onActivatedComboBox;
        public int Index { get => index; set => SetDisplayText(index = value); }
        /// <summary>A altura do scrollView.(130f padrão)</summary>
        public float ScrollViewHeight { get => scrollViewHeight; set => scrollViewHeight = value; }
        public bool UseTooltip { get => comboBoxButton.UseTooltip; set => comboBoxButton.UseTooltip = value; }
        /// <summary>A altura dos botões.(25f padrão)</summary>
        public float ComboBoxButtonHeight { get => comboBoxButtonHeight; set => comboBoxButtonHeight = value; }
        public IGUContent MyContent { get => comboBoxButton.MyContent; set => comboBoxButton.MyContent = value; }
        public GUIStyle TooltipStyle { get => comboBoxButton.TooltipStyle; set => comboBoxButton.TooltipStyle = value; }
        public GUIStyle ComboBoxButtonStyle { get => comboBoxButton.ButtonStyle; set => comboBoxButton.ButtonStyle = value; }
        public GUIStyle ScrollViewBackgroundStyle { get => scrollViewBackgroundStyle; set => scrollViewBackgroundStyle = value; }
        public GUIStyle VerticalScrollbarStyle { get => comboBoxScrollView.VerticalScrollbarStyle; set => comboBoxScrollView.VerticalScrollbarStyle = value; }
        public GUIStyle HorizontalScrollbarStyle { get => comboBoxScrollView.HorizontalScrollbarStyle; set => comboBoxScrollView.HorizontalScrollbarStyle = value; }

        protected override void OnEnable() {
#if UNITY_EDITOR
            DestroyList(ref destroyList);
            if (afterDeserialize) {
                InitScrollViewAction();
                PopulateEvents(boxButtons = CreateIGUComboBoxButtonList(copyList));
                afterDeserialize = false;
            }
#endif
        }

        public override void OnIGU() {
            if (!myConfg.IsVisible) return;

            comboBoxButton.MyRect = comboBoxButton.MyRect.SetSize(myRect.Size);
            comboBoxButton.MyRect = comboBoxScrollView.MyRect = comboBoxButton.MyRect.SetPivot(Vector2.zero);
            comboBoxButton.MyRect = comboBoxScrollView.MyRect = comboBoxButton.MyRect.SetScaleFactor(Vector2.one);
            comboBoxButton.MyColor = myColor;
            comboBoxButton.MyConfg = myConfg;

            float rwWidth = myRect.Width;
            float rwHeight = comboBoxButtonHeight * ArrayManipulation.ArrayLength(boxButtons);
            if (comboBoxScrollView.VerticalScrollbarStyle != (GUIStyle)null && rwHeight > scrollViewHeight)
                rwWidth -= comboBoxScrollView.VerticalScrollbarStyle.fixedWidth + 1f;

            float newScrollViewHeight = adjustComboBoxViewAccordingToTheButtonsPresent ?
                rwHeight > scrollViewHeight ? scrollViewHeight : rwHeight :
                scrollViewHeight;

            Rect rectView = new Rect(0, 0, rwWidth, rwHeight);
            Rect rectTemp = new Rect();
            rectTemp.position = GetPosition();
            rectTemp.width = myRect.Width;
            rectTemp.height = scrollViewHeight + myRect.Height;

            Event current = Event.current;

            comboBoxScrollView.MyRect = comboBoxScrollView.MyRect.SetPosition(0, myRect.Height);
            comboBoxScrollView.MyRect = comboBoxScrollView.MyRect.SetSize(myRect.Width, newScrollViewHeight);
            comboBoxScrollView.MyColor = myColor;
            comboBoxScrollView.MyConfg = myConfg;
            comboBoxScrollView.ViewRect = rectView;

            comboBoxButton.OnIGU();
            if (!rectTemp.Contains(current.mousePosition))
                if (current.clickCount > 0)
                    activatedComboBox = false;

            if (activatedComboBox) {
                if (_onActivatedComboBox) {
                    _onActivatedComboBox = false;
                    onActivatedComboBox.Invoke();
                }
                if (!compare.HashCodeEqual(3, rwWidth.GetHashCode()))
                    IGUComboBoxButtonChangeWidth?.Invoke(rwWidth);

                scrollViewBackgroundStyle = GetDefaultValue(scrollViewBackgroundStyle, GUI.skin.box);

                GUI.Box(new Rect(comboBoxScrollView.MyRect.ModifiedPosition, comboBoxScrollView.MyRect.Size),
                    IGUTextObject.GetGUIContentTemp(""), scrollViewBackgroundStyle);

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
            IGUComboBoxButton[] Res = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(contents); I++)
                ArrayManipulation.Add(new IGUComboBoxButton(I, GetInstanceID(), contents[I]), ref Res);
            return Res;
        }

        private void PopulateEvents(IGUComboBoxButton[] boxButtons) {
            this.IGUComboBoxButtonChangeWidth = (Action<float>)null;
            this.IGUComboBoxButtonChangeHeight = (Action<float>)null;
            this.IGUComboBoxButtonChangeIGUColor = (Action<IGUColor>)null;
            this.IGUComboBoxButtonChangeIGUConfig = (Action<IGUConfig>)null;
            this.IGUComboBoxButtonOnIGU = (Action<GUIStyle, IGUScrollView>)null;
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

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            compare = new HashCodeCompare(4);
#if UNITY_EDITOR
            destroyList = boxButtons;
            afterDeserialize = true;
#endif
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            DestroyList(ref boxButtons);
        }
        public static IGUComboBox CreateIGUInstance(string name, int index, params IGUContent[] buttons) {
            IGUComboBox comboBox = Internal_CreateIGUInstance<IGUComboBox>(name);
            comboBox.scrollViewHeight = 130f;
            comboBox.activatedComboBox = false;
            comboBox.comboBoxButtonHeight = 25f;
            comboBox._onActivatedComboBox = true;
            comboBox.myConfg = IGUConfig.Default;
            comboBox.myRect = IGURect.DefaultButton;
            comboBox.onClick = new IGUOnClickEvent();
            comboBox.myColor = IGUColor.DefaultBoxColor;
            comboBox.closeOnClickComboBoxViewButton = true;
            comboBox.onActivatedComboBox = new IGUOnClickEvent();
            comboBox.onSelectedIndex = new IGUComboBoxClickEvent();
            comboBox.adjustComboBoxViewAccordingToTheButtonsPresent = true;
            comboBox.comboBoxButton = IGUButton.CreateIGUInstance($"({name})-ComboBoxButton");
            comboBox.comboBoxScrollView = IGUScrollView.CreateIGUInstance($"({name})-ComboBoxScrollView");
            comboBox.comboBoxButton.Parent = comboBox.comboBoxScrollView.Parent = comboBox;
            comboBox.InitScrollViewAction();
            comboBox.SetIGUComboBoxButtonList(buttons);
            comboBox.Index = index;
            return comboBox;
        }

        public static IGUComboBox CreateIGUInstance(string name, params IGUContent[] buttons)
            => CreateIGUInstance(name, -1, buttons);

        public static IGUComboBox CreateIGUInstance(string name, int index, params string[] buttons)
            => CreateIGUInstance(name, index,
                ArrayManipulation.EmpytArray(buttons) ? (IGUContent[])null :
                Array.ConvertAll<string, IGUContent>(buttons, (b) => new IGUContent(b))
                );

        public static IGUComboBox CreateIGUInstance(string name, params string[] buttons)
            => CreateIGUInstance(name, -1, buttons);

        public static IGUComboBox CreateIGUInstance(string name)
            => CreateIGUInstance(name, "Item 1", "Item 2", "Item 3");

        [Serializable]
        public class IGUComboBoxClickEvent : UnityEngine.Events.UnityEvent<IGUComboBoxButton> { }
    }
}
