using System;
using UnityEngine;
using UnityEngine.Events;

namespace Cobilas.Unity.Graphics.IGU.Events {
    /// <summary>Evento de rolagem com argumento (<see cref="Vector2"/>).</summary>
    [Serializable]
    public class IGUScrollViewEvent : UnityEvent<Vector2> { }
}
