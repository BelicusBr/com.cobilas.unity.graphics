using UnityEngine;
using System.Collections;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.Resolutions;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Packages.com.cobilas.unity.graphics.Test.Runtime
{
    public class IGUTest : MonoBehaviour
    {
        /*
         - Add IGUButton.cs
         - Add IGUCheckBox.cs
         - Add IGUComboBox.cs
         - Add IGULabel.cs
         - Add IGUNumericBox.cs
         - Add IGUNumericBoxInt.cs
         - Add IGUPictureBox.cs
         - Add IGURepeatButton.cs
         - Add IGUScrollView.cs
         - Add IGUSelectionGrid.cs
         - Add IGUWindow.cs
         */
        public Vector2 pos1, pos2;
        public bool clek;
        public bool useCellSize = true;
        public int icount;
        public int breakdir;
        public IGUVerticalLayout layout;
        //public IGUObject slider1;
        //public IGUObject slider2;
        //public IGUObject slider3;
        //public IGUObject slider4;
        //public IGUObject slider5;
        //public IGUObject slider6;
        //public IGUObject slider7;
        //public IGUObject slider8;
        //public IGUObject slider9;
        //public IGUObject slider10;
        //public IGUObject slider11;
        //public IGUObject slider12;
        //public IGUObject slider13;
        //public IGUObject slider14;
        //public IGUObject slider15;
        //public IGUObject slider16;

        // Use this for initialization
        void Start()
        {

            layout = IGUVerticalLayout.CreateIGUInstance("ly-layout");
            IGUHorizontalLayout hz_layout;
            _ = layout.Add(IGUButton.CreateIGUInstance("bt-button 1"));
            _ = layout.Add(IGUButton.CreateIGUInstance("bt-button 2"));
            _ = layout.Add(hz_layout = IGUHorizontalLayout.CreateIGUInstance("ly-hz1_layout"));
            _ = hz_layout.Add(IGUButton.CreateIGUInstance("hz1-bt-button 1"));
            _ = hz_layout.Add(IGUButton.CreateIGUInstance("hz1-bt-button 2"));
            _ = layout.Add(hz_layout = IGUHorizontalLayout.CreateIGUInstance("ly-hz2_layout"));
            _ = hz_layout.Add(IGUButton.CreateIGUInstance("hz2-bt-button 1"));
            _ = hz_layout.Add(IGUButton.CreateIGUInstance("hz2-bt-button 2"));
            _ = hz_layout.Add(IGUButton.CreateIGUInstance("hz2-bt-button 3"));
            _ = layout.ApplyToGenericContainer();

            //slider1 = IGUButton.CreateIGUInstance("TDS_001");
            //slider2 = IGUCheckBox.CreateIGUInstance("TDS_002");
            //slider3 = IGUComboBox.CreateIGUInstance("TDS_003");
            //slider4 = IGULabel.CreateIGUInstance("TDS_004");
            //slider5 = IGUNumericBox.CreateIGUInstance("TDS_005");
            //slider6 = IGUNumericBoxInt.CreateIGUInstance("TDS_006");
            //slider7 = IGUPictureBox.CreateIGUInstance("TDS_007");
            //slider8 = IGURepeatButton.CreateIGUInstance("TDS_008");
            //slider9 = IGUScrollView.CreateIGUInstance("TDS_009");
            //slider10 = IGUSelectionGrid.CreateIGUInstance("TDS_010");
            //slider11 = IGUWindow.CreateIGUInstance("TDS_011");
            //slider12 = IGUTextField.CreateIGUInstance("TDS_012");
            //slider13 = IGUPasswordField.CreateIGUInstance("TDS_013");
            //slider14 = IGUHorizontalSlider.CreateIGUInstance("TDS_014");
            //slider15 = IGUHorizontalScrollbar.CreateIGUInstance("TDS_015");
            //slider16 = IGUSelectableText.CreateIGUInstance("TDS_016");

            //slider1.ApplyToGenericContainer();
            //slider2.ApplyToGenericContainer();
            //slider3.ApplyToGenericContainer();
            //slider4.ApplyToGenericContainer();
            //slider5.ApplyToGenericContainer();
            //slider6.ApplyToGenericContainer();
            //slider7.ApplyToGenericContainer();
            //slider8.ApplyToGenericContainer();
            //slider9.ApplyToGenericContainer();
            //slider10.ApplyToGenericContainer();
            //slider11.ApplyToGenericContainer();
            //slider12.ApplyToGenericContainer();
            //slider13.ApplyToGenericContainer();
            //slider14.ApplyToGenericContainer();
            //slider15.ApplyToGenericContainer();
            //slider16.ApplyToGenericContainer();
            //IGUContainer.CreateGenericIGUContainer().Add(slider1);
        }

        private void Update()
        {
            //Debug.Log(pos2 - pos1);
            icount = icount < 1 ? 1 : icount;
            breakdir = breakdir < 1 ? 1 : breakdir;
            //box.MyRect = box.MyRect.SetSize(layout.MyRect.Size);
        }

        private void OnGUI() {
            //Event current = IGUDrawer.IGUEvent;
            //switch (current.type) {
            //    case EventType.KeyDown:
            //        Debug.Log("D:" + current.keyCode);
            //        Debug.Log("D:" + current.character);
            //        break;
            //    case EventType.KeyUp:
            //        Debug.Log("U:" + current.keyCode);
            //        break;
            //}
        }

        public void Print(string txt)
            => print(txt);
    }
}