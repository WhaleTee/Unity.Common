using System;

namespace WhaleTee.Runtime.ServiceLocator {
  public readonly struct Service : IEquatable<Service> {
    public Type Type { get; }
    public object Instance { get; }
    public string Name { get; }

    public Service(Type type, object instance = null, string name = "") {
      Type = type;
      Instance = instance;
      Name = name;
    }

    public bool Equals(Service other) => Type.IsAssignableFrom(other.Type) && Name == other.Name;
    public override bool Equals(object obj) => obj is Service other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Type.GetHashCode(), Name);
    public static bool operator ==(Service a, Service b) => a.Equals(b);
    public static bool operator !=(Service a, Service b) => !(a == b);
  }
}