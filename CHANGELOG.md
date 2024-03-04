The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

# [4.5.2] (03/03/2024)
## Fixed
The issue in the `IGUDepthDictionary.ReorderDepthDictionary(IGUDepthDictionary[])` method that caused `ArgumentNullException` when reordering the depth list has been fixed.
## Changed
The `IGUCanvas.OnIGU`, `IGUCanvas.OnEndOfFrame`, and `IGUCanvas.OnToolTip` properties have been removed so that events better respect the depth of IGU elements.
## Removed
The `IGUCanvas.LoadWhenSceneActivates` depth has been removed and replaced with the `IGUCanvas.Status` property.
## Added
`IGUCanvas.Deeps` and `IGUCanvas.Status` properties.
`IGUCanvas.Clear()` method to clear the depth list.

# [4.5.1] (02/28/2024)
## Changed
- In `ToolTip` drawing methods:
- - It is now checked whether the IGU element is visible.
- - `ToolTip` text must not be empty or null.
### Comments
This change ensures that the `ToolTip` is only drawn if the IGU element is visible and the `ToolTip` text is valid.

# [4.5.0] (28/02/2024)
## Added
Class `IGUActionManager<A, T>`: manages the admission of actions for specified objects.
The class only accepts actions from objects that have not been registered.
### Comments
The `IGUActionManager<A, T>` class provides a more organized and efficient way to manage object actions.
The class ensures that only registered object actions are performed.

# [4.4.1] (02/26/2024)
## Removed
`IGUUtilityDistortion` class. \
`DoNotModifyRect` structure. \
`IGURect.ModifiedSize`, `IGURect.ModifiedPosition`, `IGURect.ModifiedUp`, `IGURect.ModifiedDonw`, `IGURect.ModifiedRight`, `IGURect.ModifiedLeft`, and `IGURect.ModifiedCenter` properties: Replaced by the `IGURect.ModifiedRect` property. \
`IGURect.SetModifiedPosition(vector2)` and `IGURect.SetModifiedSize(Vector2)` methods. \
`HashCodeCompare` class. \
`IGUDrawer.GetMousePosition()` method: replaced by the IGUDrawer.MousePosition property. \
Methods `IGUObject.CreateIGUInstance<T>(string)` and `IGUObject.CreateIGUInstance(Type, string)`: replaced by `IGUObject.Create<T>(string)` and `IGUObject.Create(Type, string)`. \
`DeepAction` and `IGUContainer` classes: Replaced by `IGUCanvas`, `IGUCanvasContainer` and `IGUDepthDictionary`. \
Class `IGUBehaviour`. \
`IGUToolTip` class. \
## Changed
In the `IGURect.SetPivot(float, float)` method, input values now pass through a `Mathf.Clamp01(float)`. \
In the `IGURect.SetRotation(float)` method, if the input value exceeds `+360f` or `-360f`, it returns to `0f`.
## Added
`IGURepeatButton.OnRepeatClick` property: to be used as an event. \
Classes `IGUTextFieldStringEvent`, `IGUCanvas`, `IGUCanvasContainer`, `IGUDepthDictionary` and interfaces `IIGUToolTip` and `IIGUEndOfFrame`.

# [2.3.0] (24/02/2024)
## Removed
Protected methods `IGUObject.GetRect(bool)` and `IGUObject.GetRect()`: replaced by the `IGUObject.LocalRect` property. \
`IGULabel.RichText` property. \
`IGULabel.SetMarkedText(params MarkedText[])` method.
## Renamed
`IGUObject.MyConfg` property for `IGUObject.MyConfig`. \
`IGUObject.myConfg` protected field for `IGUObject.myConfig`.
## Updated
`IGUSelectionGrid` class has been updated to use layout classes internally.
## Replaced
`IGUObject.Ignition()`, `IGUObject.Start()`, `IGUObject.IgnitionEnable()`, `IGUObject.IgnitionDisable()` and `IGUObject.DestroyIgnition()` methods:
```
IGUObject.Ignition() >> IGUObject.IGUAwake()
IGUObject.Start() >> IGUObject.IGUStart()
IGUObject.IgnitionEnable() >> IGUObject.IGUOnEnable()
IGUObject.IgnitionDisable() >> IGUObject.IGUOnDisable()
IGUObject.DestroyIgnition() >> IGUObject.IGUOnDestroy()
```
`IGUTextObject.GetGUIContent(string)` method: \
Use the explicit conversion operator from the `IGUContent` class.
## Added
`IGUObject.LocalConfig` property: obtains the IGU element settings, taking into account the parent IGU element. \
Explicit string to `IGUStyle` conversion: enter the style name to load it. \
`IGUTextObject.RichText` property. \
`IGUTextObject.SetMarkedText(params MarkedText[])` method.
### New methods:
```c#
IGUTextObject.GetGUIContentTemp(Texture, string);
IGUTextObject.GetGUIContentTemp(Texture);
IGUTextObject.GetIGUContentTemp(string, Texture, string);
IGUTextObject.GetIGUContentTemp(string, string);
IGUTextObject.GetIGUContentTemp(string, Texture);
IGUTextObject.GetIGUContentTemp(Texture, string);
IGUTextObject.GetIGUContentTemp(Texture);
IGUTextObject.GetIGUContentTemp(string);
```

