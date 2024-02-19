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
        [SerializeField] protected IGUConfig myConfg;
        [SerializeField] protected IGUContainer container;
#if UNITY_EDITOR
        [SerializeField] private bool foldout;
#endif

        public IGURect LocalRect => GetLocalPosition(this);
        public IGURect MyRect { get => myRect; set => myRect = value; }
        public IGUObject Parent { get => parent; set => parent = value; }
        public IGUColor MyColor { get => myColor; set => myColor = value; }
        public IGUConfig MyConfg { get => myConfg; set => myConfg = value; }
        public IGUContainer Container { get => container; set => container = value; }

        protected virtual void Ignition() {
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
        }
        protected virtual void Start() { }
        protected virtual void IgnitionEnable() { }
        protected virtual void IgnitionDisable() { }
        protected virtual void DestroyIgnition() { }

        public void OnIGU() {
            GUI.SetNextControlName(name);
            IGUConfig config = GetModIGUConfig();
            bool oldEnabled = GUI.enabled;
            GUI.enabled = config.IsEnabled;
            if (config.IsVisible) {
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

        protected IGUConfig GetModIGUConfig() {
            if (parent != null) {
                return myConfg.SetDepth(parent.GetModIGUConfig().Depth)
                    .SetEnabled(parent.GetModIGUConfig().IsEnabled && myConfg.IsEnabled)
                    .SetVisible(parent.GetModIGUConfig().IsVisible && myConfg.IsVisible);
            }
            return myConfg;
        }

        protected virtual void PreOnIGU() { }
        protected virtual void PostOnIGU() { }

        [Obsolete]
        protected Rect GetRect(bool iginoreNotMod) {
            Rect res = IGURect.rectTemp;
            IGURect rect = GetLocalPosition(this);
            res.position = rect.ModifiedPosition;
            res.size = rect.Size;
            return res;
        }

        [Obsolete]
        protected Rect GetRect() => GetRect(false);

        protected virtual void LowCallOnIGU() { }

        private void Awake() {
            isBuild = true;
        }

        private void OnDestroy() {
            if (container != null)
                if (container.Remove(this))
                    Debug.Log(string.Format("{0} removed from container", name));
            DestroyIgnition();
            IGUDrawer.RemoveReserialization(this);
        }

        private void OnEnable() {
            if (isBuild) return;
            IgnitionEnable();
        }

        private void OnDisable() => IgnitionDisable();

        void IIGUObject.InternalOnIGU() {

            myRect.SetScaleFactor(IGUDrawer.ScaleFactor);

            Vector2 pivot = myRect.Pivot;
            pivot.x = Mathf.Clamp(pivot.x, 0f, 1f);
            pivot.y = Mathf.Clamp(pivot.y, 0f, 1f);
            myRect.SetPivot(pivot);

            if (myRect.Rotation > 360f)
                myRect.SetRotation(0f);
            if (myRect.Rotation < -360f)
                myRect.SetRotation(0f);

            IGUConfig config = GetModIGUConfig();

            if (config.IsVisible) {
                Color oldColor = GUI.color;
                Color oldContentColor = GUI.contentColor;
                Color oldBackgroundColor = GUI.backgroundColor;
                GUI.color = myColor.MyColor;
                GUI.contentColor = myColor.TextColor;
                GUI.backgroundColor = myColor.BackgroundColor;

                Matrix4x4 oldMatrix = GUI.matrix;
                GUIUtility.RotateAroundPivot(myRect.Rotation, LocalRect.ModifiedPosition);
                GUIUtility.ScaleAroundPivot(myRect.ScaleFactor, LocalRect.ModifiedPosition);
                OnIGU();
                GUI.matrix = oldMatrix;

                GUI.color = oldColor;
                GUI.contentColor = oldContentColor;
                GUI.backgroundColor = oldBackgroundColor;
            }
        }

        void IIGUObject.AlteredDepth(List<IIGUObject> changed, int depth) {
            if (myConfg.Depth != depth)
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
            instance.Ignition();
            instance.Start();
            instance.IgnitionEnable();
            instance.isBuild = false;
            if (instance is IIGUSerializationCallbackReceiver)
                IGUDrawer.AddReserialization(instance);
            return instance;
        }

        public static IGURect GetLocalPosition(IGUObject obj) {
            if (obj.parent != null) {
                if (obj.parent is IIGUClipping cli && cli.IsClipping) return obj.myRect;
                IGURect res = obj.myRect;
                return res.SetScaleFactor(Vector2.one)
                    .SetPosition(res.Position + GetLocalPosition(obj.parent).ModifiedPosition);
            }
            return obj.myRect;
        }
    }
}
