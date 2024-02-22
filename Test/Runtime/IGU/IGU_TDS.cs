using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU;
using UnityEngine.SceneManagement;

public class IGU_TDS : MonoBehaviour {

    [SerializeField] private IGUComboBox comboBox;
    [SerializeField] private IGUVerticalScrollbar verticalScrollbar;
    [SerializeField] private IGUHorizontalScrollbar horizontalScrollbar;
    // Start is called before the first frame update
    void Start() {
        // verticalScrollbar = IGUObject.CreateIGUInstance<IGUVerticalScrollbar>("TDS1");
        // horizontalScrollbar = IGUObject.CreateIGUInstance<IGUHorizontalScrollbar>("TDS2");

        // _ = verticalScrollbar.ApplyToContainer(horizontalScrollbar.ApplyToGenericContainer());

        // horizontalScrollbar.MyRect = horizontalScrollbar.MyRect.SetPosition(0f, verticalScrollbar.MyRect.Height);
        comboBox = IGUObject.CreateIGUInstance<IGUComboBox>("TDS1");
        _ = comboBox.ApplyToGenericContainer();

        comboBox.OnClick.AddListener(()=>{ Debug.Log("OnClick"); });
        comboBox.OnSelectedIndex.AddListener((e) => { Debug.Log(e.Index); });
        comboBox.OnActivatedComboBox.AddListener(() => { Debug.Log("OnActivatedComboBox"); });
    }
}
