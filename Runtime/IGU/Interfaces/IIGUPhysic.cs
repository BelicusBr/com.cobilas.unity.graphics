using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUPhysic {
        IGUPhysic MyPhysic { get; }
        void CallPhysicsFeedback();
    }
}
