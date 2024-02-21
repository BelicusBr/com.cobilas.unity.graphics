using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU;
using UnityEngine.SceneManagement;

public class IGU_TDS : MonoBehaviour {

    [SerializeField] private float scrollViewHeight;
    [SerializeField] private bool adjustComboBoxView;
    [SerializeField] private float comboBoxButtonHeight;
    [SerializeField] private TDS_IGUComboBox comboBox;
    // Start is called before the first frame update
    void Start() {
        comboBox = IGUObject.CreateIGUInstance<TDS_IGUComboBox>("TDS1");
        _ = comboBox.ApplyToGenericContainer();
        scrollViewHeight = comboBox.ScrollViewHeight;
        adjustComboBoxView = comboBox.AdjustComboBoxView;
        comboBoxButtonHeight = comboBox.ComboBoxButtonHeight;

        comboBox.OnClick.AddListener(()=>{ Debug.Log("OnClick"); });
        comboBox.OnSelectedIndex.AddListener((e) => { Debug.Log(e.Index); });
        comboBox.OnActivatedComboBox.AddListener(() => { Debug.Log("OnActivatedComboBox"); });
    }

    private void Update() {
        comboBox.ScrollViewHeight = scrollViewHeight;
        comboBox.AdjustComboBoxView = adjustComboBoxView;
        comboBox.ComboBoxButtonHeight = comboBoxButtonHeight;
    }
}
