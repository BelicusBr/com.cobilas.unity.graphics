using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;

public class IGU_TDS : MonoBehaviour {

    IGUHorizontalLayout horizontalLayout;
    // Start is called before the first frame update
    void Start()
    {
        horizontalLayout = IGUObject.CreateIGUInstance<IGUHorizontalLayout>("TDS");
        _ = horizontalLayout.ApplyToGenericContainer();

        horizontalLayout.Add(IGUObject.CreateIGUInstance<IGUButton>("BTR"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
