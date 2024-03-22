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
using System;

public class IGU_TDS : MonoBehaviour {

    public TDSIGUBoxPhy boxPhy1;
    public TDSIGUBoxPhy boxPhy2;
    public TDSIGUBoxPhy boxPhy3;
    public TDSIGUBoxPhy boxPhy4;

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
        boxPhy1 = new TDSIGUBoxPhy();
        boxPhy2 = new TDSIGUBoxPhy();
        boxPhy3 = new TDSIGUBoxPhy();
        boxPhy4 = new TDSIGUBoxPhy();

        boxPhy1.name = "Box1";
        boxPhy2.name = "Box2";
        boxPhy3.name = "Box3";
        boxPhy4.name = "Box4";

        boxPhy1.rect = new IGURect(Vector2.one * 250f, Vector2.one * 350f);
        boxPhy2.rect = new IGURect(Vector2.one * 5f, Vector2.one * 250f);
        boxPhy3.rect = new IGURect(Vector2.one * 5f, Vector2.one * 150f);
        boxPhy4.rect = new IGURect(Vector2.one * 5f, Vector2.one * 50f);

        boxPhy1.color = Color.red;
        boxPhy2.color = Color.blue;
        boxPhy3.color = Color.green;
        boxPhy4.color = Color.yellow;
    }

    private void OnEnable() {
        boxPhy1.triangles = Triangle.Box;
        boxPhy2.triangles = Triangle.Box;
        boxPhy3.triangles = Triangle.Box;
        boxPhy4.triangles = Triangle.Box;

        boxPhy2.parent = boxPhy1;
        boxPhy3.parent = boxPhy2;
        boxPhy4.parent = boxPhy3;

        comboBox.OnSelectedIndex.AddListener(SceneChange);
    }

    private void OnGUI() {
        boxPhy1.OnGUI();
            boxPhy2.OnGUI();
                boxPhy3.OnGUI();
                    boxPhy4.OnGUI();
                    boxPhy4.EndOnGUI();
                boxPhy3.EndOnGUI();
            boxPhy2.EndOnGUI();
        boxPhy1.EndOnGUI();
    }

    private void OnDrawGizmos() {
        boxPhy1.OnDrawGizmos();
        boxPhy2.OnDrawGizmos();
        boxPhy3.OnDrawGizmos();
        boxPhy4.OnDrawGizmos();
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

    [Serializable]
    public sealed class TDSIGUBoxPhy {
        public string name;
        public Color color;
        public IGURect rect;
        public TDSIGUBoxPhy parent;
        public Triangle[] triangles;

        public void OnDrawGizmos() {
            if (triangles == null) return;
            foreach (var item in triangles) {
                Gizmos.color = color;
                Vector2 a = item.A * rect.Size + GetGURect(this).Position;
                Vector2 b = item.B * rect.Size + GetGURect(this).Position;
                Vector2 c = item.C * rect.Size + GetGURect(this).Position;

                Quaternion quaternion = Quaternion.Euler(Vector3.forward * rect.Rotation);
                Vector2 poss = Camera.current.ScreenToWorldPoint(((Vector2)quaternion.GenerateDirectionRight() * rect.Width) + GetGURect(this).Position);
                DrawSeta((poss - Vector2.up * Screen.height).InvertY());

                // https://en.wikipedia.org/wiki/Rotation_matrix
                // if (parent != null) {
                //     Quaternion quaternion = Quaternion.Euler(Vector3.forward * GetRotation(this));
                //     Vector2 pos = GetGURect(this).Position;

                //     a = (Vector2)(quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y) + pos;
                //     b = (Vector2)(quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y) + pos;
                //     c = (Vector2)(quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y) + pos;

                //     // a += (Vector2)(quaternion.GenerateDirectionRight() * rect.X + quaternion.GenerateDirectionUp() * rect.Y);
                //     // b += (Vector2)(quaternion.GenerateDirectionRight() * rect.X + quaternion.GenerateDirectionUp() * rect.Y);
                //     // c += (Vector2)(quaternion.GenerateDirectionRight() * rect.X + quaternion.GenerateDirectionUp() * rect.Y);
                // } else {
                //     Quaternion quaternion = Quaternion.Euler(Vector3.forward * rect.Rotation);
                //     a = (Vector2)(quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y) + rect.Position;
                //     b = (Vector2)(quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y) + rect.Position;
                //     c = (Vector2)(quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y) + rect.Position;
                // }

                a = Camera.current.ScreenToWorldPoint((a - Vector2.up * Screen.height).InvertY());
                b = Camera.current.ScreenToWorldPoint((b - Vector2.up * Screen.height).InvertY());
                c = Camera.current.ScreenToWorldPoint((c - Vector2.up * Screen.height).InvertY());

                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(b, c);
                Gizmos.DrawLine(c, a);
            }
        }

        private static float GetRotation(TDSIGUBoxPhy obj) {
            if (obj.parent != null)
                return obj.rect.Rotation + GetRotation(obj.parent);
            return obj.rect.Rotation;
        }

        private static IGURect GetGURect(TDSIGUBoxPhy phy) {
            if (phy.parent != null) {
                IGURect temp = phy.rect;
                return temp.SetPosition(GetGURect(phy.parent).Position + temp.Position);
            }
            return phy.rect;
        }

        private static void DrawSeta(Vector2 pos) {
            Color oldColor = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + Vector2.right);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, pos + Vector2.up);
            Gizmos.color = oldColor;
        }

        private Matrix4x4 matrix;
        public void OnGUI() {
            name = name ?? string.Empty;
            matrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, GetGURect(this).Position);
            GUI.Box((Rect)GetGURect(this), name);
        }

        public void EndOnGUI() {
            GUI.matrix = matrix;
        }
    }
}
