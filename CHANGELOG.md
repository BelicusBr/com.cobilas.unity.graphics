# Changelog
## [1.0.6] 27/08/2022
### (Fix)MarkedText.cs
No metódo `string:MarkedText.ToString();` existia o problema em que marcar o MarkedText<br/>
como `FontStyle.Normal`, o texto não aparecia.
### (Fix)IGURect.cs
Na propriedade `Vector2:IGURect.ModifiedPosition` se utiliza da propriedade `Vector2:IGURect.ModifiedSize`<br/>
para realizar o calculo em vez da propriedade `Vector2:IGURect.Size` como anteriormente.
### (Add)IGUContainer.cs
O metódo
```c#
	public static IGUContainer GetOrCreateIGUContainer(string name);
```
cria um novo IGUContainer ou pega um IGUContainer já existente.
### (Change)IGUContainer.cs
Agora os metódos `IGUContainer:IGUContainer.CreateGenericIGUContainer();` é `IGUContainer:IGUContainer.CreatePermanentGenericIGUContainer();`<br/>
utilizão o metódo `IGUContainer:IGUContainer.GetOrCreateIGUContainer(string)`.
### (Fix)IGUDrawer.cs
No metódo `void:IGUDrawer.OnGUI()` se usava o `Event.current` para coletar o estado dos gatilhos do<br/>
mouse, mais ocorria o problema que quando o metódo `Event.Use()` isso interferia na coletar o estado dos gatilhos do<br/>
mouse o que foi resolvido usando o metódo `bool:Event.PopEvent(Event)`.
## [1.0.5] 13/08/2022
- Change Runtime\IGU\IGUObjects\IGUObjectBase\IGUObject.cs
## [1.0.5] 12/08/2022
- Change package.json
- Change Runtime\Cobilas.Unity.Graphics.asmdef
- Change Editor\Cobilas.Unity.Editor.Graphics.asmdef
## [1.0.4] 03/08/2022
- Fix CHANGELOG.md
- Fix package.json
- Add IGUInput.cs
- Add MouseButtonTypeEnum.cs
- Change IGUObjectDraw.cs
- Change IGUConfigDraw.cs
- Change IGUDrawer.cs
- Change IGUButton.cs
- Change IGUCheckBox.cs
- Change IGUComboBox.cs
- Change IGUHorizontalScrollbar.cs
- Change IGUVerticalScrollbar.cs
- Change IGUHorizontalSlider.cs
- Change IGUVerticalSlider.cs
- Change IGUTextFieldObject.cs
- Change IGUTextObject.cs
- Change IGUPasswordField.cs
- Change IGURepeatButton.cs
- Change IGUScrollView.cs
- Change IGUSelectableText.cs
- Change IGUTextField.cs
- Change IGUWindow.cs
- Change IGUConfig.cs
## [1.0.3] 31/07/2022
- Fix CHANGELOG.md
- Fix package.json
- Add Cobilas Graphics.asset
- Remove Editor\DependencyWarning.cs
- Remove Runtime\DependencyWarning.cs
- Remove Test\Runtime\DependencyWarning.cs
## [1.0.2] 31/07/2022
- Fix CHANGELOG.md
- Fix package.json
- Change CobilasResolutions.cs
## [1.0.1] 27/07/2022
- Fix CHANGELOG.md
- Fix package.json
- Fix IGULabelDraw.cs
- Fix IGUComboBoxDraw.cs
- Change IGUPasswordFieldDraw.cs
- Fix IGUCheckBoxDraw.cs
- Fix IGUDrawer.cs
- > O método `internal void Remove(IGUContainer);` foi corrigido.
## [1.0.0] 25/07/2022
### Beta do repositorio com.cobilas.unity.graphics finalizado.
- Lançado para o GitHub
- Fix CHANGELOG.md
- Fix package.json
## [1.0.0-beta.44] 25/07/2022
- Fix CHANGELOG.md
- Fix package.json
- Fix IGUSelectionGridDraw.cs
- Fix IGUComboBoxDraw.cs
- Fix IGUComboBox.cs
- > Á propriedade `IGUComboBoxButton[]: IGUComboBox.BoxButtons { get; }` foi adicionada.
## [1.0.0-beta.43] 24/07/2022
- Fix CHANGELOG.md
- Fix package.json
- Add IGUButtonDraw.cs
- Add IGUCheckBoxDraw.cs
- Add IGUComboBoxDraw.cs
- Add IGULabelDraw.cs
- Add IGUNumericBoxDraw.cs
- Add IGUNumericBoxIntDraw.cs
- Add IGUPictureBox.cs
- Add IGURepeatButtonDraw.cs
- Add IGUScrollViewDraw.cs
- Add IGUSelectionGridDraw.cs
- Add IGUWindowDraw.cs
- Fix IGUComboBox.cs
- > Á propriedade `bool:IGUComboBox.AdjustComboBoxView { get; set; }` foi adicionada.
- > Á propriedade `bool:IGUComboBox.CloseComboBoxView { get; set; }` foi adicionada.
- Fix IGUScrollView.cs
- > Removido a instrução `GUI.SetNextControlName(name);` de `IGUScrollView.OnIGU();`.
- Fix IGUWindow.cs
- > A instrução `GUI.SetNextControlName(name);` foi realocada do método `IGUWindow.OnIGU();` para `IGUObject.InternalOnIGU()`.
- Fix IGUCheckBox.cs
- > Removido o atributo [SerializeField] de `bool oneClick;`.
## [1.0.0-beta.42] 23/07/2022
- Fix CHANGELOG.md
- Fix package.json
- Add IGUHorizontalScrollbarDraw.cs
- Add IGUHorizontalScrollbarDraw.cs
- Add IGUVerticalSliderDraw.cs
- Add IGUVerticalSliderDraw.cs
- Add IGUTest.cs
- Change IGUPropertyDrawer.cs
- Change IGUObjectDraw.cs
- Change IGUObjectPropertyDrawer.cs
## [1.0.0-beta.41] 23/07/2022
- Add CHANGELOG.md
- Fix package.json
## [1.0.0-beta.40] 22/07/2022
- Fix package.json
## [1.0.0-beta.36] 22/07/2022
- Add Editor/DependencyWarning.cs
- Add Runtime/DependencyWarning.cs
- Add LICENSE.md
- Fix Cobilas.Unity.Graphics.asmdef
- Fix Cobilas.Unity.Editor.Graphics.asmdef
- Fix Cobilas.Unity.Graphics.IGU.TestsFramework.asmdef
## [1.0.0-beta.34] 17/07/2022
- Fix package.json
- Delete main.yml
## [1.0.0-beta.32] 15/07/2022
- Delete READNE.md
## [1.0.0-beta.1] 15/07/2022
- Fix package.json
## [1.0.0-beta.1] 15/07/2022
- Add package.json
- Add main.yml
- Add folder:Editor
- Add folder:Test
- Add folder:Runtime
## [0.0.1] 15/07/2022
### Repositorio com.cobilas.unity.graphics iniciado
- Lançado para o GitHub