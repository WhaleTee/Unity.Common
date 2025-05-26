using System;
using UnityEngine;
using WhaleTee.Runtime.Extensions;

namespace WhaleTee.Runtime.Serialization {
  [Serializable]
  public class InterfaceRef<T> : SerializableRef<T> where T : class {
    public object serializedObject => implementer;
    public Type refType => typeof(T);
    public bool hasSerializedObject => implementer.OrNull() != null;

    public T value {
      get {
        if (!hasCast) {
          hasCast = true;
          type = implementer as T;
        }

        return type;
      }
    }

    [SerializeField] private Component implementer;
    private bool hasCast;
    private T type;

    public bool OnSerialize(object value) {
      var c = (Component)value;

      if (c == implementer)
        return false;

      hasCast = false;
      type = null;
      implementer = c;
      return true;
    }

    public void Clear() {
      hasCast = false;
      type = null;
      implementer = null;
    }
  }
}