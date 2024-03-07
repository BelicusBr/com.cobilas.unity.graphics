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
        [SerializeField] private bool isFocusedWindow;
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool foldout;
        #endif

        public int Depth => depth;
        public bool IsFocusedWindow => isFocusedWindow;
        public int Count => ArrayManipulation.ArrayLength(objects);

        public IGUObject this[int index] => objects[index];

        public IGUDepthDictionary(int depth, bool isFocusedWindow)
        {
            this.depth = depth;
            this.isFocusedWindow = isFocusedWindow;
        }

        public IGUDepthDictionary(int depth) : this(depth, false) {}

        public void Add(IGUObject @object)
            => ArrayManipulation.Add(@object, ref objects);

        public void Remove(IGUObject @object) {
            if (objects != null)
                ArrayManipulation.Remove(@object, ref objects);
        }

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

        //correção no método
        public static IGUDepthDictionary[] ReorderDepthDictionary(IGUDepthDictionary[] list) {
            IGUDepthDictionary[] res = new IGUDepthDictionary[ArrayManipulation.ArrayLength(list)];
            List<IGUDepthDictionary> temp = new List<IGUDepthDictionary>(list);
            for (int I = 0; I < res.Length; I++) {
                for (int J = 0; J < temp.Count; J++) {
                    if (res[I] == null) res[I] = temp[J];
                    else res[I] = temp[J].depth < res[I].depth ? temp[J] : res[I];
                }
                _ = temp.Remove(res[I]);
            }
            return res;
        }
    }
}