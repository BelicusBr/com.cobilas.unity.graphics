using System;
using UnityEngine;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUObject : ScriptableObject, IIGUObject {
        [SerializeField] protected IGURect myRect;
        [SerializeField] protected IGUColor myColor;
        [SerializeField] protected IGUObject parent;
        [SerializeField] protected IGUConfig myConfg;
        [SerializeField] protected IGUContainer container;
        protected DoNotModifyRect doNots;
        protected int modifiedRect;
#if UNITY_EDITOR
        [SerializeField] private string subname;
        public bool foldout;

        public new string name { get => base.name = subname; set => base.name = subname = value; }
#endif
        public IGURect MyRect { get => myRect; set => myRect = value; }
        public IGUObject Parent { get => parent; set => parent = value; }
        public IGUColor MyColor { get => myColor; set => myColor = value; }
        public IGUConfig MyConfg { get => myConfg; set => myConfg = value; }
        public IGUContainer Container { get => container; set => container = value; }

        protected virtual void Awake() {
            doNots = DoNotModifyRect.False;
        }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void OnIGUDestroy() { }

        public virtual void OnIGU() { }

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

        protected Vector2 GetPosition()
            => parent == null || NotMod() ? myRect.ModifiedPosition : myRect.ModifiedPosition + parent.GetPosition();

        protected GUIStyle GetDefaultValue(GUIStyle style, GUIStyle _default) {
            if (style != null) return style;
            return _default;
        }

        private void OnDestroy() {
            if (container != null)
                if (container.Remove(this))
                    Debug.Log(string.Format("{0} removed from container", name));
            OnIGUDestroy();
            IGUDrawer.RemoveReserialization(this);
        }

        void IIGUObject.InternalOnIGU() {
            GUI.SetNextControlName(name);

            myRect.SetScaleFactor(IGUDrawer.ScaleFactor);

            if (myRect.ScaleFactorWidth <= 0f)
                myRect.SetScaleFactor(new Vector2(.1f, myRect.ScaleFactorHeight));
            if (myRect.ScaleFactorHeight <= 0f)
                myRect.SetScaleFactor(new Vector2(myRect.ScaleFactorWidth, .1f));
            
            if (myRect.PivotX < 0f)
                myRect.SetPivot(new Vector2(0f, myRect.PivotY));
            if (myRect.PivotX > 1f)
                myRect.SetPivot(new Vector2(1f, myRect.PivotY));
            if (myRect.PivotY < 0f)
                myRect.SetScaleFactor(new Vector2(myRect.PivotX, 0f));
            if (myRect.PivotY > 1f)
                myRect.SetScaleFactor(new Vector2(myRect.PivotX, 1f));

            if (myRect.Rotation > 360f)
                myRect.SetRotation(0f);
            if (myRect.Rotation < -360f)
                myRect.SetRotation(0f);

            (this as IIGUObject).InternalPreOnIGU();
            Matrix4x4 oldMatrix = GUI.matrix;

            GUIUtility.RotateAroundPivot(myRect.Rotation, GetPosition());
            GUIUtility.ScaleAroundPivot(myRect.ScaleFactor, GetPosition());
            OnIGU();
            GUI.matrix = oldMatrix;
            (this as IIGUObject).InternalPostOnIGU();
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
            if (instance is IIGUSerializationCallbackReceiver)
                IGUDrawer.AddReserialization(instance);
            return instance;
        }

        private bool NotMod() => parent != null && parent.doNots;
    }
}
