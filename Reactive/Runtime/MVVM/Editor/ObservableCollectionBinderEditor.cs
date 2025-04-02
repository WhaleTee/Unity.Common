using System;
using ObservableCollections;
using UnityEditor;
using WhaleTee.Reactive.Runtime.MVVM.Binders;

namespace WhaleTee.Reactive.Runtime.MVVM.Editor {
  [CustomEditor(typeof(ObservableCollectionBinder), true)]
  public class ObservableCollectionBinderEditor : ObservableBinderBase {
    private ObservableCollectionBinder observableBinder;
    protected override SerializedProperty propertyName { get; set; }

    protected override void OnEnable() {
      base.OnEnable();

      observableBinder = (ObservableCollectionBinder)target;
      propertyName = serializedObject.FindProperty(nameof(propertyName));
    }

    protected override bool IsValidProperty(Type propertyType) {
      var requiredArgumentType = observableBinder.ArgumentType;
      var requiredType = typeof(IObservableCollection<>);

      return IsValidProperty(propertyType, requiredType, requiredArgumentType);
    }
  }
}