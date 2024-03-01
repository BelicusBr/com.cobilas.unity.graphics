using System;
using UnityEngine.Events;

namespace Cobilas.Unity.Graphics.IGU.Events {
    /// <summary>Evento de rolagem com argumento (<see cref="int"/>).</summary>
    [Serializable]
    public class IGUOnSliderIntValueEvent : UnityEvent<int> { }
}

