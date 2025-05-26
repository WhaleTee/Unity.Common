using UnityEngine;

namespace WhaleTee.Runtime.Physics.Scanner {
  /// <summary>
  /// Scans for game objects using a box cast in Unity's 3D space.
  /// </summary>
  public class BoxCastScanner : CastScanner {
    [SerializeField] protected Vector3 size;

    /// <summary>
    /// The position of the box cast.
    /// </summary>
    private Vector3 Position => transform.position + center;

    protected override void CastNonAlloc(in RaycastHit[] hits, LayerMask layer) {
      UnityEngine.Physics.BoxCastNonAlloc(
        Position,
        size,
        direction,
        hits,
        transform.rotation,
        MAX_DISTANCE,
        layer
      );
    }
  }
}