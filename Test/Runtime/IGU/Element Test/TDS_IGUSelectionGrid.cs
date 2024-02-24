using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Layouts;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class TDS_IGUSelectionGrid : IGUObject {
        [SerializeField] protected int _xCount;
        [SerializeField] protected bool useTooltip;
        [SerializeField] protected Vector2 spacing;
        [SerializeField] protected int selectedIndex;
        [SerializeField] protected IGUGridLayout gridLayout;
        [SerializeField] protected IGUStyle tooltipToggleStype;
        [SerializeField] protected IGUStyle selectionGridToggleStyle;
        [SerializeField] protected IGUOnSliderIntValueEvent onSelectedIndex;
        private bool isIgnition;

        public int CheckButtonCount => gridLayout.Count;
        public Vector2 Spacing { get => spacing; set => spacing = value; }
        public IGUOnSliderIntValueEvent OnSelectedIndex => onSelectedIndex;
        public bool UseTooltip { get => useTooltip; set => useTooltip = value; }
        public int xCount { get => _xCount; set => _xCount = value < 1 ? 1 : value; }
        public int SelectedIndex { get => selectedIndex; set => SelectedIndexFunc(value); }
        public IGUStyle TooltipToggleStype { get => tooltipToggleStype; set => tooltipToggleStype = value; }
        public IGUStyle SelectionGridToggleStyle { get => selectionGridToggleStyle; set => selectionGridToggleStyle = value; }
        public Rect RectView => new Rect(
            myRect.ModifiedPosition,
            new Vector2(
                (myRect.Width + spacing.x) * xCount,
                (myRect.Height + spacing.y) * (gridLayout.Count / xCount)
                )
            );

        public TDS_IGUSelectionGridToggle this[int index] => gridLayout[index] as TDS_IGUSelectionGridToggle;

        protected override void Ignition() {
            base.Ignition();
            gridLayout = IGUObject.CreateIGUInstance<IGUGridLayout>($"[{name}]--{nameof(IGUGridLayout)}");
            gridLayout.DirectionalBreak = DirectionalBreak.HorizontalBreak;
            gridLayout.Parent = this;
            _xCount = 3;
            selectedIndex = -1;
            spacing = Vector2.one * 3f;
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            myRect = IGURect.DefaultSelectionGrid;
            onSelectedIndex = new IGUOnSliderIntValueEvent();
            for (int I = 0; I < 10; I++)
                Add($"Item[{I}]");
            isIgnition = true;
        }

        protected override void IgnitionEnable() {
            for (int I = 0; I < CheckButtonCount && !isIgnition; I++) {
                TDS_IGUSelectionGridToggle temp = gridLayout[I] as TDS_IGUSelectionGridToggle;
                temp.OnChecked.AddListener((b) => SetEvent(b, temp));
            }
            isIgnition = false;
        }

        protected override void LowCallOnIGU() {
            if (gridLayout.Spacing != spacing)
                gridLayout.Spacing = spacing;
            if (gridLayout.CellSize != myRect.Size)
                gridLayout.CellSize = myRect.Size;
            if (gridLayout.DirectionalCount != _xCount)
                gridLayout.DirectionalCount = _xCount;

            gridLayout.OnIGU();
        }

        public void Add(string text, Texture image, string toolTip) {
            TDS_IGUSelectionGridToggle button = 
                IGUObject.CreateIGUInstance<TDS_IGUSelectionGridToggle>($"Item[{CheckButtonCount}]");
            button.Text = text;
            button.Image = image;
            button.ToolTip = toolTip;
            button.UseTooltip = UseTooltip;
            button.Index = CheckButtonCount;
            button.Style = selectionGridToggleStyle;
            button.TooltipStyle = tooltipToggleStype;
            button.OnChecked.AddListener((b) => SetEvent(b, button));
            gridLayout.Add(button);
        }

        public void Add(params ValueTuple<string, Texture, string>[] itens) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(itens); I++)
                Add(itens[I].Item1, itens[I].Item2, itens[I].Item3);
        }

        public void Add(string text, Texture image)
            => Add(text, image, string.Empty);

        public void Add(Texture image, string toolTip)
            => Add(string.Empty, image, toolTip);

        public void Add(string text, string toolTip)
            => Add(text, (Texture)null, toolTip);

        public void Add(string text)
            => Add(text, string.Empty);

        public void Clear() => gridLayout.Clear(true);

        private void SelectedIndexFunc(int index) {
            if (index < 0 || index >= CheckButtonCount)
                throw new IndexOutOfRangeException();
            if (selectedIndex != index) return;

            if (selectedIndex > 0)
                this[selectedIndex].Select(false);
            this[selectedIndex = index].Select(true);
        }

        private void SetEvent(bool status, TDS_IGUSelectionGridToggle toggle) {
            //Debug.Log($"{status}|{toggle.Index}");
            if (!status) return;
            if (selectedIndex > 0)
                this[selectedIndex].Checked = false;
            onSelectedIndex.Invoke(selectedIndex = toggle.Index);
        }

        private static void RecursiveList(Action<TDS_IGUSelectionGridToggle, int> action, int index, IGUGridLayout gridLayout) {
            action(gridLayout[index] as TDS_IGUSelectionGridToggle, index++);
            if (index < gridLayout.Count)
                RecursiveList(action, index, gridLayout);
        }
    }
}