using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public class IGUComboBoxButton {
        [SerializeField] private int index;
        [SerializeField] private IGUButton button;

        public int Index => index;
        public IGUContent Content => button.MyContent;

        public IGUComboBoxButton(int index, int parentInstanceID, IGUContent content) {
            this.button = IGUObject.CreateIGUInstance<IGUButton>($"({parentInstanceID})CBXBT[index:{this.index = index}]");
            this.button.MyContent = content;
        }
            //=> this.button = IGUButton.CreateIGUInstance($"({parentInstanceID})CBXBT[index:{this.index = index}]", content);

        public void OnIGU(IGUStyle style, IGUScrollView sv) {
            button.ButtonStyle = style;
            IGURect rect = button.MyRect = button.MyRect.SetPosition(0, button.MyRect.Height * index);
            rect.SetPosition(rect.X, rect.Y - sv.ScrollPosition.y);
            if ((rect.Up > 0 || rect.Donw > 0) &&
                (rect.Up < sv.MyRect.Height || rect.Donw < sv.MyRect.Height))
                button.OnIGU();
        }

        public void ChangeWidth(float width)
            => ChangeSize(width, button.MyRect.Height);

        public void ChangeHeight(float height)
            => ChangeSize(button.MyRect.Width, height);

        public void ChangeIGUColor(IGUColor color)
            => button.MyColor = color;

        public void ChangeIGUConfig(IGUConfig config)
            => button.MyConfg = config;

        public void OnDestroy()
            => UnityEngine.Object.Destroy(button);

        public void AddSelectedIndex(Action<IGUComboBoxButton> selectedIndex)
            => button.OnClick.AddListener(() => { selectedIndex(this); });

        public void AddOnClick(Action onClick)
            => button.OnClick.AddListener(() => onClick());

        private void ChangeSize(float width, float height)
            => button.MyRect = button.MyRect.SetSize(width, height);
    }
}
