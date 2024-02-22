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

        public int CheckButtonCount => gridLayout.Count;
        public Vector2 Spacing { get => spacing; set => spacing = value; }
        public IGUOnSliderIntValueEvent OnSelectedIndex => onSelectedIndex;
        public bool UseTooltip { get => useTooltip; set => useTooltip = value; }
        public int xCount { get => _xCount; set => _xCount = value < 1 ? 1 : value; }
        public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }
        public IGUStyle TooltipToggleStype { get => tooltipToggleStype; set => tooltipToggleStype = value; }
        public IGUStyle SelectionGridToggleStyle { get => selectionGridToggleStyle; set => selectionGridToggleStyle = value; }
        public Rect RectView => new Rect(
            myRect.ModifiedPosition,
            new Vector2(
                (myRect.Width + spacing.x) * xCount,
                (myRect.Height + spacing.y) * (gridLayout.Count / xCount)
                )
            );

        protected override void Ignition() {
            base.Ignition();
            gridLayout = IGUObject.CreateIGUInstance<IGUGridLayout>($"[{name}]--{nameof(IGUGridLayout)}");
            gridLayout.DirectionalBreak = DirectionalBreak.HorizontalBreak;
            gridLayout.Parent = this;
            _xCount = 3;
            spacing = Vector2.one * 3f;
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            myRect = IGURect.DefaultSelectionGrid;
            onSelectedIndex = new IGUOnSliderIntValueEvent();
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
            TDS_IGUComboBoxButton button = 
                IGUObject.CreateIGUInstance<TDS_IGUComboBoxButton>($"Item[{CheckButtonCount}]");
            button.Text = text;
            button.Image = image;
            button.ToolTip = toolTip;
            button.UseTooltip = UseTooltip;
            button.Index = CheckButtonCount;
            button.Style = selectionGridToggleStyle;
            button.TooltipStyle = tooltipToggleStype;
            gridLayout.Add(button);
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
            if (gridLayout.Remove(index, true))
                if (CheckButtonCount != 0) {}
                    //RecursiveList((c, i) => { c.Index = i; }, 0, cbx_verticalLayout);
        }

        public void Clear() => gridLayout.Clear(true);
    }
}