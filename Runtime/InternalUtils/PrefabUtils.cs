using UnityEngine;

namespace WhaleTee.Runtime.InternalUtils {
  public class PrefabUtils {
    internal static bool IsUninstantiatedPrefab(GameObject obj) => obj.scene.rootCount == 0;
  }
}