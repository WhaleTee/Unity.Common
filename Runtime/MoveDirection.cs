using UnityEngine;

namespace WhaleTee.Runtime {
  public enum MoveDirection {
    NoDirection,
    N,
    Nw,
    Ne,
    S,
    SW,
    Se,
    W,
    E
  }

  public static class MoveDirectionExtensions {
    public static Vector3 ToVector3(this MoveDirection direction) {
      return direction switch {
               MoveDirection.N => new Vector3(0, 1),
               MoveDirection.Nw => new Vector3(-1, 1),
               MoveDirection.Ne => new Vector3(1, 1),
               MoveDirection.S => new Vector3(0, -1),
               MoveDirection.SW => new Vector3(-1, -1),
               MoveDirection.Se => new Vector3(1, -1),
               MoveDirection.W => new Vector3(-1, 0),
               MoveDirection.E => new Vector3(1, 0),
               MoveDirection.NoDirection => Vector3.zero,
               var _ => Vector3.zero
             };
    }

    public static Vector2 ToVector2(this MoveDirection direction) {
      return direction switch {
               MoveDirection.N => new Vector2(0, 1),
               MoveDirection.Nw => new Vector2(-1, 1),
               MoveDirection.Ne => new Vector2(1, 1),
               MoveDirection.S => new Vector2(0, -1),
               MoveDirection.SW => new Vector2(-1, -1),
               MoveDirection.Se => new Vector2(1, -1),
               MoveDirection.W => new Vector2(-1, 0),
               MoveDirection.E => new Vector2(1, 0),
               MoveDirection.NoDirection => Vector2.zero,
               var _ => Vector2.zero
             };
    }

    public static MoveDirection ToMoveDirection(this Vector2Int direction) {
      return direction.x switch {
               0 when direction.y == 1 => MoveDirection.N,
               0 when direction.y == -1 => MoveDirection.S,
               -1 when direction.y == 1 => MoveDirection.Nw,
               -1 when direction.y == -1 => MoveDirection.SW,
               -1 when direction.y == 0 => MoveDirection.W,
               1 when direction.y == 0 => MoveDirection.E,
               1 when direction.y == 1 => MoveDirection.Ne,
               1 when direction.y == -1 => MoveDirection.Se,
               var _ => MoveDirection.NoDirection
             };
    }

    public static MoveDirection ToMoveDirection(this Vector2 direction) {
      var x = Mathf.RoundToInt(direction.normalized.x);
      var y = Mathf.RoundToInt(direction.normalized.y);

      return x switch {
               0 when y == 1 => MoveDirection.N,
               0 when y == -1 => MoveDirection.S,
               -1 when y == 1 => MoveDirection.Nw,
               -1 when y == -1 => MoveDirection.SW,
               -1 when y == 0 => MoveDirection.W,
               1 when y == 0 => MoveDirection.E,
               1 when y == 1 => MoveDirection.Ne,
               1 when y == -1 => MoveDirection.Se,
               var _ => MoveDirection.NoDirection
             };
    }
  }
}