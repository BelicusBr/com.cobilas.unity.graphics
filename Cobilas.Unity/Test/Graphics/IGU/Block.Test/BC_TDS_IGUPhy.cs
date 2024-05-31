using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

using UEObject = UnityEngine.Object;

[Serializable]
public class BC_TDS_IGUPhy {
    public IGUButton bt1, bt2, bt3, bt4, bt5;
    public IGUCheckBox ckb1, ckb2, ckb3, ckb4;
    public IGUComboBox cbx1;
    public IGUScrollView scv1;
    public IGUBox box1, box2, box3;
    public IGUSelectionGrid stg1;
    public IGUWindow win1;
    public IGUTextField txtf1;

    public void Start() {
        bt1 = IGUObject.Create<IGUButton>("#[TDS:001]BT");
        bt2 = IGUObject.Create<IGUButton>("#[TDS:002]BT");
        bt3 = IGUObject.Create<IGUButton>("#[TDS:003]BT");
        bt4 = IGUObject.Create<IGUButton>("#[TDS:004]BT");
        bt5 = IGUObject.Create<IGUButton>("#[TDS:005]BT");

        cbx1 = IGUObject.Create<IGUComboBox>("#[TDS:001]CBX");

        ckb1 = IGUObject.Create<IGUCheckBox>("#[TDS:001]CKB");
        ckb2 = IGUObject.Create<IGUCheckBox>("#[TDS:002]CKB");
        ckb3 = IGUObject.Create<IGUCheckBox>("#[TDS:003]CKB");
        ckb4 = IGUObject.Create<IGUCheckBox>("#[TDS:004]CKB");

        box1 = IGUObject.Create<IGUBox>("#[TDS:001]BX");
        box2 = IGUObject.Create<IGUBox>("#[TDS:002]BX");

        scv1 = IGUObject.Create<IGUScrollView>("#[TDS:001]SCV");

        bt1.Text = bt1.name;
        bt2.Text = bt2.name;
        bt3.Text = bt3.name;
        bt4.Text = bt4.name;
        bt5.Text = bt5.name;

        ckb1.Text = ckb1.name;
        ckb2.Text = ckb2.name;
        ckb3.Text = ckb3.name;
        ckb4.Text = ckb4.name;

        box1.Text = box1.name;
        box2.Text = box2.name;

        box2.Parent = scv1;
        _ = scv1.AddOtherPhysics(box2);

        IGUCanvas container = bt1.ApplyToGenericContainer();
        _ = bt2.ApplyToContainer(container);
        _ = bt3.ApplyToContainer(container);
        _ = bt4.ApplyToContainer(container);
        _ = bt5.ApplyToContainer(container);
        
        _ = cbx1.ApplyToContainer(container);

        _ = ckb1.ApplyToContainer(container);
        _ = ckb2.ApplyToContainer(container);
        _ = ckb3.ApplyToContainer(container);
        _ = ckb4.ApplyToContainer(container);

        _ = box1.ApplyToContainer(container);
        //_ = box2.ApplyToContainer(container);

        _ = scv1.ApplyToContainer(container);


        bt1.MyRect = bt1.MyRect.SetPosition(Vector2.right * 180f);
        bt2.MyRect = bt2.MyRect.SetPosition(Vector2.right * 190f + Vector2.up * 12.5f);
        bt3.MyRect = bt3.MyRect.SetPosition(Vector2.right * 200f + Vector2.up * 25f);

        ckb1.MyRect = ckb1.MyRect.SetPosition(Vector2.right * 360f);
        ckb2.MyRect = ckb2.MyRect.SetPosition(Vector2.right * 370f + Vector2.up * 12.5f);
        ckb3.MyRect = ckb3.MyRect.SetPosition(Vector2.right * 380f + Vector2.up * 25f);

        bt5.MyRect = bt5.MyRect.SetPosition(Vector2.right * 550f);
        box1.MyRect = box1.MyRect.SetPosition(Vector2.right * 550f);
        ckb4.MyRect = ckb4.MyRect.SetPosition(Vector2.right * 550f + Vector2.up * 25f);

        scv1.MyRect = scv1.MyRect.SetPosition(Vector2.right * 620f);
        scv1.RectView = new Rect(0f, 0f, 350f, 350f);

        cbx1.MyRect = cbx1.MyRect.SetPosition(180f, 60f);
        bt4.MyRect = bt4.MyRect.SetPosition(180f, 85f);

    }

    public void OnEnable() {
        startEvents();
    }

    private void startEvents() {
        bt1.OnClick.AddListener(() => {
            Debug.Log($"[{bt1.name}]OnClik");
        });
        bt2.OnClick.AddListener(() => {
            Debug.Log($"[{bt2.name}]OnClik");
        });
        bt3.OnClick.AddListener(() => {
            Debug.Log($"[{bt3.name}]OnClik");
        });
        bt4.OnClick.AddListener(() => {
            Debug.Log($"[{bt4.name}]OnClik");
        });
        ckb1.OnChecked.AddListener((c) => Debug.Log($"{ckb1.name}[S:{c}]"));
        ckb2.OnChecked.AddListener((c) => Debug.Log($"{ckb2.name}[S:{c}]"));
        ckb3.OnChecked.AddListener((c) => Debug.Log($"{ckb3.name}[S:{c}]"));
        cbx1.OnSelectedIndex.AddListener((i) => Debug.Log($"CBX:{i.Index}"));

        scv1.ScrollViewAction += (scv) => {
            box2.OnIGU();
        };
    }

    public void OnDestroy() {
        Destroy(bt1 ,bt2 ,bt3, bt4, cbx1, ckb1, ckb2, ckb3);
    }

    private static void Destroy(params UEObject[] objs) {
        foreach ( UEObject obj in objs )
            UEObject.Destroy(obj);
    }
}