using System;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using UnityEngine;

public class TDS_IGUComun : MonoBehaviour {

    [SerializeField] private IGUBox box;
    [SerializeField] private IGULabel label1;
    [SerializeField] private IGULabel label2;
    [SerializeField] private IGUButton button;
    [SerializeField] private IGUWindow window;
    [SerializeField] private IGUCheckBox checkBox;
    [SerializeField] private IGUTextField textArea;
    [SerializeField] private IGUTextField textField;
    [SerializeField] private IGUPictureBox pictureBox;
    [SerializeField] private IGURepeatButton repeatButton;
    [SerializeField] private IGUPasswordField passwordField;
    [SerializeField] private IGUVerticalSlider verticalSlider;
    [SerializeField] private IGUHorizontalSlider horizontalSlider;
    [SerializeField] private IGUVerticalScrollbar verticalScrollbar;
    [SerializeField] private IGUHorizontalScrollbar horizontalScrollbar;

    private void Awake() {
        button = IGUObject.Create<IGUButton>("#TDS6456");
        repeatButton = IGUObject.Create<IGURepeatButton>("#TDS7513");
        checkBox = IGUObject.Create<IGUCheckBox>("#TDS8564");
        label1 = IGUObject.Create<IGULabel>("#TDS9743");
        label2 = IGUObject.Create<IGULabel>("#TDS35897");
        box = IGUObject.Create<IGUBox>("#TDS2158");
        pictureBox = IGUObject.Create<IGUPictureBox>("#TDS4865");
        passwordField = IGUObject.Create<IGUPasswordField>("#TDS48566");
        textArea = IGUObject.Create<IGUTextField>("#TDS5455");
        textField = IGUObject.Create<IGUTextField>("#TDS9999");
        verticalScrollbar = IGUObject.Create<IGUVerticalScrollbar>("#TDS6562");
        verticalSlider = IGUObject.Create<IGUVerticalSlider>("#TDS23434");
        horizontalSlider = IGUObject.Create<IGUHorizontalSlider>("#TDS546332");
        horizontalScrollbar = IGUObject.Create<IGUHorizontalScrollbar>("#TDS4354");
        window = IGUObject.Create<IGUWindow>("#TDS35686");

        repeatButton.MyConfig = repeatButton.MyConfig.SetDepth(2);
        label2.MyConfig = label2.MyConfig.SetDepth(5);
        pictureBox.MyConfig = pictureBox.MyConfig.SetDepth(-2);
        button.MyConfig = button.MyConfig.SetDepth(10);

        IGUCanvas container = button.ApplyToGenericContainer();
        container = repeatButton.ApplyToContainer(container);
        container = checkBox.ApplyToContainer(container);
        container = label1.ApplyToContainer(container);
        container = label2.ApplyToContainer(container);
        container = box.ApplyToContainer(container);
        container = pictureBox.ApplyToContainer(container);
        container = passwordField.ApplyToContainer(container);
        container = textArea.ApplyToContainer(container);
        container = textField.ApplyToContainer(container);
        container = verticalScrollbar.ApplyToContainer(container);
        container = verticalSlider.ApplyToContainer(container);
        container = horizontalSlider.ApplyToContainer(container);
        container = horizontalScrollbar.ApplyToContainer(container);
        container = window.ApplyToContainer(container);
    }

    private void OnEnable() {
        button.MyRect = button.MyRect.SetPosition(Vector2.up * 50f);
        repeatButton.MyRect = repeatButton.MyRect.SetPosition(Vector2.up * button.MyRect.Donw);
        checkBox.MyRect = checkBox.MyRect.SetPosition(Vector2.up * repeatButton.MyRect.Donw);
        passwordField.MyRect = passwordField.MyRect.SetPosition(Vector2.up * checkBox.MyRect.Donw);
        textField.MyRect = textField.MyRect.SetPosition(Vector2.up * passwordField.MyRect.Donw);
        textArea.MyRect = textArea.MyRect.SetPosition(Vector2.up * textField.MyRect.Donw).SetSize(Vector2.one * 130f);

        textArea.IsTextArea = true;

        label1.Text = label2.Text = "Texte de confirmação do tamanho";
        label2.AutoSize =
            label2.RichText = true;
        
        label2.SetMarkedText(new MarkedText("confirmação", Color.red));

        label1.MyRect = label1.MyRect.SetPosition(Vector2.right * 160f);
        label2.MyRect = label2.MyRect.SetPosition(Vector2.right * 160f + Vector2.up * label1.MyRect.Donw);
        box.MyRect = box.MyRect.SetPosition(Vector2.right * 160f + Vector2.up * label2.MyRect.Donw);
        pictureBox.MyRect = pictureBox.MyRect.SetPosition(Vector2.right * 160f + Vector2.up * box.MyRect.Donw);
        verticalScrollbar.MyRect = verticalScrollbar.MyRect.SetPosition(Vector2.right * 160f + Vector2.up * pictureBox.MyRect.Donw);
        verticalSlider.MyRect = verticalSlider.MyRect.SetPosition(Vector2.right * 160f + Vector2.up * verticalScrollbar.MyRect.Donw);
        horizontalSlider.MyRect = horizontalSlider.MyRect.SetPosition(Vector2.right * 160f + Vector2.up * verticalSlider.MyRect.Donw);
        horizontalScrollbar.MyRect = horizontalScrollbar.MyRect.SetPosition(Vector2.right * 160f + Vector2.up * horizontalSlider.MyRect.Donw);

        window.MyRect = window.MyRect.SetPosition(Vector2.right * 380f);

        button.OnClick.AddListener(() => Debug.Log("BT"));
        repeatButton.OnClick.AddListener(() => Debug.Log("RBT"));

        checkBox.OnClick.AddListener(() => Debug.Log("CKB"));
        checkBox.OnChecked.AddListener((b) => Debug.Log($"CKB[{b}]"));
    }
}