using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Layouts {
    [Serializable]
    public sealed class CellIGUObject : IEquatable<IGUObject>, IDisposable {
        public IGUObject @object;
        [SerializeField]
        private Vector2 mySize;
        [SerializeField, HideInInspector]
        private bool click;

        public CellIGUObject(IGUObject @object) {
            this.@object = @object;
            this.mySize = @object.MyRect.Size;
            this.click = true;
        }

        public override bool Equals(object obj)
            => obj is IGUObject iguobj && Equals(iguobj);

        public bool Equals(IGUObject other)
            => this.@object == other;

        public override int GetHashCode()
            => @object.GetHashCode();

        public void OnIGU(CellCursor cursor) {
            if (cursor.UseCellSize) {
                if (click) {
                    click = false;
                    mySize = @object.MyRect.Size;
                }
                @object.MyRect = @object.MyRect.SetSize(cursor.CellSize);
            } else {
                if (!click) {
                    click = true;
                    @object.MyRect = @object.MyRect.SetSize(mySize);
                }
            }
            cursor.MarkCount(@object);
            @object.OnIGU();
        }

        public void Dispose() {
            @object = (IGUObject)null;
            mySize = Vector2.zero;
        }

        public static bool operator ==(CellIGUObject A, IGUObject B)
            => (object)A != null && (object)B != null && A.Equals(B);

        public static bool operator !=(CellIGUObject A, IGUObject B)
            => !(A == B);

        public static bool operator ==(IGUObject A, CellIGUObject B)
            => (object)A != null && (object)B != null && B.Equals(A);

        public static bool operator !=(IGUObject A, CellIGUObject B)
            => !(A == B);
    }
}
