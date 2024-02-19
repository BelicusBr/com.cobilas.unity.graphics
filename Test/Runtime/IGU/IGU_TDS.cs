using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU;
using UnityEngine.SceneManagement;

public class IGU_TDS : MonoBehaviour {

    [SerializeField]
    private TDS_IGUComboBox comboBox;
    [SerializeField]
    private IGUBox button;
    // Start is called before the first frame update
    void Start() {
        comboBox = IGUObject.CreateIGUInstance<TDS_IGUComboBox>("TDS1");
        button = IGUObject.CreateIGUInstance<IGUBox>("TDS2");
        button.Text = string.Empty;
        IGUContainer container = comboBox.ApplyToGenericContainer();
        button.ApplyToContainer(container);
        button.MyConfg = button.MyConfg.SetDepth(-1);

        comboBox.MyRect = comboBox.MyRect.SetPosition(Vector2.one * 250f);
        button.MyRect = button.MyRect.SetSize(130f, comboBox.ScrollViewHeight)
            .SetPosition(Vector2.up * 275f + Vector2.right * 250f);
    }
}
