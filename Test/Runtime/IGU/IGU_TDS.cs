﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU;
using UnityEngine.SceneManagement;
using Cobilas.Collections;
using Cobilas.Unity.Management.Runtime;
using System.Text;

public class IGU_TDS : MonoBehaviour {

    [SerializeField] private IGUComboBox comboBox;

    [StartAfterSceneLoad]
    static void Init() {
        GameObject gob = new GameObject("SWITCH_IGU_TDS", typeof(IGU_TDS));
        gob.isStatic = true;
        gob.SetPosition(Vector3.zero);
    }

    private void Awake() {
        comboBox = IGUObject.Create<IGUComboBox>("#TDS1");
        _ = comboBox.ApplyToPermanentGenericContainer();
        comboBox.UseTooltip = true;
        comboBox.ToolTip = "TDS List";

        comboBox.Clear();
        comboBox.Add($"TDS/None");
        comboBox.Add($"TDS/Comun");
        comboBox.Add($"TDS/{nameof(IGUComboBox)}");
        comboBox.Add($"TDS/{nameof(IGUScrollView)}");
        comboBox.Add($"TDS/{nameof(IGUSelectionGrid)}");
        comboBox.Add($"TDS/{nameof(IGUNumericBox)}");

        comboBox.Index = 0;

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {
        comboBox.OnSelectedIndex.AddListener(SceneChange);
    }

    void SceneChange(IGUComboBoxButton button) {
        switch (button.Index) {
            case 0:
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.UnloadSceneAsync(scene);

                SceneManager.LoadScene(0);
                break;
            case 1:
                scene = SceneManager.GetActiveScene();
                AsyncOperation operation = SceneManager.UnloadSceneAsync(scene);

                if (operation is null)
                    Load(button, typeof(TDS_IGUComun));
                else
                    operation.completed += (o) => {
                        Load(button, typeof(TDS_IGUComun));
                    };
                break;
            case 2:
                scene = SceneManager.GetActiveScene();
                operation = SceneManager.UnloadSceneAsync(scene);

                if (operation is null)
                    Load(button, typeof(TDS_IGUCBX));
                else
                    operation.completed += (o) => {
                        Load(button, typeof(TDS_IGUCBX));
                    };
                break;
            case 3:
                scene = SceneManager.GetActiveScene();
                operation = SceneManager.UnloadSceneAsync(scene);

                if (operation is null)
                    Load(button, typeof(TDS_IGUSCV));
                else
                    operation.completed += (o) => {
                        Load(button, typeof(TDS_IGUSCV));
                    };
                break;
            case 4:
                scene = SceneManager.GetActiveScene();
                operation = SceneManager.UnloadSceneAsync(scene);

                if (operation is null)
                    Load(button, typeof(TDS_SCG));
                else
                    operation.completed += (o) => {
                        Load(button, typeof(TDS_SCG));
                    };
                break;
            case 5:
                scene = SceneManager.GetActiveScene();
                operation = SceneManager.UnloadSceneAsync(scene);

                if (operation is null)
                    Load(button, typeof(TDS_NBlock));
                else
                    operation.completed += (o) => {
                        Load(button, typeof(TDS_NBlock));
                    };
                break;
        }
        
    }

    void Load(IGUComboBoxButton button, System.Type type) {
        Scene sc = SceneManager.CreateScene(button.Text);
        SceneManager.SetActiveScene(sc);
        GameObject gob = new GameObject(type.Name, type);
    }
}
