using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using UnityEngine;

public class TDS_SCG : MonoBehaviour {

    public int index;
    [SerializeField] private IGUCheckBox checkBox;
    [SerializeField] private TDS_IGUSelectionGrid selectionGrid;

    private void Awake() {
        checkBox = IGUObject.CreateIGUInstance<IGUCheckBox>("#TDS5748");
        selectionGrid = IGUObject.CreateIGUInstance<TDS_IGUSelectionGrid>("#TDS6534");
        IGUContainer container = selectionGrid.ApplyToGenericContainer();
        _ = checkBox.ApplyToContainer(container);
        //_ = checkBox.ApplyToGenericContainer();

        selectionGrid.MyRect = selectionGrid.MyRect.SetPosition(Vector2.right * 150f);
        checkBox.MyRect = checkBox.MyRect.SetPosition(Vector2.up * 150f);
        //checkBox.MyRect = checkBox.MyRect.SetPosition(Vector2.up * 150f);

    }

    private void Start() {
        
    }

    private void OnEnable() {
        selectionGrid.OnSelectedIndex.AddListener((i) => {
            Debug.Log($"Selec[{i}]");
        });
    }

    private void Update() {
        index = selectionGrid.SelectedIndex;
    }
}