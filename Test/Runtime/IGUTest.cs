using System.Collections;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Packages.com.cobilas.unity.graphics.Test.Runtime
{
    public class IGUTest : MonoBehaviour
    {

        public IGUObject slider;

        // Use this for initialization
        void Start()
        {
            slider = IGUVerticalScrollbar.CreateIGUInstance("TDS_001");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}