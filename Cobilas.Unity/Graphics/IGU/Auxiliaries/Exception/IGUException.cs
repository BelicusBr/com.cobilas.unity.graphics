using System;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public class IGUException : Exception
    {
        public IGUException() { }
        public IGUException(string message) : base(message) { }
        public IGUException(string message, Exception inner) : base(message, inner) { }
        protected IGUException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}