using UnityEngine;

namespace WhaleTee.Runtime.Extensions {
  public static class Vector2Extensions {
    /// <summary>
    /// Returns a new Vector2 with each component being the absolute value of the corresponding component of the input vector.
    /// </summary>
    /// <param name="vector">The input Vector2 whose components' absolute values are to be computed.</param>
    /// <returns>A new Vector2 with the absolute values of the input vector's components.</returns>
    public static Vector2 Abs(this Vector2 vector) => new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    
    /// <summary>
    /// Converts a world direction vector to a screen direction vector.
    /// </summary>
    /// <param name="worldDirection">The direction vector in world space.</param>
    /// <param name="position">The position in world space from which the direction is calculated.</param>
    /// <param name="camera">The camera used to convert the world position to screen position.</param>
    /// <returns>A normalized Vector2 representing the direction in screen space.</returns>
    public static Vector2 ToScreenDirection(this Vector2 worldDirection, Vector2 position, Camera camera) {
      return camera.WorldToScreenPoint(position + worldDirection) - camera.WorldToScreenPoint(position);
    }
    
    public static bool Approximately(this Vector2 vector, Vector2 other, float tolerance) {
      return Mathf.Abs(vector.x - other.x) <= tolerance && Mathf.Abs(vector.y - other.y) <= tolerance;
    }

    public static bool Approximately(this Vector2 vector, Vector2 other) {
      return Mathf.Approximately(vector.x, other.x) && Mathf.Approximately(vector.y, other.y) ;
    }
  }
}