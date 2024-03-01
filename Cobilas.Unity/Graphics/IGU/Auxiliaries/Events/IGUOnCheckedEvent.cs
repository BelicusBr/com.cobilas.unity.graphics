using System;
using UnityEngine.Events;

namespace Cobilas.Unity.Graphics.IGU.Events {
    /// <summary>Evento de clique com argumento(<see cref="bool"/>).</summary>
    [Serializable]
    public class IGUOnCheckedEvent : UnityEvent<bool> { }
}
