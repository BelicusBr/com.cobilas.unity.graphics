using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using Cobilas.Unity.Test.Graphics.IGU.Elements;
using Cobilas.Unity.Test.Graphics.IGU.Interfaces;
using Cobilas.Unity.Test.Graphics.IGU.Physics;
using UnityEngine;

public class TDS_IGUPHY : MonoBehaviour {
    public Vector2 mouse;
    public TDSIGUPhysicsTemp temp1;
    public TDSIGUPhysicsTemp temp2;
    public TDSIGUPhysicsTemp temp3;
    private event Action<Vector2, List<IGUPhysicsBase>> callPhy;

    private void Awake() {
        temp1 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY001");
        temp2 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY002");
        temp3 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY003");
        temp1.MyRect = temp1.MyRect.SetPosition(Vector2.right * 180f);
        temp2.MyRect = temp2.MyRect.SetPosition(Vector2.right * 195f + Vector2.up * (temp1.MyRect.Donw - temp1.MyRect.Height * .5f));
        temp3.MyRect = temp3.MyRect.SetPosition(Vector2.right * 215f + Vector2.up * (temp2.MyRect.Donw - temp2.MyRect.Height * .5f));
    }

    private void OnEnable() {
        callPhy += (temp1 as IIGUPhysics).CallPhysicsFeedback;
        callPhy += (temp2 as IIGUPhysics).CallPhysicsFeedback;
        callPhy += (temp3 as IIGUPhysics).CallPhysicsFeedback;
        result = new List<IGUPhysicsBase>(1);
        result.Add(null);
    }
    
    List<IGUPhysicsBase> result;
    private void OnGUI() {
        mouse = (Input.mousePosition - Vector3.up * Screen.height).InvertY();
        callPhy(mouse, result);
        if (result[0] != null) result[0].IsHotPotato = true;
        temp1.OnIGU();
        temp2.OnIGU();
        temp3.OnIGU();
    }
}