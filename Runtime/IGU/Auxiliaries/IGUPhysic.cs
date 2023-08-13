using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Physics
{
    public class IGUPhysic {
        public Rect rect;
        public bool isHotPotato;
        public IGUObject target;

        public IGUPhysic(IGUObject iGUObject) {
            rect = Rect.zero;
            isHotPotato = false;
            this.target = iGUObject;
        }

        public bool CheckHotPotato(Vector2 mousePosition) {
            if (target != null && target.Parent != null) {
                if (target.Parent is IIGUClip && target.Parent.MyPhysic.isHotPotato) {
                    Rect rectParent = target.Parent.MyPhysic.rect;
                    rectParent.position = Vector2.zero;
                    float right = rect.xMax - rectParent.xMax;
                    float left = rectParent.xMin - rect.xMin;
                    float top = rectParent.yMin - rect.yMin;
                    float bottom = rect.yMax - rectParent.yMax;

                    rect.width -= Mathf.Clamp(left, 0, rect.width) + Mathf.Clamp(right, 0, rect.width);
                    rect.height -= Mathf.Clamp(top, 0, rect.height) + Mathf.Clamp(bottom, 0, rect.height);

                    return rect.Contains(mousePosition);
                }
                return false;
            }
            return rect.Contains(mousePosition);
        }
    }
}
