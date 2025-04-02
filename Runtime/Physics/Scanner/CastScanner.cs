using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WhaleTee.Runtime.Physics.Scanner {
  /// <summary>
  /// Abstract class for scanning a specified layer for game objects using an implemented cast.
  /// </summary>
  public abstract class CastScanner : MonoBehaviour {
    /// <summary>
    /// The maximum distance to check for collisions.
    /// </summary>
    protected const float MAX_DISTANCE = .01f;

    [SerializeField] protected Vector3 center;
    [SerializeField] protected Vector3 direction;

    /// <summary>
    /// Scans the specified layer for game objects using a implemented cast and returns the result as an enumerable collection.
    /// </summary>
    /// <param name="expectedObjectsCount">The expected number of objects to be found. Use a negative value for an unlimited count.</param>
    /// <param name="layer">The layer to scan for objects.</param>
    /// <returns>An enumerable collection of game objects found in the specified layer.</returns>
    public IEnumerable<T> ScanForLayer<T>(uint expectedObjectsCount, LayerMask layer) where T : Object {
      var hits = new RaycastHit[expectedObjectsCount];
      CastNonAlloc(hits, layer);
      if (typeof(GameObject).IsAssignableFrom(typeof(T))) return SelectGameObject(hits) as IEnumerable<T>;

      return SelectComponent<T>(hits);
    }

    private IEnumerable<GameObject> SelectGameObject(RaycastHit[] hits) {
      return hits is { Length: > 0 } ? hits.Where(hit => hit.collider).Select(hit => hit.collider.gameObject).ToArray() : Array.Empty<GameObject>();
    }

    private IEnumerable<T> SelectComponent<T>(RaycastHit[] hits) {
      var component = typeof(T);

      return hits is { Length: > 0 }
             ? hits.Where(hit => hit.collider).Select(hit => hit.collider.GetComponent<T>()).ToArray()
             : Array.Empty<T>();
    }

    /// <summary>
    /// Performs the cast and stores the results in the provided array.
    /// </summary>
    /// <param name="hits">The array to store the cast results in.</param>
    /// <param name="layer">The layer to cast against.</param>
    protected abstract void CastNonAlloc(in RaycastHit[] hits, LayerMask layer);
  }
}