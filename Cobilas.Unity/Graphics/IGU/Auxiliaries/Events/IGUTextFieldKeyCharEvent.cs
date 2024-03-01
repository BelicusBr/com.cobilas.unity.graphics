using System;
using UnityEngine.Events;

namespace Cobilas.Unity.Graphics.IGU.Events {
    /// <summary>Evento de teclado com argumento (<see cref="char"/>).</summary>
    [Serializable]
    public class IGUTextFieldKeyCharEvent : UnityEvent<char> { }
}