# [4.2.5] (23/02/2024)
## Changed
### Update dependency `com.cobilas.unity.management.runtime`:
The dependency `com.cobilas.unity.management.runtime` has been updated to version `2.2.2`.
This update was made due to fixes made to the `SceneContainerManager` class, which is used by the `IGUDrawer` class.
## Fixed
### Problem in the `IGUCheckBox` class:
The problem in the `IGUCheckBox` class when changing the `Checked` state has been fixed.
The problem was that the `Checked` state was not changing correctly because the function checked which mouse trigger was fired before changing the state.
The fix ensures that the `Checked` state changes correctly when the user clicks the checkbox.
## Added
### New classes:
`BackEndIGU` and `IGUUtilityDistortion` classes have been added.

# [4.2.4] (22/02/2024)
## Fixed
Problem in the static method `IGUContainer.CreatePermanentGenericIGUContainer()`.
### Details
The container generated by the static method `IGUContainer.CreatePermanentGenericIGUContainer()` did not become a non-destructible object of the scene.
This meant that the container could be destroyed during the game`s scene change, which could cause problems.
The fix involved modifying the `IGUContainer.CreatePermanentGenericIGUContainer()` method to ensure that the generated container becomes a non-destructible scene object.

# [4.2.3] (21/02/2024)
## Fixed
### Fixed#1
Problem in the `IGUVerticalScrollbar` and `IGUHorizontalScrollbar` classes.
#### Details
The `IGUObject.LowCallOnIGU()` method in the `IGUVerticalScrollbar` and `IGUHorizontalScrollbar` classes passed the `IGUSliderObject.Value` property through a `Mathf.Clamp(float, float, float)`.
However, this function did not take into account the `ScrollbarThumbSize` property of the `IGUVerticalScrollbar` and `IGUHorizontalScrollbar` classes.
The fix involved modifying the `IGUObject.LowCallOnIGU()` method to take the `ScrollbarThumbSize` property into account.
### Fixed#2
Problem in the `IGUScrollView` class.
#### Details
The `IGUScrollView` class previously passed the value of the `IGUSliderObject.Value` property of the `IGUVerticalScrollbar` and `IGUHorizontalScrollbar` classes to the RectClip without checking whether objects of type `IGUVerticalScrollbar` and `IGUHorizontalScrollbar` were visible.
This could cause the RectClip to be positioned incorrectly.
The fix involved adding a check to the `IGUScrollView` class code to ensure that objects of type `IGUVerticalScrollbar` and `IGUHorizontalScrollbar` are visible before passing the value of the `IGUSliderObject.Value` property to the RectClip.

# [4.2.2] (21/02/2024)
## Fixed
Problem in the `IGUComboBoxDrawer` drawing class.
### Details
The `IGUComboBoxDrawer` drawing class needed to be updated due to a change in the `IGUComboBox` class. \
The fix ensures that the `IGUComboBoxDrawer` class works correctly with the updated version of the `IGUComboBox` class.

# [4.2.1] (20/02/2024)
## Fixed
### Event `IGUScrollView.OnScrollView`:
The `IGUScrollView.OnScrollView` event was not working correctly due to a problem with the function call order.
The fix ensures that the `IGUScrollView.OnScrollView` event is called at the correct time.
## Added
### Property `IGURectClip.AutoInvert`:
The `IGURectClip.AutoInvert` property has been added to automatically invert the `IGURectClip.ScrollView` property.
This property makes it easier to configure `IGURectClip` to work with `IGUScrollView`.
## Removed
### Inheritance of the `IIGUSerializationCallbackReceiver` interface in the `IGUComboBox` class:
The `IGUComboBox` class no longer inherits from the `IIGUSerializationCallbackReceiver` interface.
The `IGUComboBox.IgnitionEnable()` method fulfills the same function as the interface.

# [4.1.1] (19/02/2024)
## Fixed
Class `IGUComboBox`: \
Updating the `IGUComboBox` class resolved issues that made it unfeasible.
## Added
### Abstract class `IGULayout`:
- New abstract methods:
- - `IGULayout.Remove(int)`
- - `IGULayout.Remove(int, bool)`
- - `IGULayout.Remove(IGUObject, bool)`
### Class `IGUScrollView`:
- New properties:
- - `IGUScrollView.VerticalScrollbarIsVisible`
- - `IGUScrollView.HorizontalScrollbarIsVisible`

# [4.0.0] (18/02/2024)
## Changed
Replacement of methods in `IGUObject`.
### Details
The following methods on `IGUObject` have been deprecated: \
`Awake()` by `Ignition()` \
`OnEnable()` by `IgnitionEnable()` \
`OnDisable()` by `IgnitionDisable()` \
`OnIGUDestroy()` by `DestroyIgnition()`
| Old method | New method | Description |
|---|---|---|
|`Awake()`|`Ignition()`| Called before the first frame is updated. |
|`OnEnable()`|`IgnitionEnable()`| Called when the object is activated. |
|`OnDisable()`|`IgnitionDisable()`| Called when the object is deactivated. |
|`OnIGUDestroy()`|`DestroyIgnition()`| Called when the object is destroyed. |

# [3.5.5] (14/02/2024)
## Fixed
Fixed an issue that caused `ScriptableObjects` that depended on the `IGUStyleCustom` and `SO_IGUTextSettings` classes to malfunction.
### Details
- The metadata of the `IGUStyleCustom` and `SO_IGUTextSettings` classes was accidentally changed by the editor. \
This change affected `ScriptableObjects` that depended on these classes, causing compilation errors.
- **_Solution_**: The metadata of the `IGUStyleCustom` and `SO_IGUTextSettings` classes has been restored to the previous version.
### Affected versions
This issue has been present since version `3.5.0` and affects all versions prior to `3.5.5`.
## Changed
The dependency `com.cobilas.unity.utility` has been updated from version `2.10.3` to version `2.10.4`. \
The dependency `com.cobilas.unity.core.net4x` has been updated from version `1.4.1` to version `1.4.2`.

# [3.5.4] (10/02/2024)
## Fixed
In the internal method `IGUDrawer.RemoveReserialization(IGUObject)` a null argument exception could occur, this occurred when the method first checked whether the object `IGUObject` existed in a list that could be null.

# [3.5.3] - 05/02/2024
## Changed
Updated dependency `com.cobilas.unity.utility` to version `2.10.3`. \
This update includes bug fixes and new features that do not directly impact this package. \
The sub-dependency `com.cobilas.unity.core.net4x@1.4.1` was made explicit in the package dependencies.
# [3.5.2] 28/01/2024
## Fixed
The `IGUDrawer` and `CobilasResolutions` classes inherit the `ISceneContainerItem` interface so they can be added to the SceneContainerManager.
# [3.5.1] 28/01/2024
## Fixed
The `IGUStyleStatus.ScaledBackgrounds` property caused compilation errors because it depended on the `GUIStyleState.ScaledBackgrounds` property, which in turn is exclusive to the editor.
# [3.5.0] 26/01/2024
## Changed
A change in package dependencies.
# [3.4.1] - 08/09/2023
##Fixed
- The `AddToPermanentContainer` attribute in the `CobilasResolutions` and `IGUDrawer` classes has been replaced by `[AddSceneContainer]`.
- The <kbd>Cobilas.Unity.Utility</kbd> assembly reference has been added to <kbd>Cobilas.Unity.Graphics.asmdef</kbd>.
- ps: Lack of attention in this XD.
# [3.4.0] - 08/09/2023
## Changed
- package dependencies have been changed.
# [3.3.1] - 30/08/2023
##Fixed
- Fixed a dependency that was incorrect.
The dependency in question was `com.cobilas.unity.management.runtime@1.15.0-ch1` which did not exist.
# [3.3.0] - 29/08/2023
# Changed
- Package dependencies have been changed.
# [3.2.6-ch1] - 28/08/2023
## Changed
- The package author was changed from `Cobilas CTB` to `BélicusBr`.
# [3.2.5] - 22/04/2023
##Fixed
- There was a visual error that occurred when two `IGUWindow` elements were displayed on the screen due to the fact that the same temporary element was used.
- In the styles `Black text field style` and `Black text field border style` the `Text Clipping` field was marked incorrectly with the value `TextClipping.Overflow` which was changed to `TextClipping.Clip`.
## Deprecated
- The methods `GUIStyle:IGUStyle.GetGUIStyleTemp(IGUStyle)` and `GUIStyle:IGUStyle.GetGUIStyleTemp(IGUStyle, int)` are not supported, use the explicit convention of `IGUStyle`.
# [3.2.4] - 18/04/2023
##Fixed
- When calling the `IGUScrollView.ScrollViewAction` event, doNots will not be activated.
# [3.2.3] - 18/04/2023
##Fixed
- The protected field `IGUComboBox.scrollViewBackgroundStyle` was not initialized which caused a null reference.
- Now checking whether the mouse is inside the `IGUComboBox` has been fixed.
- The protected properties `IGUScrollView.alwaysShowVertical` and `IGUScrollView.alwaysShowHorizontal` are not used.
- The positioning of the scrolls has been corrected.
# [3.2.0] - 17/04/2023
## Changed
- Reworked all `CustomPropertyDrawer` classes of `IGUObject` elements.
# [3.1.1] - 17/04/2023
##Fixed
- Fixed problem in the `IIGUObject.internalOnIGU` method of `IGUObjetc` which was incorrectly checking the `IGUObject.Pivot` property. Now only `IGUObject.PivotX` is set correctly.
- Fixed an unexpected error that occurred during the serialization and deserialization process that caused the `GUIStyle tooltipStyle` field to become `GUIStyle.none` when replacing `GUIStyle` with `IGUStyle`.
## Added
- Added protected method `IGUObjetc.LowCallOnIGU()` as new internal process `OnIGU`.
- Added the `IGURectClip` class to make clippings.
## Removed
- Removed methods:
   - `IGUObject.GetDefaultValue(GUIStyle, GUIStyle)`
   - `IGUScrollView.ScrollTo(IGUObject)`
   - `IGUScrollView.ScrollTo(IGURect)`
   - `IGUScrollView.ScrollTo(Rect)`
# [2.3.0] - 15/04/2023
## Added
- Added the `IGUStyle` style class.
# [2.2.1] - 15/04/2023
## Changed
Now the IGUConfig and IGUColor functions are executed in the `void:IIGUObject.InternalOnIGU()` method of the `IGUObject` class.
##Fixed
In the `IGUNumericBox` and `IGUNumericBoxInt` classes, the sub-elements were not parented, which did not change the position of the sub-elements.
## Deprecated
The protected method `Vector2:IGUObject.GetPosition()` is not supported.
## Added
The property `IGUObject.GlobalRect { get; set; }` represents the local position of the `IGUObject` element added to the parent `IGUObject` element.
The protected methods
```c#
protected Rect GetRect(bool iginoreNotMod);
protected Rect GetRect();
```
were added to obtain the position and size.
# [2.1.1] - 14/04/2023
## Changed
Now the properties `IGUTextObject.UseTooltip`, `IGUTextObject.MyContent` and `IGUObject.MyConfg` receive a default value.
# [2.1.0-rc1] - 08/04/2023
## Added
The `IIGUSerializationCallbackReceiver` interface has been added to serialize `IGUObject` in the Editor.
# [2.0.2] - 04/05/2023
## Fixed(IGU)
In `IGUComboBox` there was a problem that occurred when the object was affiliated with another that caused the position of the `IGUComboBox` not to be changed.
## Removed(IGU)
The `BeginDoNotMofifyRect` and `EndDoNotMofifyRect` functions affect child and sub-child objects, and have been replaced by the doNots compo.
`CreateIGUInstance` methods have been removed from all `IGUObject` classes and replaced with
```c#
IGUObject.CreateIGUInstace(Type);
IGUObject.CreateIGUInstace(Type, string);
IGUObject.CreateIGUInstace<T>(string);
IGUObject.CreateIGUInstace<T>();
```
# [1.7.1] - 12/02/2023
## Added
Now the layout schemes have been added.
```c#
public sealed class IGUHorizontalLayout{}
public sealed class IGUVerticalLayout{}
public sealed class IGUGridLayout{}
```
##Fixed
## IGUObject
In the `IGUConfig:GetModIGUConfig()` method, in addition to checking whether the parent is null, they are also checked
if `parent.GetModIGUConfig().IsEnabled && myConfg.IsEnabled` is `parent.GetModIGUConfig().IsVisible && myConfg.IsVisible`
are true.
In the `Vector2:GetPosition()` method, the parent`s position will be obtained from the `parent.myRect.ModifiedPosition` property
which will cause incorrect positioning.
# [1.0.16] - 12/02/2023
##Fixed
In the `IGUConatiner.AddDeepAction(int)` method, the `RefreshDepth();` instruction was executed before the `DeepAction` object
which meant that the first `DeepAction` object was not added to the OnIGU event.
# [1.0.15] - 09/02/2023
##Fixed
## Depth not applied correctly [#ISU-IGU0001](https://github.com/BelicusBr/com.cobilas.unity.graphics/issues/2)
In the `IGUContainer` class where depth is applied, now when adding a new depth the event responsible for calling `DeepAction.OnIGU()` is updated.
# [1.0.14] - 05/02/2023
##Fixed
In the `void:InitInternalIndowFunction()` method, the `GUI.DragWindow` instruction was executed after the `windowFunction` action, which could cause the window to stop moving due to the fact that
that if any IGU element was disabled or when the `GUI.enabled = false` property was called within the `windowFunction` action.
# [1.0.13] - 30/01/2023
## Changed
- Removal of unused fields.
- Removal of unnecessary assignments.
- Transforming possible fields into `readonly`.
# [1.0.12] 09/01/2023
## Changed
The body of the `public static Vector2Int GetBaseResolutionPlatform()` method has been changed.
# [1.0.9] 06/09/2022
##Fixed
The `IGUEvent` property was removed due to conflicts with the `GUI` class methods.<br/><br/>

