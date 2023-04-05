using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUSelectionGridToggle {
        [SerializeField] private int index;
        [SerializeField] private Vector2 floor;
        [SerializeField] private Vector2 spacing;
        [SerializeField] private IGUCheckBox checkBox;

        public int Index => index;
        public IGUContent Content => checkBox.MyContent;

        public IGUSelectionGridToggle(string name, int index, IGUObject parent, IGUContent content) {
            checkBox = IGUObject.CreateIGUInstance<IGUCheckBox>(name);
            checkBox.MyContent = content;
            this.index = index;
            checkBox.Parent = parent;
        }

        public void OnDestroy()
            => UnityEngine.Object.Destroy(checkBox);

        public void OnIGU(GUIStyle style, GUIStyle tooltipStyle) {
            checkBox.MyRect = checkBox.MyRect.SetPosition(checkBox.Parent.MyRect.Size.Multiplication(floor) + spacing.Multiplication(floor));
            checkBox.MyRect = checkBox.MyRect.SetSize(checkBox.Parent.MyRect.Size);
            checkBox.CheckBoxStyle = style;
            checkBox.TooltipStyle = tooltipStyle;
            checkBox.OnIGU();
        }

        public void ClickSelectionGridToggle(Action<int> selected) {
            checkBox.OnChecked.AddListener((b) => {
                if (b) selected(index);
            });
        }

        public void UseTooltip(bool status)
            => checkBox.UseTooltip = status;

        public void SelectedIndex(int selectedIndex)
            => checkBox.Checked = index == selectedIndex;

        public void ChangeFloor(Vector2 floor)
            => this.floor = floor;

        public void ChangeSpacing(Vector2 spacing)
            => this.spacing = spacing;

        public void ChangeIGUColor(IGUColor color)
            => checkBox.MyColor = color;

        //public void ChangeIGUConfig(IGUConfig config)
        //    => checkBox.MyConfg = config;
    }
}
