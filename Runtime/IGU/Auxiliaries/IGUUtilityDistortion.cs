using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    public static class IGUUtilityDistortion {
        private static bool isDistortion;
        private static Matrix4x4 oldMatrix;

        public static bool IsDistortion => isDistortion;

        public static void Begin(IGURect rect) {
            if (IsDistortion)
                throw new InvalidOperationException("The distortion process has not been completed.");
            isDistortion = true;
            oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(rect.Rotation, rect.ModifiedPosition);
            GUIUtility.ScaleAroundPivot(rect.ScaleFactor, rect.ModifiedPosition);
        }

        public static void End() {
            if (!isDistortion)
                throw new InvalidOperationException("The distortion process has not been initialized.");
            isDistortion = false;
            GUI.matrix = oldMatrix;
        }
    }
}