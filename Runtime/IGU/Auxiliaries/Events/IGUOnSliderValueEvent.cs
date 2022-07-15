using System;
using UnityEngine.Events;

namespace Cobilas.Unity.Graphics.IGU.Events {
    /// <summary>Evento de rolagem com argumento (<see cref="float"/>).</summary>
    [Serializable]
    public class IGUOnSliderValueEvent : UnityEvent<float> { }
}
