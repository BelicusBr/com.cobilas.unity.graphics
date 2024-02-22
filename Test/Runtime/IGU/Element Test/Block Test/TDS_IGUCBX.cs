using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

public class TDS_IGUCBX : MonoBehaviour {

    [SerializeField] private TDS_IGUComboBox comboBox;

    private void Start() {
        comboBox = IGUObject.CreateIGUInstance<TDS_IGUComboBox>("#TDS1258");
        _ = comboBox.ApplyToGenericContainer();

        comboBox.MyRect = comboBox.MyRect.SetPosition(Vector2.right * 145f);
    }
}