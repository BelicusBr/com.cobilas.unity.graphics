using System;
using UnityEngine;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUDepthDictionary : IEnumerable<IGUObject> {
        [SerializeField] private int depth;
        [SerializeField] private IGUObject[] objects;

        public int Depth => depth;
        public int Count => ArrayManipulation.ArrayLength(objects);

        public IGUObject this[int index] => objects[index];

        public IGUDepthDictionary(int depth)
        {
            this.depth = depth;
        }

        public void Add(IGUObject @object)
            => ArrayManipulation.Add(@object, ref objects);

        public void Remove(IGUObject @object)
            => ArrayManipulation.Remove(@object, ref objects);

        public bool Contains(IGUObject @object) {
            for (int I = 0; I < Count; I++)
                if (objects[I] == @object)
                    return true;
            return false;
        }

        public void Clear()
            => ArrayManipulation.ClearArraySafe(ref objects);

        public IEnumerator<IGUObject> GetEnumerator()
            => new ArrayToIEnumerator<IGUObject>(objects);

        IEnumerator IEnumerable.GetEnumerator()
            => new ArrayToIEnumerator<IGUObject>(objects);

        public static IGUDepthDictionary[] ReorderDepthDictionary(IGUDepthDictionary[] list) {
            IGUDepthDictionary[] res = new IGUDepthDictionary[ArrayManipulation.ArrayLength(list)];
            List<IGUDepthDictionary> temp = new List<IGUDepthDictionary>(list);
            int index = 0;
            while (temp.Count != 0) {
                for (int I = index; I < res.Length; I++) {
                    if (res[I] == null) res[I] = list[I];
                    else res[I] = temp[I].depth < res[I].depth ? temp[I] : res[I];
                }
                _ = temp.Remove(res[index]);
                index++;
            }
            temp.Capacity = 0;
            return res;
        }
    }
}