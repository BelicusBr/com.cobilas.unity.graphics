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

using BEIGU = Cobilas.Unity.Test.Graphics.IGU.BackEndIGU;
using Cobilas.Unity.Test.Graphics.IGU.Physics;

public class IGU_TDS : MonoBehaviour {

    public IGURect rect;
    public Rect rectDrag;
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
        rect = IGURect.DefaultButton.SetPosition(Vector2.right * 180f);

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {
        comboBox.OnSelectedIndex.AddListener(SceneChange);
    }

    private void OnGUI() {
        // rect = TDS_IGUStyle.DrawWindow(rect, rectDrag,
        //     GUIUtility.GetControlID(FocusType.Passive, rect),
        //     (id) => {}, (IGUStyle)"Black window border", new IGUContent("Win"));
        //GUI.enabled = false;
        BEIGU.PasswordField(rect, content, GetInstanceID(), 0, '*', new IGUPhysicsTest(), (IGUStyle)"Black text field border");
        //Debug.Log(Event.current.type);
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
