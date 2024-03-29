using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    [Serializable]
    public sealed class IGUMonoPhysics : IGUPhysicsBase {
        [SerializeField] private IGUObject target;
        [SerializeField] private bool isHotPotato;

        public override IGUObject Target { get => target; set => target = value; }
        public override Triangle[] Triangles => throw new NotImplementedException();
        public override bool IsHotPotato { get => isHotPotato; set => isHotPotato = value; }

        public override bool CollisionConfirmed(Vector2 mouse)
            => target != null && target.MyRect.ModifiedRect.Contains(mouse);
    }
}