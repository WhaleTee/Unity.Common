using UnityEngine;

namespace WhaleTee.Runtime.DragSystem.Component {
  public enum Axis {
    X, Y, Z
  }

  internal static class AxisExtensions {
    public static Vector3 ToVector(this Axis axis, Transform relative) {
      return axis switch { Axis.X => relative.right, Axis.Y => relative.up, Axis.Z => relative.forward, var _ => Vector3.zero };
    }
  }
}