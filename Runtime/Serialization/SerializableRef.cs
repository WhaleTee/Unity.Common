using System;

namespace WhaleTee.Runtime.Serialization {
  public interface SerializableRef {
    Type RefType { get; }
    object SerializedObject { get; }
    bool HasSerializedObject { get; }

    // True if the value has changed
    bool OnSerialize(object value);

    void Clear();
  }

  internal interface SerializableRef<out T> : SerializableRef where T : class {
    T Value { get; }
  }
}