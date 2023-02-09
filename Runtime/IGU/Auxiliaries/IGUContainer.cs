using System;
using UnityEngine;
using Cobilas.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    //PersistentDataPath
    public class IGUContainer : IGUBehaviour, IIGUContainer, ISerializationCallbackReceiver {
        [SerializeField] private DeepAction[] deepActions;
        private bool wasDestroyed;
        private Action onIGU;
        private Action<List<IIGUObject>> alteredDepth;
        private readonly List<IIGUObject> AlteredDepthList = new List<IIGUObject>();

        void IIGUContainer.OnIGU() {
            onIGU?.Invoke();
            alteredDepth?.Invoke(AlteredDepthList);

            if (AlteredDepthList.Count != 0) {
                for (int A = 0; A < AlteredDepthList.Count; A++)
                    for (int I = 0; I < ArrayManipulation.ArrayLength(deepActions); I++)
                        if (deepActions[I].Remove(AlteredDepthList[A] as IGUObject)) {
                            Add(AlteredDepthList[A] as IGUObject);
                            //if (deepActions[I].Count == 0) {
                            //    onIGU -= deepActions[I].OnIGU;
                            //    alteredDepth -= deepActions[I].AlteredDepth;
                            //    ArrayManipulation.Remove(I, ref deepActions);
                            //}
                            //A = AlteredDepthList.Count + 1;
                            break;
                        }
                AlteredDepthList.Clear();
            }
        }

        public void Add(IGUObject item) {
            if (!IsRegistered(item)) {
                item.Container = this;
                AddDeepAction(item.MyConfg.Depth).Add(item);
            }
        }

        public bool Remove(IGUObject item) {
            DeepAction deep = GetDeepAction(item.MyConfg.Depth);
            if (deep != null)
                if (deep.Remove(item)) {
                    item.Container = null;
                    return true;
                }
            return false;
        }

        public void DestroyContainer() {
            wasDestroyed = true;
            IGUDrawer.Drawer.Remove(this);
            this.DestroyMyGameObject();
        }

        private void OnDestroy() {
            if (wasDestroyed) return;
            IGUDrawer.Drawer.Remove(this);
        }

        private bool IsRegistered(IGUObject item) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(deepActions); I++)
                if (deepActions[I].Contains(item))
                    return true;
            return false;
        }

        private bool ContainsDepth(int depth) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(deepActions); I++)
                if (deepActions[I].Depth == depth)
                    return true;
            return false;
        }

        private DeepAction AddDeepAction(int depth) {
            if(ContainsDepth(depth)) return GetDeepAction(depth);
            DeepAction deep;
            if (!ArrayManipulation.EmpytArray(deepActions))
                for (int I = 0; I < ArrayManipulation.ArrayLength(deepActions); I++)
                    if (depth < deepActions[I].Depth) {
                        deep = new DeepAction(depth);
                        //onIGU += deep.OnIGU;
                        RefreshDepth();
                        alteredDepth += deep.AlteredDepth;
                        ArrayManipulation.Insert(deep, I, ref deepActions);
                        return deep;
                    }
            deep = new DeepAction(depth);
            //onIGU += deep.OnIGU;
            RefreshDepth();
            alteredDepth += deep.AlteredDepth;
            ArrayManipulation.Add(deep, ref deepActions);
            return deep;
        }

        private void RefreshDepth() {
            onIGU = (Action)null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(deepActions); I++)
                onIGU += deepActions[I].OnIGU;
        }

        private DeepAction GetDeepAction(int depth) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(deepActions); I++)
                if (deepActions[I].Depth == depth)
                    return deepActions[I];
            return null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            onIGU = (Action)null;
            alteredDepth = (Action<List<IIGUObject>>)null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(deepActions); I++) {
                onIGU += deepActions[I].OnIGU;
                alteredDepth += deepActions[I].AlteredDepth;
            }
        }

        public static IGUContainer CreateIGUContainer(string name) {
            GameObject game = new GameObject(name);
            game.SetPosition(Vector3.zero);
            game.isStatic = true;
            IGUContainer temp = game.AddComponent<IGUContainer>();
            IGUDrawer.Drawer.Add(temp);
            return temp;
        }

        public static IGUContainer CreateGenericIGUContainer()
            => GetOrCreateIGUContainer("Generic container");

        public static IGUContainer CreatePermanentGenericIGUContainer() {
            IGUContainer temp = GetOrCreateIGUContainer("Permanent generic container");
            if (temp == null)
                DontDestroyOnLoad(temp.gameObject);
            return temp;
        }

        public static IGUContainer GetIGUContainer(string name) {
            IGUContainer[] containers = GetAllIGUContainers();
            for (int I = 0; I < ArrayManipulation.ArrayLength(containers); I++)
                if (containers[I].name == name)
                    return containers[I];
            return (IGUContainer)null;
        }

        public static IGUContainer GetOrCreateIGUContainer(string name) {
            IGUContainer res = GetIGUContainer(name);
            return res == (IGUContainer)null ? CreateIGUContainer(name) : res;
        }

        public static IGUContainer[] GetAllIGUContainers() => FindObjectsOfType<IGUContainer>();
    }
}
