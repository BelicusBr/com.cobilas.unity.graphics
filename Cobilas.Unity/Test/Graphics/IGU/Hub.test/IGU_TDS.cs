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

    public Vector2[] triangle = new Vector2[4];

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
        // boxPhy1.OnGUI();
        //     boxPhy2.OnGUI();
        //         boxPhy3.OnGUI();
        //             boxPhy4.OnGUI();
        //             boxPhy4.EndOnGUI();
        //         boxPhy3.EndOnGUI();
        //     boxPhy2.EndOnGUI();
        // boxPhy1.EndOnGUI();
    }

    private void OnDrawGizmos() {
        // boxPhy1.OnDrawGizmos();
        // boxPhy2.OnDrawGizmos();
        // boxPhy3.OnDrawGizmos();
        // boxPhy4.OnDrawGizmos();
        /*
            A = (3,4)
            B = (2,1)
            C = (1,3)

            AB = (2,1) - (3,4) = (-1,-3)
            AC = (1,3) - (3,4) = (-2,-1)
            N = n(AB) = (-3,1)
            D = dot(N,AC) = 6 + -1 = 5

            since D > 0:
              N = -N = (3,-1)
        */
        
        Gizmos.color = Color.red;
        triangle[3] = (triangle[0] + triangle[1] + triangle[2]) / 3f;
        Vector2 mpos = (Input.mousePosition - Vector3.up * Screen.height).InvertY();

        Vector2 norm1 = (triangle[1] - triangle[0]).normalized;
        Vector2 norm2 = (triangle[2] - triangle[1]).normalized;
        Vector2 norm3 = (triangle[0] - triangle[2]).normalized;

        norm1 = (Vector2.right * norm1.y + Vector2.up * norm1.x).InvertX();
        norm2 = (Vector2.right * norm2.y + Vector2.up * norm2.x).InvertX();
        norm3 = (Vector2.right * norm3.y + Vector2.up * norm3.x).InvertX();
        Vector2 n3pos = (Vector2.right * norm3.y + Vector2.up * norm3.x).InvertY();
        n3pos = n3pos * new Vector2(200f, 150f) + triangle[2];

        Debug.Log($"{norm1} {norm2} {norm3} {mpos}");

        Vector2 vc1 = Camera.current.ScreenToWorldPoint((triangle[0] - Vector2.up * Screen.height).InvertY());
        Vector2 vc2 = Camera.current.ScreenToWorldPoint((triangle[1] - Vector2.up * Screen.height).InvertY());
        Vector2 vc3 = Camera.current.ScreenToWorldPoint((triangle[2] - Vector2.up * Screen.height).InvertY());
        Vector2 cen = Camera.current.ScreenToWorldPoint((triangle[3] - Vector2.up * Screen.height).InvertY());

        Vector2 n1 = Camera.current.ScreenToWorldPoint((norm1 * 10f + ((triangle[1] + triangle[0]) / 2f) - Vector2.up * Screen.height).InvertY());
        Vector2 n2 = Camera.current.ScreenToWorldPoint((norm2 * 10f + ((triangle[2] + triangle[1]) / 2f) - Vector2.up * Screen.height).InvertY());
        Vector2 n3 = Camera.current.ScreenToWorldPoint((norm3 * 10f + ((triangle[0] + triangle[2]) / 2f) - Vector2.up * Screen.height).InvertY());

        Vector2 n4 = Camera.current.ScreenToWorldPoint((n3pos - Vector2.up * Screen.height).InvertY());

        Gizmos.DrawLine(vc1, vc2);
        Gizmos.DrawLine(vc2, vc3);
        Gizmos.DrawLine(vc3, vc1);

        Gizmos.DrawLine(cen, cen + Vector2.right * .5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(cen, cen + Vector2.up * .5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(n1, n1 + Vector2.right * .5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(n1, n1 + Vector2.up * .5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(n2, n2 + Vector2.right * .5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(n2, n2 + Vector2.up * .5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(n3, n3 + Vector2.right * .5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(n3, n3 + Vector2.up * .5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(n4, n4 + Vector2.right * .5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(n4, n4 + Vector2.up * .5f);
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
                Vector2 a = item.A * rect.Size;
                Vector2 b = item.B * rect.Size;
                Vector2 c = item.C * rect.Size;

                Quaternion quaternion = Quaternion.Euler(Vector3.forward * GetRotation(this));

                a = quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y;
                b = quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y;
                c = quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y;

                a += GetGURect(this, false).Position;
                b += GetGURect(this, false).Position;
                c += GetGURect(this, false).Position;

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

        private static IGURect GetGURect(TDSIGUBoxPhy phy, bool noRot) {
            if (phy.parent != null) {
                IGURect temp = phy.rect;
                Vector2 pos = temp.Position;
                if (!noRot) {
                    Quaternion quaternion = Quaternion.Euler(Vector3.forward * GetRotation(phy.parent));
                    pos = quaternion.GenerateDirectionRight() * temp.X + quaternion.GenerateDirectionUp() * temp.Y;
                }
                return temp.SetPosition(GetGURect(phy.parent, noRot).Position + pos);
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
            GUIUtility.RotateAroundPivot(rect.Rotation, GetGURect(this, false).Position);
            GUI.Box((Rect)GetGURect(this, true), name);
        }

        public void EndOnGUI() {
            GUI.matrix = matrix;
        }
    }
}
