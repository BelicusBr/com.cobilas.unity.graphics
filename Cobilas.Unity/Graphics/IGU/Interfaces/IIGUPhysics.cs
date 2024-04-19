using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUPhysics {
        IGUConfig LocalConfig { get; }
        bool IsPhysicalElement { get; set;}
        IGUBasicPhysics Physics { get; set; }
        void CallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys);
    }
}