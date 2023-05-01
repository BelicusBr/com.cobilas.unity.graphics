using System;
using System.IO;
using UnityEngine;
using System.Collections;
using Cobilas.Unity.Utility;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Test.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Layouts;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

public class NewBehaviourScript : MonoBehaviour {

    public IGUStyleCustom custom;
    public IGUStyleCustom customThumb;
    public float value;
    public float value2;
    public float min;
    public float max;

    public float size;
    public bool isHoriz;
    public Rect slider1;
    public Rect slider2;

    //public Rect horiz;
    //public Rect horizThumb;
    //public bool mouseThumb;
    //public bool moveMouse;
    //public Vector2 startPos;
    //public Vector2 startPos2;
    //public Vector2 startPosRect;

    private void Start()
    {

    }

    private void OnGUI() {
        #region Temp code
        //Event @event = Event.current;
        //float mintemp;
        //float maxtemp;
        //if (min > max) {
        //    maxtemp = min;
        //    mintemp = max;
        //} else {
        //    mintemp = min;
        //    maxtemp = max;
        //}

        //Rect horizThumbTemp = horizThumb;
        //horizThumbTemp.position += horiz.position;
        //double compri;
        //float movx;
        //float num1;
        //double num2;
        //float num3;

        //bool isHover = horiz.Contains(@event.mousePosition);
        //bool isHoverThumb = horizThumbTemp.Contains(@event.mousePosition);
        //switch (@event.type) {
        //    case EventType.MouseDown:
        //        if (!(mouseThumb = isHoverThumb) && isHover) {
        //            horizThumb.x = @event.mousePosition.x - horiz.x - 7f;

        //            num1 = startPosRect.x + (startPos2 = @event.mousePosition - startPos).x;
        //            compri = Mathf.Abs(max - min);
        //            num2 = horiz.width - horizThumb.width;
        //            num3 = (float)compri / (float)num2;
        //            num1 = (num1 * num3) / ((float)num2 * num3);

        //            value = num1 * (float)compri + mintemp;

        //            startPos = @event.mousePosition;
        //            startPosRect = horizThumb.position;
        //        } else {
        //            startPos = @event.mousePosition;
        //            startPosRect = horizThumb.position;
        //        }
        //        break;
        //    case EventType.MouseDrag:
        //        if (mouseThumb) {
        //            num1 = startPosRect.x + (startPos2 = @event.mousePosition - startPos).x;
        //            compri = Mathf.Abs(max - min);
        //            num2 = horiz.width - horizThumb.width;
        //            num3 = (float)compri / (float)num2;
        //            num1 = (num1 * num3) / ((float)num2 * num3);

        //            value = num1 * (float)compri + mintemp;
        //        }
        //        break;
        //    case EventType.MouseUp:
        //        mouseThumb = false;
        //        break;
        //}
        //value = Mathf.Clamp(value, mintemp, maxtemp);

        //compri = Mathf.Abs(max - min);
        //movx = (horiz.width - horizThumb.width) * (float)(Math.Abs(value - mintemp) / compri);

        //horizThumb.x = Mathf.Clamp(movx, 0f, horiz.width - horizThumb.width);

        //GUI.DrawTexture(horiz, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f, Color.green, Vector4.zero, Vector4.zero);
        //GUI.DrawTexture(horizThumbTemp, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f, Color.red, Vector4.zero, Vector4.zero);
        #endregion

        int ID = GUIUtility.GetControlID(FocusType.Passive, slider1);
        value = TDS_IGUStyle.DrawSlider(slider1, value, size, min, max, ID, custom.Style, customThumb.Style, isHoriz, false, false);

        ID = GUIUtility.GetControlID(FocusType.Passive, slider2);
        value2 = GUI.Slider(slider2, value2, size, min, max, (GUIStyle)custom.Style, (GUIStyle)customThumb.Style, isHoriz, ID);
    }
}