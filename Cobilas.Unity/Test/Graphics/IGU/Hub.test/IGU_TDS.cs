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
using System.Linq;

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

    private Triangle[] triangles = Triangle.Circle;
    private Triangle[] trianglesbox = Triangle.Box;
    public IGURect recttt = new IGURect(Vector2.one * 145f, Vector2.one * 250f);
    private void Update() {
        // Vector2 mpos = (Input.mousePosition - Vector3.up * Screen.height).InvertY();
        // for (int I = 0; I < triangles.Length; I++) {
        //     if (Triangle.InsideInTriangle(triangles[I], recttt, mpos))
        //         print($"Colisão triangulo {I}");
        // }
    }

    private void OnDrawGizmos() {
        for (int I = 0; I < triangles.Length; I++) {
            Quaternion quaternion = Quaternion.Euler(Vector3.forward * recttt.Rotation);
            Vector2 n_size = recttt.Size * .5f;
            Vector2 a = triangles[I].A * n_size;
            Vector2 b = triangles[I].B * n_size;
            Vector2 c = triangles[I].C * n_size;

            a = (Vector2)(quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y) + recttt.Position;
            b = (Vector2)(quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y) + recttt.Position;
            c = (Vector2)(quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y) + recttt.Position;
          
            a = Camera.main.ScreenToWorldPoint(a).InvertY();
            b = Camera.main.ScreenToWorldPoint(b).InvertY();
            c = Camera.main.ScreenToWorldPoint(c).InvertY();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(c, a);            
        }
        for (int I = 0; I < trianglesbox.Length; I++) {
            Quaternion quaternion = Quaternion.Euler(Vector3.forward * recttt.Rotation);
            Vector2 a = trianglesbox[I].A * recttt.Size;
            Vector2 b = trianglesbox[I].B * recttt.Size;
            Vector2 c = trianglesbox[I].C * recttt.Size;

            a = (Vector2)(quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y) + recttt.Position;
            b = (Vector2)(quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y) + recttt.Position;
            c = (Vector2)(quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y) + recttt.Position;

            Gizmos.color = Color.green;
            Vector2 vector = Camera.main.ScreenToWorldPoint(recttt.Position).InvertY();
            Gizmos.DrawLine(vector, vector + Vector2.up);
            Gizmos.DrawLine(vector, vector + Vector2.right);
        
            a = Camera.main.ScreenToWorldPoint(a).InvertY();
            b = Camera.main.ScreenToWorldPoint(b).InvertY();
            c = Camera.main.ScreenToWorldPoint(c).InvertY();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(c, a);
        }
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
