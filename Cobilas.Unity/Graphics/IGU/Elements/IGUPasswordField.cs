using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUPasswordField : IGUTextFieldObject {

        [SerializeField] protected int maxLength;
        [SerializeField] protected char maskChar;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected IGUStyle passwordFieldStyle;
        [SerializeField, HideInInspector] protected bool isFocused;
        [SerializeField] protected IGUTextFieldStringEvent onStringChanged;

        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
        public IGUTextFieldStringEvent OnStringChanged => onStringChanged;
        public char MaskChar { get => maskChar; set => maskChar = value; }
        public int MaxLength { get => maxLength; set => maxLength = value; }
        public IGUStyle PasswordFieldStyle { 
            get => passwordFieldStyle;
            set => passwordFieldStyle = value ?? (IGUStyle)"Black text field border";
        }
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            maskChar = '*';
            maxLength = 50000;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            myColor = IGUColor.DefaultBoxColor;
            onStringChanged = new IGUTextFieldStringEvent();
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
            passwordFieldStyle = (IGUStyle)"Black text field border";
        }

        protected override void LowCallOnIGU() {

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);

            IGUContent contentTemp = GetIGUContentTemp(MyContent.Text, MyContent.Image, MyContent.Tooltip);

            BackEndIGU.PasswordField(LocalRect, contentTemp, GetInstanceID(), maxLength,
                maskChar, physics, passwordFieldStyle, ref isFocused);

            SetGUISettings(oldSettings);
            Event current = Event.current;

            if (LocalRect.Contains(current.mousePosition))
                if (IGUDrawer.GetMouseButtonDown(LocalConfig.MouseType) && isFocused)
                    onClick.Invoke();

            if (contentTemp != MyContent && isFocused)
                onStringChanged.Invoke((MyContent = contentTemp).Text);
        }

        protected override void SetGUISettings(GUISettings settings)
            => base.SetGUISettings(settings);

        protected override void SetGUISettings(IGUTextSettings settings)
            => base.SetGUISettings(settings);
    }
}
