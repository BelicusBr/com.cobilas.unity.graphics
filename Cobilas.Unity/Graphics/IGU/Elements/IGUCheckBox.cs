using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUCheckBox : IGUTextObject {
        public const string DefaultContantIGUCheckBox = "IGU check box";

        [SerializeField] private bool onclicked;
        [SerializeField] protected bool _checked;
        [SerializeField] protected bool checkedtemp;
        [SerializeField] protected IGUStyle checkBoxStyle;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUBoxPhysics boxPhysics;
        [SerializeField] protected IGUOnClickEvent checkBoxOn;
        [SerializeField] protected IGUOnClickEvent checkBoxOff;
        [SerializeField] protected IGUOnCheckedEvent onChecked;

        public IGUOnClickEvent OnClick => onClick;
        public IGUOnCheckedEvent OnChecked => onChecked;
        public IGUOnClickEvent CheckBoxOn => checkBoxOn;
        public IGUOnClickEvent CheckBoxOff => checkBoxOff;
        public IGUStyle CheckBoxStyle { 
            get => checkBoxStyle;
            set => checkBoxStyle = value ?? (IGUStyle)"Black toggle border";
        }
        public bool Checked { 
            get => _checked;
            set => onChecked.Invoke(_checked = checkedtemp = value);
        }
        public override IGUBasicPhysics Physics { get => boxPhysics; set => boxPhysics = (IGUBoxPhysics)value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            checkBoxOn = new IGUOnClickEvent();
            checkBoxOff = new IGUOnClickEvent();
            onChecked = new IGUOnCheckedEvent();
            boxPhysics = new IGUBoxPhysics(this);
            onclicked = checkedtemp = _checked = false;
            checkBoxStyle = (IGUStyle)"Black toggle border";
            content = new IGUContent(DefaultContantIGUCheckBox);
        }

        protected override void LowCallOnIGU() {
            checkBoxStyle.RichText = richText;
            checkedtemp = BackEndIGU.Toggle(LocalRect, checkedtemp, MyContent, checkBoxStyle,
                IGUNonePhysics.None, GetInstanceID(), out bool onclickedTemp);

            Event @event = Event.current;
            bool isRect = LocalRect.Contains(IGUDrawer.MousePosition);
            // if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType))
            //     onclicked = true;
            if (@event.type == EventType.MouseDown)
                if (@event.button == (int)LocalConfig.MouseType)
                    onclicked = onclickedTemp;
            if (@event.type == EventType.Repaint)
                if (isRect && _checked != checkedtemp && onclicked) {
                    // onclicked = false;
                    onClick.Invoke();
                    onChecked.Invoke(_checked = checkedtemp);
                }

            if (_checked) checkBoxOn.Invoke();
            else checkBoxOff.Invoke();
        }
    }
}
