using System;
using UnityEngine;
using UnityEngine.Events;

namespace Cobilas.Unity.Graphics.IGU.Events {
    /// <summary>Evento de teclado com argumento (<see cref="KeyCode"/>).</summary>
    [Serializable]
    public class IGUTextFieldKeyCodeEvent : UnityEvent<KeyCode> { }
}
