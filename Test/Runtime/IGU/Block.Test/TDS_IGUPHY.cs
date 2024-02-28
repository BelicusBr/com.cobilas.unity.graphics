using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Physics;
using Test.Runtime.IGU.IGUObjects;
using UnityEngine;

namespace Test.Runtime.IGU.Block.Test {
    public class TDS_IGUPHY : MonoBehaviour {
        public Vector2 mouse;
        public TDSIGUPhysicsTemp temp1;
        public TDSIGUPhysicsTemp temp2;
        public TDSIGUPhysicsTemp temp3;

        private delegate bool ResPhy(Vector2 mouse, out IGUPhysicsBase result);
        private event ResPhy getPhy;

        private void Awake() {
            temp1 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY001");
            temp2 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY002");
            temp3 = IGUObject.Create<TDSIGUPhysicsTemp>("#TDSPHY003");
        }

        private void OnEnable() {
            getPhy += (Vector2 m, out IGUPhysicsBase r) => {
                r = null;
                if (temp1.Physics.CollisionConfirmed(m)) {
                    temp1.Physics.IsHotPotato = false;
                    r = temp1.Physics;
                    return true;
                }
                return false;
            };
            getPhy += (Vector2 m, out IGUPhysicsBase r) => {
                r = null;
                if (temp2.Physics.CollisionConfirmed(m)) {
                    temp2.Physics.IsHotPotato = false;
                    r = temp2.Physics;
                    return true;
                }
                return false;
            };
            getPhy += (Vector2 m, out IGUPhysicsBase r) => {
                r = null;
                if (temp3.Physics.CollisionConfirmed(m)) {
                    temp3.Physics.IsHotPotato = false;
                    r = temp3.Physics;
                    return true;
                }
                return false;
            };
        }

        private void OnGUI() {
            IGUPhysicsBase temp = null;
            mouse = (Input.mousePosition - Vector3.up * Screen.height).InvertY();
            temp1._Physics.MyRect = temp1.MyRect;
            temp2._Physics.MyRect = temp2.MyRect;
            temp3._Physics.MyRect = temp3.MyRect;
            temp1._Physics.IsHotPotato = false;
            temp2._Physics.IsHotPotato = false;
            temp3._Physics.IsHotPotato = false;

            if (temp1.Physics.CollisionConfirmed(mouse)) {
                Debug.Log(temp1.name);
                temp = temp1.Physics;
            }
            if (temp2.Physics.CollisionConfirmed(mouse)) {
                Debug.Log(temp2.name);
                temp = temp2.Physics;
            }
            if (temp3.Physics.CollisionConfirmed(mouse)) {
                Debug.Log(temp3.name);
                temp = temp3.Physics;
            }
            if (temp != null) temp.IsHotPotato = true;

            temp1.MyConfig = temp1.MyConfig.SetEnabled(temp1._Physics.IsHotPotato);
            temp2.MyConfig = temp2.MyConfig.SetEnabled(temp2._Physics.IsHotPotato);
            temp3.MyConfig = temp3.MyConfig.SetEnabled(temp3._Physics.IsHotPotato);

            temp1.OnIGU();
            temp2.OnIGU();
            temp3.OnIGU();
        }


    }
}