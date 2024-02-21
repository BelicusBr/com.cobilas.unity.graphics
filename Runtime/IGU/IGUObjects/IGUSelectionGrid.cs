using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUSelectionGrid : IGUObject, IIGUSerializationCallbackReceiver {

        private readonly HashCodeCompare compare = new HashCodeCompare(6);
        [SerializeField] protected int _xCount;
        [SerializeField] protected bool useTooltip;
        [SerializeField] protected Vector2 spacing;
        [SerializeField] protected int selectedIndex;
        [SerializeField] protected IGUStyle tooltipToggleStype;
        [SerializeField] protected IGUStyle selectionGridToggleStyle;
        [SerializeField] protected IGUOnSliderIntValueEvent onSelectedIndex;
        [SerializeField] protected IGUSelectionGridToggle[] selectionGridToggles;
#if UNITY_EDITOR
        [SerializeField] protected IGUContent[] copyList;
        [SerializeField, HideInInspector] protected IGUSelectionGridToggle[] destroyList;
#endif
        protected event Action<bool> UseTooltipEvent;
        protected event Action<int> IGUSelectionGridToggleSelectedIndex;
        protected event Action<Vector2> IGUSelectionGridToggleChangeSpacing;
        protected event Action<IGUColor> IGUSelectionGridToggleChangeIGUColor;
        protected event Action<IGUStyle, IGUStyle> IGUSelectionGridToggleOnIGU;
        //protected event Action<IGUConfig> IGUSelectionGridToggleChangeIGUConfig;

        /// <summary>Espaçamento entre os botões.(3x3 padrão)</summary>
        public Vector2 Spacing { get => spacing; set => spacing = value; }
        public IGUOnSliderIntValueEvent OnSelectedIndex => onSelectedIndex;
        public bool UseTooltip { get => useTooltip; set => useTooltip = value; }
        public IGUSelectionGridToggle[] SelectionGridToggles => selectionGridToggles;
        /// <summary>Comprimento até a quebra de linha.</summary>
        public int xCount { get => _xCount; set => _xCount = value < 1 ? 1 : value; }
        /// <summary>Indice do botão selecionado.</summary>
        public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }
        public IGUStyle TooltipToggleStype { get => tooltipToggleStype; set => tooltipToggleStype = value; }
        public IGUStyle SelectionGridToggleStyle { get => selectionGridToggleStyle; set => selectionGridToggleStyle = value; }
        public Rect RectView => new Rect(
            myRect.ModifiedPosition,
            new Vector2(
                (myRect.Width + spacing.x) * xCount,
                (myRect.Height + spacing.y) * (ArrayManipulation.ArrayLength(selectionGridToggles) / xCount)
                )
            );

        protected override void Ignition() {
            base.Ignition();
            _xCount = 3;
            spacing = Vector2.one * 3f;
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            myRect = IGURect.DefaultSelectionGrid;
            onSelectedIndex = new IGUOnSliderIntValueEvent();
            selectionGridToggles = new IGUSelectionGridToggle[0];
            SetSelectionGridToggleList("Toggle1", "Toggle2", "Toggle3", "Toggle4", "Toggle5", "Toggle6");
        }

        protected override void LowCallOnIGU() {
            if (!GetModIGUConfig().IsVisible) return;

            if (!compare.HashCodeEqual(5, selectedIndex))
                IGUSelectionGridToggleSelectedIndex?.Invoke(selectedIndex);

            IGUSelectionGridToggleOnIGU?.Invoke(selectionGridToggleStyle, tooltipToggleStype);

            if (!compare.HashCodeEqual(0, myColor.GetHashCode()))
                IGUSelectionGridToggleChangeIGUColor?.Invoke(myColor);

            //if (!compare.HashCodeEqual(1, myConfg.GetHashCode()))
            //    IGUSelectionGridToggleChangeIGUConfig?.Invoke(myConfg);

            if (!compare.HashCodeEqual(2, _xCount))
                ChangeFloor(selectionGridToggles);

            if (!compare.HashCodeEqual(3, spacing.GetHashCode()))
                IGUSelectionGridToggleChangeSpacing?.Invoke(spacing);

            if (!compare.HashCodeEqual(4, spacing.GetHashCode()))
                UseTooltipEvent?.Invoke(useTooltip);
        }

        public void SetSelectionGridToggleList(params string[] contents) {
            if (!ArrayManipulation.EmpytArray(contents))
                SetSelectionGridToggleList(Array.ConvertAll<string, IGUContent>(contents, (s) => new IGUContent(s)));
        }

        public void SetSelectionGridToggleList(params IGUContent[] contents) {
            DestroyToggleList(ref selectionGridToggles);
            PopulateEvents(selectionGridToggles = CreateSelectionGridToggleList(contents));
        }

        private IGUSelectionGridToggle[] CreateSelectionGridToggleList(params IGUContent[] contents) {
#if UNITY_EDITOR
            if (ArrayManipulation.EmpytArray(contents)) copyList = (IGUContent[])null;
            else contents = (IGUContent[])(copyList = contents).Clone();
#endif
            IGUSelectionGridToggle[] res = new IGUSelectionGridToggle[0];
            for (int I = 0, floorX = 0, floorY = 0; I < ArrayManipulation.ArrayLength(contents); I++, floorX++) {
                if (floorX >= _xCount) {
                    floorX = 0;
                    floorY += 1;
                }
                IGUSelectionGridToggle toggle = new IGUSelectionGridToggle(
                    $"(parent:{GetInstanceID()})IGUSelectionGridToggle#{Guid.NewGuid()}", I,
                    this, contents[I]
                    );
                toggle.ChangeFloor(new Vector2(floorX, floorY));
                toggle.ChangeSpacing(spacing);
                ArrayManipulation.Add(toggle, ref res);
            }
            return res;
        }

        private void ChangeFloor(IGUSelectionGridToggle[] toggles) {
            for (int I = 0, floorX = 0, floorY = 0; I < ArrayManipulation.ArrayLength(toggles); I++, floorX++) {
                if (floorX >= _xCount) {
                    floorX = 0;
                    floorY += 1;
                }
                toggles[I].ChangeFloor(new Vector2(floorX, floorY));
            }
        }

        private void PopulateEvents(IGUSelectionGridToggle[] toggles) {
            this.UseTooltipEvent = (Action<bool>)null;
            this.IGUSelectionGridToggleSelectedIndex = (Action<int>)null;
            this.IGUSelectionGridToggleChangeSpacing = (Action<Vector2>)null;
            this.IGUSelectionGridToggleChangeIGUColor = (Action<IGUColor>)null;
            this.IGUSelectionGridToggleOnIGU = (Action<IGUStyle, IGUStyle>)null;
            //this.IGUSelectionGridToggleChangeIGUConfig = (Action<IGUConfig>)null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(toggles); I++) {
                this.UseTooltipEvent += toggles[I].UseTooltip;
                this.IGUSelectionGridToggleOnIGU += toggles[I].OnIGU;
                this.IGUSelectionGridToggleSelectedIndex += toggles[I].SelectedIndex;
                this.IGUSelectionGridToggleSelectedIndex += toggles[I].SelectedIndex;
                this.IGUSelectionGridToggleChangeSpacing += toggles[I].ChangeSpacing;
                this.IGUSelectionGridToggleChangeIGUColor += toggles[I].ChangeIGUColor;
                //this.IGUSelectionGridToggleChangeIGUConfig += toggles[I].ChangeIGUConfig;
                toggles[I].ClickSelectionGridToggle(ChangeSelectedIndex);
            }
            ChangeSelectedIndex(selectedIndex);
        }

        private void ChangeSelectedIndex(int selectedIndex) {
            IGUSelectionGridToggleSelectedIndex?.Invoke(selectedIndex);
            onSelectedIndex?.Invoke(this.selectedIndex = selectedIndex);
        }

        private void DestroyToggleList(ref IGUSelectionGridToggle[] toggles) {
            if (!ArrayManipulation.EmpytArray(toggles)) {
                for (int I = 0; I < toggles.Length; I++)
                    toggles[I].OnDestroy();
                ArrayManipulation.ClearArraySafe(ref toggles);
            }
        }

        protected override void DestroyIgnition() {
            base.DestroyIgnition();
            DestroyToggleList(ref selectionGridToggles);
        }

        void IIGUSerializationCallbackReceiver.Reserialization() {
#if UNITY_EDITOR
            destroyList = selectionGridToggles;
            DestroyToggleList(ref destroyList);
            PopulateEvents(selectionGridToggles = CreateSelectionGridToggleList(copyList));
#endif
        }
    }
}
