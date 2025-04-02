using System;
using UnityEditor;
using WhaleTee.Reactive.Runtime.MVVM.Binders;

namespace WhaleTee.Reactive.Runtime.MVVM.Editor
{
    [CustomEditor(typeof(ObservableBinder), true)]
    public class ObservableBinderEditor : ObservableBinderBase
    {
        private ObservableBinder _observableBinder;
        protected override SerializedProperty propertyName { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();

            _observableBinder = (ObservableBinder)target;
            propertyName = serializedObject.FindProperty(nameof(propertyName));
        }

        protected override bool IsValidProperty(Type propertyType)
        {
            var requiredArgumentType = _observableBinder.ArgumentType;
            var requiredType = typeof(IObservable<>);

            return IsValidProperty(propertyType, requiredType, requiredArgumentType);
        }
    }
}