using System;

namespace Cobilas.Unity.Graphics.IGU {
    public struct DoNotModifyRect : IDisposable, IEquatable<bool> {
        private bool pilha;

        public DoNotModifyRect(bool pilha)
        {
            this.pilha = pilha;
        }

        public override int GetHashCode()
            => base.GetHashCode() >> pilha.GetHashCode();

        public bool Equals(bool other)
            => other == pilha;

        public override bool Equals(object obj)
            => obj is bool bl && Equals(bl);

        public void Dispose() => this.pilha = false;

        public static implicit operator bool(DoNotModifyRect A)
            => A.pilha;
    }
}
