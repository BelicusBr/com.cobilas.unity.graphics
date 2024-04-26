using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUClippingPhysics : IIGUPhysics {
        bool AddOtherPhysics(IGUObject obj);
        bool RemoveOtherPhysics(IGUObject obj);
    }
}