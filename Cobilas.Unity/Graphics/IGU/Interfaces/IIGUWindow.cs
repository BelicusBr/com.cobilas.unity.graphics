namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUWindow {
        WindowFocusStatus IsFocused { get; }

        int GetInstanceID();
    }
}