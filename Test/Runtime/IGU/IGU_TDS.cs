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
        // IGUSelectionGrid precissa de uma atualização
        // IGUComboBox precissa de uma atualização
        comboBox = IGUObject.CreateIGUInstance<IGUComboBox>("TDS1");
        IGUContainer container = comboBox.ApplyToGenericContainer();
        SceneManager.LoadScene("TDS_Clipping");
    }
}
