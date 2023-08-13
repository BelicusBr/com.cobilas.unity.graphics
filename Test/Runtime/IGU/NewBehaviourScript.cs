using System;
using System.IO;
using UnityEngine;
using System.Collections;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Test.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

public class NewBehaviourScript : MonoBehaviour, ISerializationCallbackReceiver {

    public Rect[] rects;
    private bool AfterDeserialize;

    private void Start() {
        for (int I = 0; I < 55; I++) {
            int ide = (int)(Randomico.value * 5f);
            ide = ide == 0 ? 1 : ide;

            Rect temp = Rect.zero;
            switch (ide) {
                case 1:
                    temp.size = new Vector2(130, 25);
                    break;
                case 2:
                    temp.size = new Vector2(130, 130);
                    break;
                case 3:
                    temp.size = new Vector2(50, 50);
                    break;
                case 4:
                    temp.size = new Vector2(200, 200);
                    break;
                case 5:
                    temp.size = new Vector2(300, 300);
                    break;
            }
            temp.x = Randomico.value * (Screen.width - temp.width);
            temp.y = Randomico.value * (Screen.height - temp.height);
            ArrayManipulation.Add(temp, ref rects);
        }
    }

    private void OnEnable()
    {
        if (AfterDeserialize) {
            StartCoroutine(WaitForEndOfFrame());
            AfterDeserialize = false;
        }
    }

    public int rot;
    int batataQuente = -1;
    private void OnGUI() {


        //int index = 0;
        //foreach (var item in rects) {
        //    //bool flag = false;
        //    if (Event.current.type == EventType.Layout)
        //        if (item.Contains(Event.current.mousePosition))
        //            batataQuente = index;

        //    Color old = GUI.color;
        //    GUI.color = batataQuente == index ? Color.green : Color.red;

        //    GUI.Box(item, $"Item:{index++}");

        //    GUI.color = old;
        //}
    }

    private IEnumerator WaitForEndOfFrame() {
        while (true) {
            yield return new WaitForEndOfFrame();
            batataQuente = -1;
        }
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        AfterDeserialize = true;
    }

    public class TDS_SeletorFisico {
    
    }

    public class TDS_Fisica {
        public Rect rect;
        public bool isHotPotato;
        public TDS_onGUI target;

        public bool CheckHotPotato(Vector2 mousePosition) {
            if (target != null && target.parent != null) {
                if (target.parent is TDS_onGUI && target.parent.MyPhysic.isHotPotato) {
                    Rect rectParent = target.parent.MyPhysic.rect;
                    rectParent.position = Vector2.zero;
                    float right = rect.xMax - rectParent.xMax;
                    float left = rectParent.xMin - rect.xMin;
                    float top = rectParent.yMin - rect.yMin;
                    float bottom = rect.yMax - rectParent.yMax;

                    rect.width -= Mathf.Clamp(left, 0, rect.width) + Mathf.Clamp(right, 0, rect.width);
                    rect.height -= Mathf.Clamp(top, 0, rect.height) + Mathf.Clamp(bottom, 0, rect.height);

                    return rect.Contains(mousePosition);
                }
                return false;
            }
            return rect.Contains(mousePosition);
        }
    }

    public abstract class TDS_onGUI {
        public abstract TDS_onGUI parent { get; set; }
        public abstract TDS_Fisica MyPhysic { get; set; }
        public abstract void onGUI();
    }

    public class TDS_Box : TDS_onGUI {
        public Rect rect;
        public override TDS_onGUI parent { get; set; }
        public override TDS_Fisica MyPhysic { get; set; }

        public TDS_Box()
        {
            MyPhysic = new TDS_Fisica();
        }

        public override void onGUI() {
            GUI.Box(rect, "Box 1");
        }
    }

    public class TDS_Clip : TDS_onGUI {
        public Rect rect;
        public Vector2 scrollView;
        public event Action _onGUI;
        public override TDS_onGUI parent { get; set; }
        public override TDS_Fisica MyPhysic { get; set; }

        public TDS_onGUI[] onGUIs;

        public TDS_Clip()
        {
            MyPhysic = new TDS_Fisica();
        }

        public override void onGUI() {
            GUI.BeginClip(rect, scrollView, Vector2.zero, false);
            _onGUI?.Invoke();
            GUI.EndClip();
        }
    }
}