using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using UnityEngine;

public class TDS_SCG : MonoBehaviour {

    public int index;
    public int xCount = 3;
    public Vector2 spacing = Vector2.one * 3f;
    [SerializeField] private IGUSelectionGrid selectionGrid;

    private void Awake() {
        selectionGrid = IGUObject.Create<IGUSelectionGrid>("#TDS6534");
        IGUCanvas container = selectionGrid.ApplyToGenericContainer();

        selectionGrid.MyRect = selectionGrid.MyRect.SetPosition(Vector2.right * 150f);

    }

    private void OnEnable() {
        selectionGrid.OnSelectedIndex.AddListener((i) => {
            Debug.Log($"Selec[{i}]");
        });
        selectionGrid.SelectedIndex = 2;
    }

    private void Update() {
        selectionGrid.Spacing = spacing;
        index = selectionGrid.SelectedIndex;
        selectionGrid.xCount = xCount = xCount < 1 ? 1 : xCount;
    }
}