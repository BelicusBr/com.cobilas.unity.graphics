using System.Collections;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.Resolutions;

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

        public IGUObject slider1;
        public IGUObject slider2;
        public IGUObject slider3;
        public IGUObject slider4;
        public IGUObject slider5;
        public IGUObject slider6;
        public IGUObject slider7;
        public IGUObject slider8;
        public IGUObject slider9;
        public IGUObject slider10;
        public IGUObject slider11;

        // Use this for initialization
        void Start()
        {
            print(CobilasResolutions.ResolutionsCount);
            //slider1  = IGUButton.CreateIGUInstance("TDS_001");
            //slider2  = IGUCheckBox.CreateIGUInstance("TDS_002");
            //slider3  = IGUComboBox.CreateIGUInstance("TDS_003");
            //slider4  = IGULabel.CreateIGUInstance("TDS_004");
            //slider5  = IGUNumericBox.CreateIGUInstance("TDS_005");
            //slider6  = IGUNumericBoxInt.CreateIGUInstance("TDS_006");
            //slider7  = IGUPictureBox.CreateIGUInstance("TDS_007");
            //slider8  = IGURepeatButton.CreateIGUInstance("TDS_008");
            //slider9  = IGUScrollView.CreateIGUInstance("TDS_009");
            //slider10 = IGUSelectionGrid.CreateIGUInstance("TDS_010");
            //slider11 = IGUWindow.CreateIGUInstance("TDS_011");

            //IGUContainer.CreateGenericIGUContainer().Add(slider3);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}