/*
using System;
using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    using MMSTemp = MaxMinSliderDraw.MaxMinSliderTemp;
    using MMSITemp = MaxMinSliderIntDraw.MaxMinSliderIntTemp;

    [IGUCustomDrawer(typeof(IGUSliderObject), true)]
    public class IGUSliderObjectDraw : IGUObjectDraw {
        private MaxMinSliderDraw maxMinSliderDraw = new MaxMinSliderDraw();
        private MaxMinSliderIntDraw maxMinSliderIntDraw = new MaxMinSliderIntDraw();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => base.OnGUI(position, property, label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => base.GetPropertyHeight(property, label);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override Type GetMyType() => typeof(IGUSliderObject);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);
        
        //[SerializeField] protected bool isInt;
        //[SerializeField] protected float value;
        //[SerializeField] protected MaxMinSlider maxMinSlider;
         
        protected override Rect OnGUI(Rect position, IGUObject iGUObject) {
            position = base.OnGUI(position, iGUObject);
            IGUSliderObject sliderObject = iGUObject as IGUSliderObject;

            if (sliderObject.IsInt = EditorGUI.Toggle(position = MoveDownWithBlankSpace(position), GetGUIContent("Is int"), sliderObject.IsInt))
                sliderObject.ValueToInt = EditorGUI.IntField(position = MoveDownWithBlankSpace(position), GetGUIContent("Value"), sliderObject.ValueToInt);
            else sliderObject.Value = EditorGUI.FloatField(position = MoveDownWithBlankSpace(position), GetGUIContent("Value"), sliderObject.Value);

            DrawBackgroundProperty(position = MoveDownWithBlankSpace(position), maxMinSliderDraw.GetPropertyHeight(sliderObject.MaxMinValue.foldout) + BlankSpace);
            EditorGUI.indentLevel++;
            if (sliderObject.IsInt) {
                MMSITemp temp;
                maxMinSliderIntDraw.OnGUI(position, temp = MMSITemp.GetMaxMinSliderTemp(sliderObject.MaxMinValue.ToMaxMinSliderInt()), GetGUIContent("Min max"));
                sliderObject.MaxMinValue = temp.OutputValue;
            } else {
                MMSTemp temp;
                maxMinSliderDraw.OnGUI(position, temp = MMSTemp.GetMaxMinSliderTemp(sliderObject.MaxMinValue), GetGUIContent("Min max"));
                sliderObject.MaxMinValue = temp.OutputValue;
            }
            EditorGUI.indentLevel--;
            return position;
        }

        protected override float GetPropertyHeight(IGUObject iGUObject)
            => base.GetPropertyHeight(iGUObject) +
            (SingleRowHeightWithBlankSpace * 2f) + maxMinSliderDraw.GetPropertyHeight((iGUObject as IGUSliderObject).MaxMinValue.foldout);
    }
}
*/