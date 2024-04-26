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
    public class IGUSelectionGrid : IGUObject, IIGUToolTip, IEnumerable<IGUSelectionGridToggle> {
        private Action onToolTip;
        [SerializeField] protected int _xCount;
        [SerializeField] protected bool useTooltip;
        [SerializeField] protected Vector2 spacing;
        [SerializeField] protected int selectedIndex;
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected IGUGridLayout gridLayout;
        [SerializeField] protected IGUStyle tooltipToggleStype;
        [SerializeField] protected IGUStyle selectionGridToggleStyle;
        protected IGUBasicPhysics.CallPhysicsFeedback callPhysicsFeedback;
        [SerializeField] protected IGUOnSliderIntValueEvent onSelectedIndex;

        public int ToggleCount => gridLayout.Count;
        public Vector2 Spacing { get => spacing; set => spacing = value; }
        public IGUOnSliderIntValueEvent OnSelectedIndex => onSelectedIndex;
        public int xCount { get => _xCount; set => _xCount = value < 1 ? 1 : value; }
        public int SelectedIndex { get => selectedIndex; set => SelectedIndexFunc(value); }
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }
        public IGUStyle TooltipStyle { get => tooltipToggleStype; set => tooltipToggleStype = value; }
        public IGUStyle SelectionGridToggleStyle { get => selectionGridToggleStyle; set => selectionGridToggleStyle = value; }
        public bool UseTooltip { 
            get => useTooltip;
            set {
                if (useTooltip != (useTooltip = value))
                    RecursiveList((t, i) => { t.UseTooltip = value; }, 0, gridLayout);
            }
        }
        public Rect RectView => new Rect(
            myRect.Position,
            new Vector2(
                (myRect.Width + spacing.x) * xCount,
                (myRect.Height + spacing.y) * (gridLayout.Count / xCount)
                )
            );
        string IIGUToolTip.ToolTip { 
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public IGUSelectionGridToggle this[int index] => gridLayout[index] as IGUSelectionGridToggle;

        protected override void IGUAwake() {
            base.IGUAwake();
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this, true);
            gridLayout = IGUObject.Create<IGUGridLayout>($"[{name}]--{nameof(IGUGridLayout)}");
            gridLayout.DirectionalBreak = DirectionalBreak.HorizontalBreak;
            gridLayout.Parent = this;
            _xCount = 3;
            selectedIndex = -1;
            spacing = Vector2.one * 3f;
            myColor = IGUColor.DefaultBoxColor;
            myRect = IGURect.DefaultSelectionGrid;
            onSelectedIndex = new IGUOnSliderIntValueEvent();
            for (int I = 0; I < 10; I++)
                Add($"Item[{I}]");
        }

        protected override void IGUOnEnable() {
            onToolTip = (Action)null;
            callPhysicsFeedback = (IGUBasicPhysics.CallPhysicsFeedback)null;
            for (int I = 0; I < ToggleCount; I++) {
                IGUSelectionGridToggle temp = gridLayout[I] as IGUSelectionGridToggle;
                onToolTip += (temp as IIGUToolTip).InternalDrawToolTip;
                callPhysicsFeedback += (temp as IIGUPhysics).CallPhysicsFeedback;
                temp.OnChecked.RemoveAllListeners();
                temp.OnChecked.AddListener((b) => SetEvent(b, temp));
            }
        }

        protected override void LowCallOnIGU() {
            if (Event.current.type == EventType.Layout) {
                if (gridLayout.Spacing != spacing)
                    gridLayout.Spacing = spacing;
                if (gridLayout.CellSize != myRect.Size)
                    gridLayout.CellSize = myRect.Size;
                if (gridLayout.DirectionalCount != _xCount)
                    gridLayout.DirectionalCount = _xCount;
            }

            gridLayout.OnIGU();
        }

        public void Add(string text, Texture image, string toolTip) {
            IGUSelectionGridToggle button = 
                IGUObject.Create<IGUSelectionGridToggle>($"Item[{ToggleCount}]");
            button.Text = text;
            button.Image = image;
            button.ToolTip = toolTip;
            button.UseTooltip = UseTooltip;
            button.Index = ToggleCount;
            button.Style = selectionGridToggleStyle;
            button.TooltipStyle = tooltipToggleStype;
            onToolTip += (button as IIGUToolTip).InternalDrawToolTip;
            callPhysicsFeedback += (button as IIGUPhysics).CallPhysicsFeedback;
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

        public void Remove(int index) {
            if (gridLayout.Remove(index, true))
                if (ToggleCount != 0) {
                    onToolTip = (Action)null;
                    callPhysicsFeedback = (IGUBasicPhysics.CallPhysicsFeedback)null;
                    RecursiveList((c, i) => { 
                        c.Index = i;
                        onToolTip += (c as IIGUToolTip).InternalDrawToolTip;
                        callPhysicsFeedback += (c as IIGUPhysics).CallPhysicsFeedback;
                    }, 0, gridLayout);
                }
        }

        public void Clear() => gridLayout.Clear(true);

        public IEnumerator<IGUSelectionGridToggle> GetEnumerator() {
            for (int I = 0; I < ToggleCount; I++)
                yield return this[I];
        }

        protected override void InternalCallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys)
            => callPhysicsFeedback?.Invoke(mouse, ref phys);

        private void SelectedIndexFunc(int index) {
            if (index < 0 || index >= ToggleCount)
                throw new IndexOutOfRangeException();
            if (selectedIndex == index) return;

            this[index].Checked = true;
        }

        private void SetEvent(bool status, IGUSelectionGridToggle toggle) {
            if (!status) return;
            if (selectedIndex >= 0)
                this[selectedIndex].Checked = false;
            onSelectedIndex.Invoke(selectedIndex = toggle.Index);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            for (int I = 0; I < ToggleCount; I++)
                yield return this[I];
        }

        void IIGUToolTip.InternalDrawToolTip() => onToolTip?.Invoke();

        private static void RecursiveList(Action<IGUSelectionGridToggle, int> action, int index, IGUGridLayout gridLayout) {
            action(gridLayout[index] as IGUSelectionGridToggle, index++);
            if (index < gridLayout.Count)
                RecursiveList(action, index, gridLayout);
        }
    }
}