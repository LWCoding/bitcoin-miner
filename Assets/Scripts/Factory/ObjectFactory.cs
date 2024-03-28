using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum PoolableType
{
    CS_USER_TEXT = 0, CS_ADMIN_TEXT = 1, FILE_WINDOW = 2, SHOP_WINDOW = 3,
    MONEY_PARTICLE = 4
}

public class ObjectFactory : MonoBehaviour
{

    public static ObjectFactory Instance;

    [Header("Prefab Assignments")]
    [SerializeField] private GameObject _csUserTextPrefab;
    [SerializeField] private GameObject _csAdminTextPrefab;
    [SerializeField] private GameObject _fileWindowPrefab;
    [SerializeField] private GameObject _shopWindowPrefab;
    [SerializeField] private GameObject _moneyParticlePrefab;
    [Header("Object Assignments")]
    [SerializeField] private Transform _pooledParentTransform;

    private readonly Dictionary<PoolableType, Queue<GameObject>> _pooledObjects = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    /// <summary>
    /// Get (or create) a pooled object of type `type`. Optionally takes
    /// in a supplied `parent` to parent the object to.
    /// 
    /// If the object has a `PoolableObject` MonoBehaviour attached,
    /// calls the `OnSpawn()` method before returning.
    /// </summary>
    public GameObject GetPooledObject(PoolableType type, Transform parent = null)
    {
        // If the key doesn't exist, initialize it in the dictionary
        if (!_pooledObjects.ContainsKey(type))
        {
            _pooledObjects[type] = new();
        }
        Queue<GameObject> _qToDequeue = _pooledObjects[type];
        // If the queue is empty, create a new instance
        if (_qToDequeue.Count == 0)
        {
            switch (type)
            {
                case PoolableType.CS_USER_TEXT:
                    _qToDequeue.Enqueue(Instantiate(_csUserTextPrefab));
                    break;
                case PoolableType.CS_ADMIN_TEXT:
                    _qToDequeue.Enqueue(Instantiate(_csAdminTextPrefab));
                    break;
                case PoolableType.FILE_WINDOW:
                    _qToDequeue.Enqueue(Instantiate(_fileWindowPrefab));
                    break;
                case PoolableType.SHOP_WINDOW:
                    _qToDequeue.Enqueue(Instantiate(_shopWindowPrefab));
                    break;
                case PoolableType.MONEY_PARTICLE:
                    _qToDequeue.Enqueue(Instantiate(_moneyParticlePrefab));
                    break;
            }
        }
        GameObject obj = _qToDequeue.Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(parent, false);
        obj.transform.localScale = Vector3.one;
        if (obj.TryGetComponent(out PoolableObject poolable))
        {
            poolable.OnSpawn();
        }
        return obj;
    }

    /// <summary>
    /// Return an object to the pool. Assumes that the returned object
    /// is of type `type`. Should NOT be called first; should always
    /// returned a previously retrieved object from `GetPooledObject`.
    /// 
    /// If the object has a `PoolableObject` MonoBehaviour attached,
    /// calls the `OnDespawn()` method before exiting.
    /// </summary>
    public void ReturnObjectToPool(GameObject obj, PoolableType type)
    {
        if (obj.TryGetComponent(out PoolableObject poolable))
        {
            poolable.OnDespawn();
        }
        obj.transform.SetParent(_pooledParentTransform);
        obj.SetActive(false);
        _pooledObjects[type].Enqueue(obj);
    }

}
