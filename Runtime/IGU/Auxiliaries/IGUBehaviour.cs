using Cobilas.Unity.Mono;

namespace Cobilas.Unity.Graphics.IGU {
    public abstract class IGUBehaviour : CobilasBehaviour {
        protected virtual void Start() { }
        protected virtual void Awake() { }
        protected virtual void OnGUI() { }
    }
}
