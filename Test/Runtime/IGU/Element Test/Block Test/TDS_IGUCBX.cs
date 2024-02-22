using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

public class TDS_IGUCBX : MonoBehaviour {

    [SerializeField] private IGUComboBox comboBox;

    private void Start() {
        comboBox = IGUObject.CreateIGUInstance<IGUComboBox>("#TDS1258");
        _ = comboBox.ApplyToGenericContainer();

        comboBox.MyRect = comboBox.MyRect.SetPosition(Vector2.right * 145f);
    }
}