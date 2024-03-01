using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    [Serializable]
    public sealed class IGUMonoPhysics : IGUPhysicsBase {
        [SerializeField] private IGURect myRect;
        [SerializeField] private bool isHotPotato;

        public IGURect MyRect { get => myRect; set => myRect = value; }
        public override bool IsHotPotato { get => isHotPotato; set => isHotPotato = value; }

        public override bool CollisionConfirmed(Vector2 mouse)
            => myRect.ModifiedRect.Contains(mouse);
    }
}