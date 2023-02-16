## [1.7.1] - 12/02/2023
### Added
Agora foi adicionado os esquemas de layout.
```c#
	public sealed class IGUHorizontalLayout
	public sealed class IGUVerticalLayout
	public sealed class IGUGridLayout
```
### Fixed
#### IGUObject
No método `IGUConfig:GetModIGUConfig()` além de verificar se o parent é nulo também são verificados
se `parent.GetModIGUConfig().IsEnabled && myConfg.IsEnabled` é `parent.GetModIGUConfig().IsVisible && myConfg.IsVisible`
são verdadeiros.
No método `Vector2:GetPosition()` a posição do parent erá obtida da propriedade `parent.myRect.ModifiedPosition`
o que vai ocasionar um posicionamento incorreto.
## [1.0.16] - 12/02/2023
### Fixed
No metódo `IGUConatiner.AddDeepAction(int)` á instrução `RefreshDepth();` era executada antes do objeto `DeepAction`
o que fazia que o primeiro objeto `DeepAction` não fosse adicionado ao evento OnIGU.
## [1.0.15] - 09/02/2023
### Fixed
#### Profundidade não aplicada corretamente [#ISU-IGU0001](https://github.com/BelicusBr/com.cobilas.unity.graphics/issues/2)
Na classe `IGUContainer` onde é feito a aplicação de profundidade, agora ao adicionar uma nova profundidade o evento responsável por chamar `DeepAction.OnIGU()` é atualizado.
## [1.0.14] - 05/02/2023
### Fixed
No método `void:InitInternalIndowFunction()` a instrução `GUI.DragWindow` era executado depois da ação `windowFunction` o que podia ocasionar a parada de movimento da janela pelo fato de
que se algum elemento IGU fosse desativado ou quando fosse chamado a propriedade `GUI.enabled = false` dentro da ação `windowFunction`.
## [1.0.13] - 30/01/2023
### Changed
- Romoção de campos não utilizados.
- Remoção de atribuições desnecessárias.
- Transformando possiveis campos em `readonly`.
## [1.0.12] 09/01/2023
### Changed
O corpo do método `public static Vector2Int GetBaseResolutionPlatform()` foi alterado.
## [1.0.9] 06/09/2022
### Fixed
A propriedade `IGUEvent` foi removida por casar sertos conflitos com os metódos da classe `GUI`.<br/><br/>

Nas calsses `IGUDrawer` e `IGUConatiner` ocorria o problema com os eventos OnIGU que a cada deserialização<br/>
os eventos OnIGU não eram limpos o que fazia com que os elementos IGU fossem duplicados.

### Changed
Agora os gatilhos do mouse são detectados pela classe `Imput`.
## [1.0.7] 02/09/2022
### Fixed (IGUDrawer)
Agora o evento coletado e marcado como usado.
### Fixed (IGUObjectDrawer)
As instruções `property.serializedObject.Update();` e `property.serializedObject.ApplyModifiedProperties();`<br/>
foram removidas por causar o congelamento dos valores já pre-definidos no inspetor depóis de mudado<br/>
pro modo jogo.
## [1.0.6] 27/08/2022 
### Fixed (MarkedText)
No metódo `string:MarkedText.ToString();` existia o problema em que marcar o MarkedText<br/>
como `FontStyle.Normal`, o texto não aparecia.
### Fixed (IGURect)
Na propriedade `Vector2:IGURect.ModifiedPosition` se utiliza da propriedade `Vector2:IGURect.ModifiedSize`<br/>
para realizar o calculo em vez da propriedade `Vector2:IGURect.Size` como anteriormente.
### Added (IGUContainer)
O metódo
```c#
	public static IGUContainer GetOrCreateIGUContainer(string name);
```
cria um novo IGUContainer ou pega um IGUContainer já existente.
### Changed (IGUContainer)
Agora os metódos `IGUContainer:IGUContainer.CreateGenericIGUContainer();` é `IGUContainer:IGUContainer.CreatePermanentGenericIGUContainer();`<br/>
utilizão o metódo `IGUContainer:IGUContainer.GetOrCreateIGUContainer(string)`.
### Fixed (IGUDrawer)
No metódo `void:IGUDrawer.OnGUI()` se usava o `Event.current` para coletar o estado dos gatilhos do<br/>
mouse, mais ocorria o problema que quando o metódo `Event.Use()` isso interferia na coletar o estado dos gatilhos do<br/>
mouse o que foi resolvido usando o metódo `bool:Event.PopEvent(Event)`.
## [1.0.2] 31/07/2022
Agora a classe `CobilasResolutions` é MonoBehaviour.
## [1.0.1] 27/07/2022
### Fixed (IGUDrawer)
Ao remover um objeto `IGUContainer` ele não era removido do evento OnIGU.
## [1.0.0] 25/07/2022
### Beta do repositorio com.cobilas.unity.graphics finalizado.
- Lançado para o GitHub
## [0.1.0] 15/07/2022
### Repositorio com.cobilas.unity.graphics iniciado
- Lançado para o GitHub