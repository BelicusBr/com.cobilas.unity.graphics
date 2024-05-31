using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUClipping {
        Rect RectView { get; set; }
        bool AutoInvert { get; set; }
        bool RenderOffSet { get; set; }
        Vector2 ScrollView { get; set; }
    }
}