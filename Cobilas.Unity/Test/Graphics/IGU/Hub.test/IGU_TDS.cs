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

    public bool isHoriz = true;
#region GUI.Slider
    public Rect rect = new Rect(350f, 0f, 130f, 25f);
    public float value;
    public float size;
    public MaxMinSlider maxMin = new MaxMinSlider(0f, 150f);
#endregion
#region TDSBackEndIGU.Slider
    public IGURect rect2 = new IGURect(350f, 50f, 130f, 25f);
    public float size2;
    public float value2;
#endregion

    public IGUContent content = IGUContent.none;
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
        comboBox.Add($"TDS/IGUPhysics/1");

        comboBox.Index = 0;

        DontDestroyOnLoad(gameObject);
    }

    private IGUStyle hstl_scrollbar;
    private IGUStyle hstl_scrollbarThumb;
    private IGUStyle vstl_scrollbar;
    private IGUStyle vstl_scrollbarThumb;

    private void OnGUI() {
        Matrix4x4 old = GUI.matrix;
        GUIUtility.RotateAroundPivot(rect2.Rotation, (rect.position + rect2.Position) / 2f);
        GUIStyle stl_scrollbar = isHoriz ? GUI.skin.horizontalScrollbar : GUI.skin.verticalScrollbar;
        GUIStyle stl_scrollbarThumb = isHoriz ? GUI.skin.horizontalScrollbarThumb : GUI.skin.verticalScrollbarThumb;

        IGUStyle istl_scrollbar = isHoriz ? hstl_scrollbar : vstl_scrollbar;
        IGUStyle istl_scrollbarThumb = isHoriz ? hstl_scrollbarThumb : vstl_scrollbarThumb;

        int id = GUIUtility.GetControlID(FocusType.Passive, rect);
        value = GUI.Slider(rect, value, size, maxMin.Min, maxMin.Max, stl_scrollbar, stl_scrollbarThumb, isHoriz, id);
        
        id = GUIUtility.GetControlID(FocusType.Passive, (Rect)rect2);
        value2 = BackEndIGU.Slider(rect2, value2, size2, maxMin, IGUNonePhysics.None, id, true, isHoriz, istl_scrollbar, istl_scrollbarThumb);
        GUI.matrix = old;
    }

    private void OnEnable() {
        comboBox.OnSelectedIndex.AddListener(SceneChange);
        hstl_scrollbar = new IGUStyle("Black horizontal scrollbar border");
        hstl_scrollbarThumb = new IGUStyle("Black horizontal scrollbar border thumb");
        vstl_scrollbar = new IGUStyle("Black vertical scrollbar border");
        vstl_scrollbarThumb = new IGUStyle("Black vertical scrollbar border thumb");
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
            case 6:
                scene = SceneManager.GetActiveScene();
                operation = SceneManager.UnloadSceneAsync(scene);

                if (operation is null)
                    Load(button, typeof(TDS_IGUPHY));
                else
                    operation.completed += (o) => {
                        Load(button, typeof(TDS_IGUPHY));
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