In the `IGUDrawer` and `IGUConatiner` classes, there was a problem with the OnIGU events that with each deserialization<br/>
OnIGU events were not cleared which caused IGU elements to be duplicated.

## Changed
Mouse triggers are now detected by the `Imput` class.
# [1.0.7] 02/09/2022
## Fixed (IGUDrawer)
Now the event collected and marked as used.
## Fixed (IGUObjectDrawer)
The statements `property.serializedObject.Update();` and `property.serializedObject.ApplyModifiedProperties();`<br/>
were removed because they caused the values already pre-defined in the inspector to freeze after being changed<br/>
for game mode.
# [1.0.6] 27/08/2022
## Fixed (MarkedText)
In the method `string:MarkedText.ToString();` there was a problem where marking the MarkedText<br/>
like `FontStyle.Normal`, the text did not appear.
## Fixed (IGURect)
In the `Vector2:IGURect.ModifiedPosition` property, the `Vector2:IGURect.ModifiedSize` property is used<br/>
to perform the calculation instead of the `Vector2:IGURect.Size` property as before.
## Added (IGUContainer)
The method
```c#
public static IGUContainer GetOrCreateIGUContainer(string name);
```
creates a new IGUContainer or takes an existing IGUContainer.
## Changed (IGUContainer)
Now the methods `IGUContainer:IGUContainer.CreateGenericIGUContainer();` is `IGUContainer:IGUContainer.CreatePermanentGenericIGUContainer();`<br/>
use the `IGUContainer:IGUContainer.GetOrCreateIGUContainer(string)` method.
## Fixed (IGUDrawer)
In the `void:IGUDrawer.OnGUI()` method, `Event.current` was used to collect the state of the<br/> triggers
mouse, but the problem occurred when the `Event.Use()` method interfered with collecting the state of the <br/> triggers.
mouse which was resolved using the `bool:Event.PopEvent(Event)` method.
# [1.0.2] 31/07/2022
Now the `CobilasResolutions` class is MonoBehaviour.
# [1.0.1] 27/07/2022
## Fixed (IGUDrawer)
When removing an `IGUContainer` object it was not removed from the OnIGU event.
# [1.0.0] 25/07/2022
## Beta of the com.cobilas.unity.graphics repository completed.
- Released to GitHub
# [0.1.0] 15/07/2022
## Com.cobilas.unity.graphics repository started
- Released to GitHub