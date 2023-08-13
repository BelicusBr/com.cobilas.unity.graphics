using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUClip {
        Rect RectView { get; set; }
        Vector2 ScrollView { get; set; }

        void AddClip(IGUObject item);
        void RemoveClip(IGUObject item);
    }
}
