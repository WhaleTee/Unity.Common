using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace WhaleTee.Runtime.ObjectPool {
  public class ObjectPoolManager : MonoBehaviour {
    [SerializeField] private bool destroyOnLoad;

    private GameObject emptyHolder;
    private static GameObject particleSystemsEmpty;
    private static GameObject gameObjectsEmpty;
    private static GameObject soundFXEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> objectPools;
    private static Dictionary<GameObject, GameObject> cloneToPrefabMap;

    public enum PoolType {
      ParticleSystems, GameObjects, SoundFX
    }

    public static PoolType poolingType;

    private void Awake() {
      objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
      cloneToPrefabMap = new Dictionary<GameObject, GameObject>();
      SetupEmpties();
    }

    private void SetupEmpties() {
      emptyHolder = new GameObject("Object Pools");

      particleSystemsEmpty = new GameObject("Particle Effects");
      particleSystemsEmpty.transform.SetParent(emptyHolder.transform);

      gameObjectsEmpty = new GameObject("GameObjects");
      gameObjectsEmpty.transform.SetParent(emptyHolder.transform);

      soundFXEmpty = new GameObject("Sound FX");
      soundFXEmpty.transform.SetParent(emptyHolder.transform);

      if (!destroyOnLoad) {
        DontDestroyOnLoad(particleSystemsEmpty.transform.root);
      }
    }

    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType) {
      var pool = new ObjectPool<GameObject>(
        createFunc: () => CreateObject(prefab, pos, rot, poolType),
        actionOnGet: OnGetObject,
        actionOnRelease: OnReleaseObject,
        actionOnDestroy: OnDestroyObject
      );

      objectPools.Add(prefab, pool);
    }

    private static void CreatePool(GameObject prefab, Transform parent, Quaternion rot, PoolType poolType) {
      var pool = new ObjectPool<GameObject>(
        createFunc: () => CreateObject(prefab, parent, rot, poolType),
        actionOnGet: OnGetObject,
        actionOnRelease: OnReleaseObject,
        actionOnDestroy: OnDestroyObject
      );

      objectPools.Add(prefab, pool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects) {
      prefab.SetActive(false);
      var obj = Instantiate(prefab, pos, rot);
      
      prefab.SetActive(true);

      var parentObject = SetParentObject(poolType);
      obj.transform.SetParent(parentObject.transform);

      return obj;
    }

    private static GameObject CreateObject(GameObject prefab, Transform parent, Quaternion rot, PoolType poolType = PoolType.GameObjects) {
      prefab.SetActive(false);
      var obj = Instantiate(prefab, parent);
      
      obj.transform.localPosition = Vector3.zero;
      obj.transform.localRotation = rot;
      obj.transform.localScale = Vector3.one;
      
      prefab.SetActive(true);

      var parentObject = SetParentObject(poolType);
      obj.transform.SetParent(parentObject.transform);

      return obj;
    }

    private static void OnGetObject(GameObject obj) {
      // optional logic
    }

    private static void OnReleaseObject(GameObject obj) {
      obj.SetActive(false);
    }

    private static void OnDestroyObject(GameObject obj) {
      cloneToPrefabMap.Remove(obj);
    }

    private static GameObject SetParentObject(PoolType poolType) {
      return poolType switch {
               PoolType.ParticleSystems => particleSystemsEmpty,
               PoolType.GameObjects => gameObjectsEmpty,
               PoolType.SoundFX => soundFXEmpty,
               var _ => null
             };
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
    where T : Object {
      if (!objectPools.ContainsKey(objectToSpawn)) CreatePool(objectToSpawn, spawnPos, spawnRotation, poolType);

      var obj = objectPools[objectToSpawn].Get();

      if (obj) {
        if (cloneToPrefabMap.ContainsKey(obj)) {
          cloneToPrefabMap.Add(obj, objectToSpawn);
        }

        obj.transform.position = spawnPos;
        obj.transform.rotation = spawnRotation;
        obj.SetActive(true);

        if (typeof(T) == typeof(GameObject)) return obj as T;

        var component = obj.GetComponent<T>();

        if (!component) {
          Debug.LogError($"Object {objectToSpawn.name} doesn't have component of type {typeof(T)}");
          return null;
        }

        return component;
      }

      return null;
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Object {
      if (!objectPools.ContainsKey(objectToSpawn)) {
        CreatePool(objectToSpawn, parent, spawnRotation, poolType);
      }

      var obj = objectPools[objectToSpawn].Get();

      if (obj) {
        if (cloneToPrefabMap.ContainsKey(obj)) {
          // Some code that handles the clone mapping (not shown in provided snippet)
        }

        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = spawnRotation;
        obj.SetActive(true);
        
        var result = obj as T;

        if (!result) {
          Debug.LogError($"Object {objectToSpawn.name} doesn't have component of type {typeof(T)}.");
          return null;
        }

        return result;
      }

      return null;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component {
      return SpawnObject<T>(typePrefab.gameObject, spawnPos, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) {
      return SpawnObject<GameObject>(objectToSpawn, spawnPos, spawnRotation, poolType);
    }

    public static T SpawnObject<T>(T typePrefab, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component {
      return SpawnObject<T>(typePrefab.gameObject, parent, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) {
      return SpawnObject<GameObject>(objectToSpawn, parent, spawnRotation, poolType);
    }

    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects) {
      if (cloneToPrefabMap.TryGetValue(obj, out var prefab)) {
        var parentObject = SetParentObject(poolType);

        if (obj.transform.parent != parentObject.transform)
          obj.transform.SetParent(parentObject.transform);

        if (objectPools.TryGetValue(prefab, out var pool))
          pool.Release(obj);
      } else {
        Debug.LogWarning("Trying to return an object that is not pooled: " + obj.name);
      }
    }
  }
}