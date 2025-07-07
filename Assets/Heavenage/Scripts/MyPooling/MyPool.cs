using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Heavenage.Scripts.MyPooling
{
    public class MyPool : MonoBehaviour
    {
        // Singleton для легкого доступа
        private static MyPool _instance;

        public static MyPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<MyPool>();
                    if (_instance == null)
                    {
                        var go = new GameObject("[MyPool]");
                        _instance = go.AddComponent<MyPool>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        [Serializable]
        public struct PoolConfig
        {
            public GameObject prefab;
            public int defaultSize;
            public int maxSize;
        }

        private readonly Dictionary<int, IObjectPool<GameObject>> _pools = new();
        private readonly Dictionary<int, Queue<float>> _delayedDespawnTimes = new();
        private readonly Dictionary<int, PooledObject> _activeObjects = new();
#if UNITY_EDITOR
        private readonly Dictionary<int, Transform> _poolContainers = new();
#endif

        [SerializeField] private List<PoolConfig> poolConfigs = new();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializePools();
        }

        private void Update()
        {
            ProcessDelayedDespawns();
        }

        private void InitializePools()
        {
            foreach (var config in poolConfigs)
            {
                if (config.prefab == null)
                {
                    Debug.LogWarning("PoolConfig contains null prefab, skipping.");
                    continue;
                }

                int prefabId = config.prefab.GetInstanceID();
                var pool = CreateNewPool(config.prefab, config.defaultSize, config.maxSize);
                _pools[prefabId] = pool;
                _delayedDespawnTimes[prefabId] = new Queue<float>();

#if UNITY_EDITOR
                // Создаем контейнер для неактивных объектов в редакторе
                var container = new GameObject($"[PoolContainer] {config.prefab.name}");
                container.transform.SetParent(transform, false);
                _poolContainers[prefabId] = container.transform;
#endif

                // Предварительное заполнение пула
                for (int i = 0; i < config.defaultSize; i++)
                {
                    var obj = pool.Get();
                    pool.Release(obj);
                }
            }
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : MonoBehaviour
        {
            if (prefab == null)
            {
                Debug.LogWarning("Spawn called with null prefab.");
                return null;
            }

            int prefabId = prefab.GetInstanceID();
            if (!_pools.TryGetValue(prefabId, out var pool))
            {
                pool = CreateNewPool(prefab.gameObject, 10, 100);
                _pools[prefabId] = pool;
                _delayedDespawnTimes[prefabId] = new Queue<float>();
#if UNITY_EDITOR
                // Создаем контейнер для нового пула в редакторе
                var container = new GameObject($"[PoolContainer] {prefab.name}");
                container.transform.SetParent(transform, false);
                _poolContainers[prefabId] = container.transform;
#endif
            }

            var obj = pool.Get(); // Активация объекта происходит в CreateNewPool (go.SetActive(true))
            if (obj == null)
            {
                Debug.LogError($"Failed to get object from pool for prefab {prefab.name} (prefabId: {prefabId}).");
                return null;
            }

            var pooledObject = obj.GetComponent<PooledObject>();
            if (pooledObject == null)
            {
                Debug.LogError($"Object {obj.name} (instanceId: {obj.GetInstanceID()}) missing PooledObject component after spawn.");
                return null;
            }

            if (pooledObject.prefabId != prefabId)
            {
                pooledObject.prefabId = prefabId;
            }

            obj.transform.SetPositionAndRotation(position, rotation);
#if UNITY_EDITOR
            // В редакторе спавним объекты в корне сцены
            obj.transform.SetParent(null, false);
#else
            // В билде используем указанный родитель
            if (parent != null)
                obj.transform.SetParent(parent, false);
#endif

            int instanceId = obj.GetInstanceID();
            _activeObjects[instanceId] = pooledObject;
            return obj.GetComponent<T>();
        }

        public void Despawn<T>(T obj) where T : Component
        {
            if (obj != null)
                Despawn(obj.gameObject);
        }

        public void Despawn<T>(T obj, float delay) where T : Component
        {
            if (obj != null)
                Despawn(obj.gameObject, delay);
        }

        public void Despawn(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("Despawn called with null object.");
                return;
            }

            int instanceId = obj.GetInstanceID();
            if (!_activeObjects.ContainsKey(instanceId))
            {
                Debug.LogWarning($"Object {obj.name} (instanceId: {instanceId}) not found in active objects.");
                return;
            }

            var pooledObject = obj.GetComponent<PooledObject>();
            if (pooledObject == null)
            {
                Debug.LogError($"Object {obj.name} (instanceId: {instanceId}) missing PooledObject component.");
                return;
            }

            int prefabId = pooledObject.prefabId;
            if (_pools.TryGetValue(prefabId, out var pool))
            {
                pool.Release(obj); // Отключение и помещение в контейнер происходит в CreateNewPool
                _activeObjects.Remove(instanceId);
            }
            else
            {
                Debug.LogError($"No pool found for prefabId: {prefabId} for object {obj.name}. Attempting to create new pool.");
                // Попытка создать пул для префаба, если он не найден
                var prefab = obj; // Предполагаем, что объект сам является префабом (или нужно получить префаб другим способом)
                pool = CreateNewPool(prefab, 10, 100);
                _pools[prefabId] = pool;
                _delayedDespawnTimes[prefabId] = new Queue<float>();
#if UNITY_EDITOR
                var container = new GameObject($"[PoolContainer] {prefab.name}");
                container.transform.SetParent(transform, false);
                _poolContainers[prefabId] = container.transform;
#endif
                pool.Release(obj);
                _activeObjects.Remove(instanceId);
                Debug.Log($"Despawned object {obj.name} (instanceId: {instanceId}, prefabId: {prefabId}) to newly created pool.");
            }
        }

        public void Despawn(GameObject obj, float delay)
        {
            if (obj == null)
            {
                Debug.LogWarning("DespawnDelayed called with null object.");
                return;
            }

            int instanceId = obj.GetInstanceID();
            if (!_activeObjects.ContainsKey(instanceId))
            {
                Debug.LogWarning($"Object {obj.name} (instanceId: {instanceId}) not found in active objects for delayed despawn.");
                return;
            }

            var pooledObject = obj.GetComponent<PooledObject>();
            if (pooledObject == null)
            {
                Debug.LogError($"Object {obj.name} (instanceId: {instanceId}) missing PooledObject component for delayed despawn.");
                return;
            }

            int prefabId = pooledObject.prefabId;
            if (_delayedDespawnTimes.TryGetValue(prefabId, out var queue))
            {
                queue.Enqueue(Time.time + delay);
                Debug.Log($"Scheduled delayed despawn for {obj.name} (instanceId: {instanceId}, prefabId: {prefabId}) in {delay} seconds.");
            }
            else
            {
                Debug.LogError($"No delayed despawn queue found for prefabId: {prefabId} for object {obj.name}. Creating new queue.");
                _delayedDespawnTimes[prefabId] = new Queue<float>();
                _delayedDespawnTimes[prefabId].Enqueue(Time.time + delay);
            }
        }

        private void ProcessDelayedDespawns()
        {
            foreach (var kvp in _delayedDespawnTimes)
            {
                var queue = kvp.Value;
                while (queue.Count > 0 && queue.Peek() <= Time.time)
                {
                    queue.Dequeue();
                    if (_pools.TryGetValue(kvp.Key, out var pool))
                    {
                        foreach (var pooled in _activeObjects.Values)
                        {
                            var pooledObject = pooled;
                            if (pooledObject && pooledObject.prefabId == kvp.Key)
                            {
                                pool.Release(pooled.gameObject); // Отключение и помещение в контейнер происходит в CreateNewPool
                                _activeObjects.Remove(pooled.GetInstanceID());
                                Debug.Log($"Processed delayed despawn for {pooled.name} (instanceId: {pooled.GetInstanceID()}, prefabId: {kvp.Key})");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError($"No pool found for prefabId: {kvp.Key} during delayed despawn.");
                    }
                }
            }
        }

        private IObjectPool<GameObject> CreateNewPool(GameObject prefab, int defaultSize, int maxSize)
        {
            int prefabId = prefab.GetInstanceID();
            return new ObjectPool<GameObject>(
                () =>
                {
                    var obj = Instantiate(prefab);
#if UNITY_EDITOR
                    obj.name = "Pooled_" + prefab.name;
#endif
                    var pooledObj = obj.AddComponent<PooledObject>();
                    pooledObj.prefabId = prefabId;
                    return obj;
                },
                go => go.SetActive(true), // Активация объекта при доставании из пула
                go =>
                {
                    go.SetActive(false); // Отключение объекта при возврате в пул
#if UNITY_EDITOR
                    // Помещаем неактивный объект в соответствующий контейнер
                    int goPrefabId = go.GetComponent<PooledObject>().prefabId;
                    if (_poolContainers.TryGetValue(goPrefabId, out var container))
                    {
                        go.transform.SetParent(container, false);
                    }
#endif
                },
                go =>
                {
                    if (Application.isPlaying)
                    {
                        Destroy(go); // Уничтожаем объект только в Play Mode
                    }
                },
                true,
                defaultSize,
                maxSize
            );
        }

        private class PooledObject : MonoBehaviour
        {
            public int prefabId;
        }
    }
}