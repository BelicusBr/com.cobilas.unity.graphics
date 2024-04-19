using System.Linq;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    public sealed class IGUPhyContainer {
        private IGUObject[] phys;
        private event IGUBasicPhysics.CallPhysicsFeedback result;

        public void RefreshPhysics(IGUObject[] list) {
            for (long I = 0; I < ArrayManipulation.ArrayLongLength(list); I++)
                foreach (IGUObject item in GetAllPhysics(list[I]).Cast<IGUObject>())
                    ArrayManipulation.Add(item, ref phys);
        }

        public IGUBasicPhysics.CallPhysicsFeedback GetCallPhysicsFeedback() {
            result = (IGUBasicPhysics.CallPhysicsFeedback)null;
            for (long I = 0; I < ArrayManipulation.ArrayLongLength(phys); I++)
                result += (phys[I] as IIGUPhysics).CallPhysicsFeedback;
            return result;
        }

        private static IIGUPhysics[] GetAllPhysics(IGUObject obj) {
            IIGUPhysics[] result = new IIGUPhysics[0];
            if (obj.IsPhysicalElement)
                ArrayManipulation.Add(obj, ref result);
            if (obj.Physics is IGUCollectionPhysics cphy)
                for (int I = 0; I < cphy.SubPhysicsCount; I++)
                    ArrayManipulation.Add(GetAllPhysics(cphy.SubPhysics[I].Target), ref result);
            return result;
        }
    }
}