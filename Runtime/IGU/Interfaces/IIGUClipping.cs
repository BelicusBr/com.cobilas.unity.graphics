using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUClipping {
        bool IsClipping { get; }
        Rect RectView { get; set;}
        Vector2 ScrollView { get; set; }
    }
}