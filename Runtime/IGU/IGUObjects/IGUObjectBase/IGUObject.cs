using System;
using UnityEngine;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUObject : ScriptableObject, IIGUObject {
        private bool isBuild;
        [SerializeField] protected IGURect myRect;
        [SerializeField] protected IGUColor myColor;
        [SerializeField] protected IGUObject parent;
        [SerializeField] protected IGUConfig myConfig;
        [SerializeField] protected IGUContainer container;
#if UNITY_EDITOR
        [SerializeField] private bool foldout;
#endif

        public IGURect LocalRect => GetLocalPosition(this);
        public IGUConfig LocalConfig => GetLocalConfig(this);
        public IGURect MyRect { get => myRect; set => myRect = value; }
        public IGUObject Parent { get => parent; set => parent = value; }
        public IGUColor MyColor { get => myColor; set => myColor = value; }
        public IGUConfig MyConfig { get => myConfig; set => myConfig = value; }
        public IGUContainer Container { get => container; set => container = value; }

        public void OnIGU() {
            GUI.SetNextControlName(name);
            IGUConfig config = LocalConfig;
            bool oldEnabled = GUI.enabled;
            GUI.enabled = config.IsEnabled;
            if (config.IsVisible) {
                myRect.SetScaleFactor(IGUDrawer.ScaleFactor);
                Vector2 pivot = myRect.Pivot;
                pivot.x = Mathf.Clamp(pivot.x, 0f, 1f);
                pivot.y = Mathf.Clamp(pivot.y, 0f, 1f);
                myRect.SetPivot(pivot);

                if (myRect.Rotation > 360f)
                    myRect.SetRotation(0f);
                if (myRect.Rotation < -360f)
                    myRect.SetRotation(0f);

                (this as IIGUObject).InternalPreOnIGU();
                LowCallOnIGU();
                (this as IIGUObject).InternalPostOnIGU();
            }
            GUI.enabled = oldEnabled;
        }

        public IGUContainer ApplyToContainer(IGUContainer container) {
            container.Add(this);
            return container;
        }

        public IGUContainer ApplyToContainer(string name)
            => ApplyToContainer(IGUContainer.GetOrCreateIGUContainer(name));

        public IGUContainer ApplyToGenericContainer()
            => ApplyToContainer(IGUContainer.CreateGenericIGUContainer());

        public IGUContainer ApplyToPermanentGenericContainer()
            => ApplyToContainer(IGUContainer.CreatePermanentGenericIGUContainer());

        public void RemoveFromContainer() {
            if (container != null)
                container.Remove(this);
        }

        protected virtual void IGUAwake() { }
        protected virtual void IGUStart() { }
        protected virtual void IGUOnEnable() { }
        protected virtual void IGUOnDisable() { }
        protected virtual void IGUOnDestroy() { }
        protected virtual void PreOnIGU() { }
        protected virtual void PostOnIGU() { }
        protected virtual void LowCallOnIGU() { }

        private void Awake() {
            isBuild = true;
        }

        private void OnDestroy() {
            if (container != null)
                if (container.Remove(this))
                    Debug.Log(string.Format("{0} removed from container", name));
            IGUOnDestroy();
            IGUDrawer.RemoveReserialization(this);
        }

        private void OnEnable() {
            if (isBuild) return;
            IGUOnEnable();
        }

        private void OnDisable() => IGUOnDisable();

        void IIGUObject.InternalOnIGU() {

            IGUConfig config = LocalConfig;

            if (config.IsVisible) {
                // myRect.SetScaleFactor(IGUDrawer.ScaleFactor);
                Color oldColor = GUI.color;
                Color oldContentColor = GUI.contentColor;
                Color oldBackgroundColor = GUI.backgroundColor;
                GUI.color = myColor.MyColor;
                GUI.contentColor = myColor.TextColor;
                GUI.backgroundColor = myColor.BackgroundColor;

                OnIGU();

                GUI.color = oldColor;
                GUI.contentColor = oldContentColor;
                GUI.backgroundColor = oldBackgroundColor;
            }
        }

        void IIGUObject.AlteredDepth(List<IIGUObject> changed, int depth) {
            if (myConfig.Depth != depth)
                changed.Add(this);
        }

        void IIGUObject.InternalPreOnIGU() => PreOnIGU();

        void IIGUObject.InternalPostOnIGU() => PostOnIGU();

        public static T CreateIGUInstance<T>() where T : IGUObject
            => (T)CreateIGUInstance(typeof(T));

        public static T CreateIGUInstance<T>(string name) where T : IGUObject
            => (T)CreateIGUInstance(typeof(T), name);

        public static IGUObject CreateIGUInstance(Type type)
            => CreateIGUInstance(type, nameof(IGUObject));

        public static IGUObject CreateIGUInstance(Type type, string name) {
            if (!type.IsSubclassOf(typeof(IGUObject)))
                throw new IGUException($"Class {type.Name} does not inherit from class IGUObject.");
            else if (type.IsAbstract) 
                throw new IGUException("The target class cannot be abstract.");
            IGUObject instance = (IGUObject)CreateInstance(type.Name);
            instance.name = name;
            instance.myConfig = IGUConfig.Default;
            instance.myColor = IGUColor.DefaultBoxColor;
            instance.IGUAwake();
            instance.IGUOnEnable();
            instance.IGUStart();
            instance.isBuild = false;
            if (instance is IIGUSerializationCallbackReceiver)
                IGUDrawer.AddReserialization(instance);
            return instance;
        }

        public static IGUConfig GetLocalConfig(IGUObject obj) {
            if (obj.parent != null) {
                IGUConfig myConfig = obj.myConfig;
                IGUConfig config = GetLocalConfig(obj.parent);
                return myConfig.SetEnabled(myConfig.IsEnabled && config.IsEnabled)
                    .SetVisible(myConfig.IsVisible && config.IsVisible)
                    .SetMouseButtonType(config.MouseType);
            }
            return obj.myConfig;
        }

        public static IGURect GetLocalPosition(IGUObject obj) {
            if (obj.parent != null) {
                if (obj.parent is IIGUClipping cli && cli.IsClipping) 
                    return obj.myRect.SetScaleFactor(GetLocalPosition(obj.parent).ScaleFactor);
                IGURect res = obj.myRect;
                return res.SetScaleFactor(GetLocalPosition(obj.parent).ScaleFactor)
                    .SetPosition(res.Position + GetLocalPosition(obj.parent).Position);
            }
            return obj.myRect;
        }
    }
}
