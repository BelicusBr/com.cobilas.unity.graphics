using System;
using UnityEditor;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    public static class IGUPropertyDrawer {
        public delegate void IGUPropertyDrawerCallback(Type type, IGUObjectPropertyDrawer drawer);
        private readonly static Dictionary<Type, IGUObjectPropertyDrawer> list = new Dictionary<Type, IGUObjectPropertyDrawer>();
        private readonly static Dictionary<Type, IGUObjectPropertyDrawer> listSubClass = new Dictionary<Type, IGUObjectPropertyDrawer>();
        private readonly static Dictionary<Type, IGUObjectPropertyDrawer> listUseForChildren = new Dictionary<Type, IGUObjectPropertyDrawer>();
        private readonly static Dictionary<string, PropertyDrawer> listFieldDrawer = new Dictionary<string, PropertyDrawer>();

        [InitializeOnLoadMethod]
        private static void Init() {
            Type[] types = UnityTypeUtility.GetAllTypes();
            for (int I = 0; I < ArrayManipulation.ArrayLength(types); I++) {
                IGUCustomDrawerAttribute att = types[I].GetAttribute<IGUCustomDrawerAttribute>(false);
                IGUCustomFieldDrawerAttribute att2 = types[I].GetAttribute<IGUCustomFieldDrawerAttribute>(false);
                if (att2 != null)
                    listFieldDrawer.Add(att2.TargetField, (PropertyDrawer)Activator.CreateInstance(types[I]));
                if (att != null) {
                    try {
                        IGUObjectPropertyDrawer drawer = (IGUObjectPropertyDrawer)Activator.CreateInstance(types[I]);
                        if (att.UseForChildren) listUseForChildren.Add(att.TypeTarget, drawer);
                        else list.Add(att.TypeTarget, drawer);
                    } catch (Exception e) {
                        UnityEngine.Debug.LogError("Invalid cast|" + types[I]);
                        throw e;
                    }
                }
            }
        }

        public static IGUObjectPropertyDrawer GetPropertyDrawer(Type type) {
            if (list.ContainsKey(type)) return list[type];
            else {
                if (listUseForChildren.ContainsKey(type)) return listUseForChildren[type];
                if (listSubClass.ContainsKey(type)) return listSubClass[type];
                List<Type> list = new List<Type>();
                GetSubClassList(type, list);
                foreach (var item in list) {
                    if (listUseForChildren.ContainsKey(item)) {
                        IGUObjectPropertyDrawer res;
                        listSubClass.Add(type, res = listUseForChildren[item]);
                        return res;
                    }
                }
            }
            return null;
        }

        public static PropertyDrawer GetPropertyFieldDrawer(string targetField) {
            if (listFieldDrawer.ContainsKey(targetField))
                return listFieldDrawer[targetField];
            return (PropertyDrawer)null;
        }

        private static void GetSubClassList(Type typeTarget, List<Type> list) {
            list.Add(typeTarget);
            if (typeTarget != typeof(IGUObject))
                GetSubClassList(typeTarget.BaseType, list);
        }
    }
}
