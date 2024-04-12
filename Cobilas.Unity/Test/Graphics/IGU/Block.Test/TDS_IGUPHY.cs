using System;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using Cobilas.Unity.Test.Graphics.IGU.Elements;
using Cobilas.Unity.Test.Graphics.IGU;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Physics;

public class TDS_IGUPHY : MonoBehaviour, ISerializationCallbackReceiver {
    public Vector2 mouse;

    // public TDSIGUPhysicsTemp temp1;
    // public TDSIGUPhysicsTemp temp2;
    // public TDSIGUPhysicsTemp temp3;

    // public TDSIGUWindow temp4;
    // public TDSIGUPhysicsTemp temp5;
    // public TDSIGUPhysicsTemp temp6;
    // public TDSIGUPhysicsTemp temp7;
    // public TDSIGUPhysicsTemp temp8;
    private event Action<Vector2, List<IGUBasicPhysics>> callPhy;

    private void Awake() {

        // temp1 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY001");
        // temp2 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY002");
        // temp3 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY003");
        // temp1.MyRect = temp1.MyRect.SetPosition(Vector2.right * 180f);
        // temp2.MyRect = temp2.MyRect.SetPosition(Vector2.right * 195f + Vector2.up * (temp1.MyRect.Donw - temp1.MyRect.Height * .5f));
        // temp3.MyRect = temp3.MyRect.SetPosition(Vector2.right * 215f + Vector2.up * (temp2.MyRect.Donw - temp2.MyRect.Height * .5f));

        // temp4 = IGUObject.Create<TDSIGUWindow>("#TDSPHY004");
        // temp4.MyRect = temp4.MyRect.SetPosition(512f, 25f);

        // temp5 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY005");
        // temp6 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY006");
        // temp7 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY007");
        // temp5.MyRect = temp5.MyRect.SetPosition(Vector2.right * 25f);
        // temp6.MyRect = temp6.MyRect.SetPosition(Vector2.right * 45f + Vector2.up * (temp5.MyRect.Donw - temp5.MyRect.Height * .5f));
        // temp7.MyRect = temp7.MyRect.SetPosition(Vector2.one * 130f);

        // temp8 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY008");
        // temp8.MyRect = temp8.MyRect.SetPosition(Vector2.right * 380f).SetRotation(45f);

        // temp5.Parent = temp6.Parent = temp7.Parent = temp4;
        // bool ok = false;
        // ok = ((temp4 as IIGUPhysics).Physics as IGUMultiPhysics).Add(temp5.Physics);
        // Debug.Log($"[{ok}]temp5");
        // ok = ((temp4 as IIGUPhysics).Physics as IGUMultiPhysics).Add(temp6.Physics);
        // Debug.Log($"[{ok}]temp6");
        // ok = ((temp4 as IIGUPhysics).Physics as IGUMultiPhysics).Add(temp7.Physics);
        // Debug.Log($"[{ok}]temp7");
    }
    
    private void OnEnable() {
        // callPhy += (temp1 as IIGUPhysics).CallPhysicsFeedback;
        // callPhy += (temp2 as IIGUPhysics).CallPhysicsFeedback;
        // callPhy += (temp3 as IIGUPhysics).CallPhysicsFeedback;
        // callPhy += (temp4 as IIGUPhysics).CallPhysicsFeedback;
        // callPhy += (temp8 as IIGUPhysics).CallPhysicsFeedback;
        // foreach (var item in ((temp4 as IIGUPhysics).Physics as IGUMultiPhysics).SubPhysics)
        //     callPhy += (item.Target as IIGUPhysics).CallPhysicsFeedback;
        // temp4.windowFunction += (id) => {
        //     temp5.OnIGU();
        //     temp6.OnIGU();
        //     temp7.OnIGU();
        // };
        // result = new List<IGUBasicPhysics>(1);
        // result.Add(null);
    }
    
    List<IGUBasicPhysics> result;
    private void OnGUI() {
        // mouse = Event.current.mousePosition;

        // callPhy(mouse, result);
        // if (result[0] != null) {
        //     result[0].IsHotPotato = true;
        //     Debug.Log(result[0].Target.name);
        // }
        // temp1.OnIGU();
        // temp2.OnIGU();
        // temp3.OnIGU();
        // temp4.OnIGU();
        // temp8.OnIGU();
        // result[0] = null;
    }

    private void OnDrawGizmos() {
        // temp1.OnDrawGizmos();
        // temp2.OnDrawGizmos();
        // temp3.OnDrawGizmos();
        // temp8.OnDrawGizmos();
        //temp4.OnDrawGizmos();
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        Debug.Log("OnBeforeSerialize");
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Debug.Log("OnAfterDeserialize");
    }
}