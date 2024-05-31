using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU;
using UnityEngine.SceneManagement;
using Cobilas.Collections;
using Cobilas.Unity.Management.Runtime;
using System.Text;
using Cobilas.Unity.Test.Graphics.IGU;

using BEIGU = Cobilas.Unity.Graphics.IGU.BackEndIGU;
using System.Linq;
using System;
using Cobilas.Unity.Graphics.IGU.Physics;

public class IGU_TDS : MonoBehaviour {

    public IGUContent content = IGUContent.none;
    [SerializeField] private IGUComboBox comboBox;
    [SerializeField] private BC_TDS_IGUPhy tdsphy;

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
        comboBox.Add($"TDS/IGUPhysics");
        comboBox.Add($"TDS/Windows");

        comboBox.Index = 0;

        DontDestroyOnLoad(gameObject);
    }

    private IGUStyle hstl_scrollbar;
    private IGUStyle hstl_scrollbarThumb;
    private IGUStyle vstl_scrollbar;
    private IGUStyle vstl_scrollbarThumb;

    private void OnEnable() {
        tdsphy?.OnEnable();
        comboBox.OnSelectedIndex.AddListener(SceneChange);
        hstl_scrollbar = new IGUStyle("Black horizontal scrollbar border");
        hstl_scrollbarThumb = new IGUStyle("Black horizontal scrollbar border thumb");
        vstl_scrollbar = new IGUStyle("Black vertical scrollbar border");
        vstl_scrollbarThumb = new IGUStyle("Black vertical scrollbar border thumb");
    }

    void SceneChange(IGUComboBoxButton button) {
        switch (button.Index) {
            case 0:
                // Scene scene = SceneManager.GetActiveScene();
                // SceneManager.UnloadSceneAsync(scene);
                // SceneManager.LoadScene(0);
                tdsphy?.OnDestroy();
                tdsphy = null;
                break;
            case 1:
                if (tdsphy != null) break;
                (tdsphy = new BC_TDS_IGUPhy()).Start();
                tdsphy.OnEnable();
                break;
        }
    }

    void Load(IGUComboBoxButton button, System.Type type) {
        Scene sc = SceneManager.CreateScene(button.Text);
        SceneManager.SetActiveScene(sc);
        GameObject gob = new GameObject(type.Name, type);
    }
}
