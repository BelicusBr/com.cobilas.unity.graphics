using System;
using UnityEngine;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.Unity.Graphics.IGU.Events {
    public class DIGUAction<T> : IGUActionManager<Action, T> {
        private event Action funcition;
        private Func<T, Action> refreshFunction;
        private T[] registeredItem;

        public override Action Function => funcition;
        public override int RegisteredCount => ArrayManipulation.ArrayLength(registeredItem);
        public override Func<T, Action> RefreshFunction { get => refreshFunction; set => refreshFunction = value; }

        public override T this[int index] => registeredItem[index];

        public override bool Contains(T item) {
            for (int I = 0; I < RegisteredCount; I++)
                if (EqualityComparer<T>.Default.Equals(item, this[I]))
                    return true;
            return false;
        }

        public override void Clear() {
            funcition = (Action)null;
            ArrayManipulation.ClearArraySafe(ref registeredItem);
        }

        public static DIGUAction<T> operator +(DIGUAction<T> A, T item) {
            if (!A.Contains(item)) {
                A.funcition += A.refreshFunction(item);
                ArrayManipulation.Add(item, ref A.registeredItem);
            }
            return A;
        }

        public static DIGUAction<T> operator -(DIGUAction<T> A, T item) {
            if (A.Contains(item)) {
                A.funcition -= A.refreshFunction(item);
                ArrayManipulation.Remove(item, ref A.registeredItem);
            }
            return A;
        }
    }
}