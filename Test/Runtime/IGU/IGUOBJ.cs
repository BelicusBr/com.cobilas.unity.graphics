using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Test.Graphics.IGU {
    public sealed class IGUOBJ : IIGUObject {
        public string name { get; set; }

        public void AlteredDepth(List<IIGUObject> changed, int depth) {
            throw new NotImplementedException();
        }

        public int GetInstanceID()
        {
            throw new NotImplementedException();
        }

        public void InternalOnIGU()
        {
            throw new NotImplementedException();
        }

        public void InternalPostOnIGU()
        {
            throw new NotImplementedException();
        }

        public void InternalPreOnIGU()
        {
            throw new NotImplementedException();
        }
    }
}
