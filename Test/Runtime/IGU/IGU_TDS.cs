using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU;
using UnityEngine.SceneManagement;
using Cobilas.Collections;

public class IGU_TDS : MonoBehaviour {

    [SerializeField] private IGUComboBox comboBox;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        GameObject gob = new GameObject("SWITCH_IGU_TDS", typeof(IGU_TDS));
        gob.isStatic = true;
        gob.SetPosition(Vector3.zero);
    }

    void Start() {
        comboBox = IGUObject.CreateIGUInstance<IGUComboBox>("#TDS1");
        _ = comboBox.ApplyToPermanentGenericContainer();

        comboBox.Clear();
        comboBox.Add($"TDS/{nameof(IGUComboBox)}");
        comboBox.Add($"TDS/{nameof(IGUScrollView)}");

        comboBox.OnSelectedIndex.AddListener(SceneChange);

        DontDestroyOnLoad(gameObject);
    }

    void SceneChange(IGUComboBoxButton button) {
        switch (button.Index) {
            case 0:
                Scene scene = SceneManager.GetActiveScene();
                Debug.Log(scene.name);
                UnloadSceneOptions op = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;
                SceneManager.UnloadSceneAsync(scene, op);
                Scene sc = SceneManager.CreateScene(button.Text);
                Debug.Log("Is loaded:" + SceneManager.SetActiveScene(sc));
                GameObject gob = new GameObject(nameof(TDS_IGUCBX), typeof(TDS_IGUCBX));
                break;
        }
        
    }
}
