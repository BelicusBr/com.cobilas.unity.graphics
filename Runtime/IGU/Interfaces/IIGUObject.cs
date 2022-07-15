using System.Collections.Generic;

namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUObject {
        string name { get; set; }

        int GetInstanceID();
        void InternalOnIGU();
        void AlteredDepth(List<IIGUObject> changed, int depth);
    }
}
