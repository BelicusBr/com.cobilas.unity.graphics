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

public class IGU_TDS : MonoBehaviour {

    public float rot;
    public Vector3 pos, scl;
    public Matrix4x4 mat;
    public Matrix4x4 mat2;
    public IGURect rect1 = new IGURect(Vector2.zero, Vector2.one * 150f);
    public IGURect rect2 = new IGURect(Vector2.right * 180f, Vector2.one * 150f);
    public IGURect rect3 = new IGURect(Vector2.up * 180f, Vector2.one * 250f);
    public IGURect rect4 = new IGURect(Vector2.zero, Vector2.one * 150f);
    [SerializeField] private IGUComboBox comboBox;

    [StartAfterSceneLoad]
    static void Init() {
        GameObject gob = new GameObject("SWITCH_IGU_TDS", typeof(IGU_TDS));
        gob.isStatic = true;
        gob.SetPosition(Vector3.zero);
    }

    private void Awake() {
        rect1 = new IGURect(Vector2.zero, Vector2.one * 150f);
        rect2 = new IGURect(Vector2.right * 180f, Vector2.one * 150f);
        comboBox = IGUObject.CreateIGUInstance<IGUComboBox>("#TDS1");
        //_ = comboBox.ApplyToPermanentGenericContainer();

        comboBox.Clear();
        comboBox.Add($"TDS/None");
        comboBox.Add($"TDS/Comun");
        comboBox.Add($"TDS/{nameof(IGUComboBox)}");
        comboBox.Add($"TDS/{nameof(IGUScrollView)}");
        comboBox.Add($"TDS/{nameof(IGUSelectionGrid)}");

        comboBox.Index = 0;

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {
        comboBox.OnSelectedIndex.AddListener(SceneChange);
    }

    private void OnGUI() {
        //GUIUtility.ScaleAroundPivot(IGUDrawer.ScaleFactor, Vector2.zero);
        // Draw1();
        // Draw2();
        Draw3();
        //GUI.matrix = mat;
        // m20 + m23 servem pata fazer clipagem no vetor x

        mat2 = Matrix4x4.Translate(pos) * 
            Matrix4x4.Scale(scl) * Matrix4x4.Rotate(Quaternion.Euler(Vector3.forward * rot));
        //clipping = Matrix4x4.Transpose(Matrix4x4.Inverse(mat2)) * clipping;
        //mat2 *= Camera.current.CalculateObliqueMatrix(mat2 * clipping);
        // mat2.m02 = mat.m02;
        // mat2.m12 = mat.m12;
        // mat2.m20 = mat.m20;
        // mat2.m21 = mat.m21;
        // mat2.m23 = mat.m23;
        // mat2.m30 = mat.m30;
        // mat2.m31 = mat.m31;
        // mat2.m32 = mat.m32;
        GUI.matrix = mat2;
        //Draw4();
    }

    void Draw1() {
        Matrix4x4 oldMatrix = GUI.matrix;
        GUIUtility.RotateAroundPivot(rect1.Rotation, rect1.Position.Multiplication(IGUDrawer.ScaleFactor));
        GUI.Box((Rect)rect1, "Draw1");
        GUI.matrix = oldMatrix;
    }

    void Draw2() {
        Matrix4x4 oldMatrix = GUI.matrix;
        GUIUtility.RotateAroundPivot(rect2.Rotation, rect2.Position.Multiplication(IGUDrawer.ScaleFactor));
        GUI.Box((Rect)rect2, "Draw2");
        GUI.matrix = oldMatrix;
    }

    void Draw3() {
        Matrix4x4 oldMatrix = GUI.matrix;
        GUIUtility.RotateAroundPivot(rect3.Rotation, rect3.Position.Multiplication(IGUDrawer.ScaleFactor));
        GUI.Box((Rect)rect3, string.Empty);
        Rect rect = (Rect)rect3;
        rect.size = oldMatrix.inverse.MultiplyPoint(rect.size);
        GUI.BeginClip(rect, Vector2.zero, Vector2.zero, false);
        Draw4();
        GUI.EndClip();
        GUI.matrix = oldMatrix;
    }

    void Draw4() {
        GUIUtility.RotateAroundPivot(rect4.Rotation, rect4.Position.Multiplication(IGUDrawer.ScaleFactor));
        GUI.Box((Rect)rect4, "Draw4");
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
        }
        
    }

    void Load(IGUComboBoxButton button, System.Type type) {
        Scene sc = SceneManager.CreateScene(button.Text);
        SceneManager.SetActiveScene(sc);
        GameObject gob = new GameObject(type.Name, type);
    }
}
