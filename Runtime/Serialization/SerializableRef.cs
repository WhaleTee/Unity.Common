using System;

namespace WhaleTee.Runtime.Serialization {
  public interface SerializableRef {
    Type refType { get; }
    object serializedObject { get; }
    bool hasSerializedObject { get; }

    // True if the value has changed
    bool OnSerialize(object value);

    void Clear();
  }

  internal interface SerializableRef<out T> : SerializableRef where T : class {
    T value { get; }
  }
}