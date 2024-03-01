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
        [SerializeField] protected IGUCanvas container;
#if UNITY_EDITOR
        [SerializeField] private bool foldout;
#endif

        public IGUCanvas Container => container;
        public IGURect LocalRect => GetLocalPosition(this);
        public IGUConfig LocalConfig => GetLocalConfig(this);
        public IGURect MyRect { get => myRect; set => myRect = value; }
        public IGUObject Parent { get => parent; set => parent = value; }
        public IGUColor MyColor { get => myColor; set => myColor = value; }
        public IGUConfig MyConfig { get => myConfig; set => myConfig = value; }

        public void OnIGU() {
            GUI.SetNextControlName(name);
            IGUConfig config = LocalConfig;
            if (config.IsVisible) {
                bool oldEnabled = GUI.enabled;
                GUI.enabled = config.IsEnabled;
                myRect.SetScaleFactor(IGUDrawer.ScaleFactor);

                (this as IIGUObject).InternalPreOnIGU();
                LowCallOnIGU();
                (this as IIGUObject).InternalPostOnIGU();
                GUI.enabled = oldEnabled;
            }
        }

        public IGUCanvas ApplyToContainer(IGUCanvas container) {
            if (!container.Add(this))
                Debug.Log($"The object '{name}' already exists in the container '{container.Name}'!");
            return this.container = container;
        }

        public IGUCanvas ApplyToContainer(string name, IGUCanvasContainer.CanvasType type)
            => ApplyToContainer(IGUCanvasContainer.GetOrCreateIGUCanvas(name, type));

        public IGUCanvas ApplyToContainer(string name)
            => ApplyToContainer(name, IGUCanvasContainer.CanvasType.Volatile);

        public IGUCanvas ApplyToGenericContainer()
            => ApplyToContainer(IGUCanvasContainer.GetGenericContainer());

        public IGUCanvas ApplyToPermanentGenericContainer()
            => ApplyToContainer(IGUCanvasContainer.GetPermanentGenericContainer());

        public void RemoveFromContainer() {
            container?.Remove(this);
            container = null;
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

        private void SetIGUConfig(IGUConfig config) {
            if (myConfig.Depth != config.Depth)
                container.ChangeDepth(this, myConfig.Depth, config.Depth);
            myConfig = config;
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

        void IIGUObject.InternalPreOnIGU() => PreOnIGU();

        void IIGUObject.InternalPostOnIGU() => PostOnIGU();

        public static T Create<T>() where T : IGUObject
            => (T)Create(typeof(T));

        public static T Create<T>(string name) where T : IGUObject
            => (T)Create(typeof(T), name, "none");

        public static T Create<T>(string name, string applyToContainer) where T : IGUObject
            => (T)Create(typeof(T), name, applyToContainer);

        public static IGUObject Create(Type type)
            => Create(type, nameof(IGUObject));

        public static IGUObject Create(Type type, string name)
            => Create(type, name, "none");

        public static IGUObject Create(Type type, string name, string applyToContainer) {
            if (!type.IsSubclassOf(typeof(IGUObject)))
                throw new IGUException($"Class {type.Name} does not inherit from class IGUObject.");
            else if (type.IsAbstract) 
                throw new IGUException("The target class cannot be abstract.");
            IGUObject instance = (IGUObject)CreateInstance(type.Name);
            if (!string.IsNullOrEmpty(applyToContainer) && 
                applyToContainer.ToLower() != "none") {
                    if (applyToContainer.ToLower() == "gc")
                        _ = instance.ApplyToGenericContainer();
                    else if (applyToContainer.ToLower() == "pgc")
                        _ = instance.ApplyToPermanentGenericContainer();
                    else _ = instance.ApplyToContainer(applyToContainer);
            }
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
                    return obj.myRect.SetScaleFactor(IGUDrawer.ScaleFactor);
                IGURect res = obj.myRect;
                return res.SetScaleFactor(IGUDrawer.ScaleFactor)
                    .SetPosition(res.Position + GetLocalPosition(obj.parent).Position);
            }
            return obj.myRect;
        }
    }
}
