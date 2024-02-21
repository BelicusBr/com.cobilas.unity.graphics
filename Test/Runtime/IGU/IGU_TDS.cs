using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU;
using UnityEngine.SceneManagement;

public class IGU_TDS : MonoBehaviour {

    [SerializeField] private IGUComboBox comboBox;
    // Start is called before the first frame update
    void Start() {
        comboBox = IGUObject.CreateIGUInstance<IGUComboBox>("TDS1");
        _ = comboBox.ApplyToGenericContainer();

        comboBox.OnClick.AddListener(()=>{ Debug.Log("OnClick"); });
        comboBox.OnSelectedIndex.AddListener((e) => { Debug.Log(e.Index); });
        comboBox.OnActivatedComboBox.AddListener(() => { Debug.Log("OnActivatedComboBox"); });
    }
}
