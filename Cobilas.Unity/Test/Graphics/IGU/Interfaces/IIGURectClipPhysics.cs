namespace Cobilas.Unity.Test.Graphics.IGU.Interfaces {
    public interface IIGURectClipPhysics : IIGUPhysics {
        IIGUPhysics[] InternalPhysicsList { get; }

        bool Add(IIGUPhysics physics);
        bool Remove(IIGUPhysics physics);
    }
}