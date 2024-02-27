using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

public class TDS_NBlock : MonoBehaviour {

    [SerializeField] private IGUNumericBox numericBox;
    [SerializeField] private IGUNumericBoxInt numericBoxInt;

    private void Awake() {
        numericBox = IGUObject.Create<IGUNumericBox>("#TDS54894");
        numericBoxInt = IGUObject.Create<IGUNumericBoxInt>("#TDS212358");

        IGUCanvas container = numericBox.ApplyToGenericContainer();
        _ = numericBoxInt.ApplyToContainer(container);
    }

    private void OnEnable() {
        numericBox.MyRect = numericBox.MyRect.SetPosition(Vector2.right * 180f);
        numericBoxInt.MyRect = numericBoxInt.MyRect.SetPosition(Vector2.right * 180f + Vector2.up * numericBox.MyRect.Donw);
    }
}