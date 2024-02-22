using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

public class TDS_IGUSCV : MonoBehaviour {

    [SerializeField] private IGUScrollView scrollView;
    [SerializeField, HideInInspector] private IGUButton button1;
    [SerializeField, HideInInspector] private IGUButton button2;
    [SerializeField, HideInInspector] private IGUButton button3;

    private void Start() {
        scrollView = IGUObject.CreateIGUInstance<IGUScrollView>("#TDS7321");
        _ = scrollView.ApplyToGenericContainer();
        scrollView.MyRect = scrollView.MyRect.SetPosition(150f, 0f);
        scrollView.RectView = new Rect(Vector2.zero, Vector2.one * 450f);

        button1 = IGUObject.CreateIGUInstance<IGUButton>("BT1");
        button2 = IGUObject.CreateIGUInstance<IGUButton>("BT2");
        button3 = IGUObject.CreateIGUInstance<IGUButton>("BT3");

        button2.MyRect = button2.MyRect.SetPosition(0f, button1.MyRect.Donw);
        button3.MyRect = button3.MyRect.SetPosition(0f, button2.MyRect.Donw);

        scrollView.ScrollViewAction += (svc) => {
            button1.OnIGU();
            button2.OnIGU();
            button3.OnIGU();
        };
    }
}