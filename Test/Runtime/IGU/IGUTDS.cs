using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

public class IGUTDS : IGUObject
{
    public IGUBox box1;
    public IGUBox box2;
    public IGUBox box3;


    protected override void Awake()
    {
        base.Awake();
        box1 = IGUObject.CreateIGUInstance<IGUBox>("--box1");
        box2 = IGUObject.CreateIGUInstance<IGUBox>("--box2");
        box3 = IGUObject.CreateIGUInstance<IGUBox>("--box3");

        box1.UseTooltip =
        box2.UseTooltip =
        box3.UseTooltip = true;

        box1.ToolTip = "My Box 1";
        box2.ToolTip = "My Box 2";
        box3.ToolTip = "My Box 3";

        box1.Text = "box1";
        box2.Text = "box2";
        box3.Text = "box3";

        box1.MyRect = box1.MyRect.SetSize(230f, 230f).SetPosition(Vector2.zero);
        box2.MyRect = box2.MyRect.SetSize(230f, 230f).SetPosition(Vector2.one * 50f);
        box3.MyRect = box3.MyRect.SetSize(230f, 230f).SetPosition(Vector2.one * 100f);

        box2.Parent = box3.Parent = box1.Parent = this;
    }

    protected override void LowCallOnIGU() {
        box1.OnIGU();
        box2.OnIGU();
        box3.OnIGU();
    }
}
