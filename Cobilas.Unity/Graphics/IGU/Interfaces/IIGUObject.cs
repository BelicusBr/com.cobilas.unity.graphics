namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUObject {
        string name { get; set; }

        int GetInstanceID();
        void InternalOnIGU();
        void InternalPreOnIGU();
        void InternalPostOnIGU();
    }
}
