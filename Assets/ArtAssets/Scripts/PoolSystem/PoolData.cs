using System;
using UnityEngine;



[Serializable]
public class PoolData
{
    private Transform _parent;
    private PoolSystem<PoolableObj> _pool;
    [SerializeField] private PoolableObj poolObjPrefab;
    [SerializeField] private int initialCount = 1;
    public void CreatePool(Transform parent)
    {
        _pool = new PoolSystem<PoolableObj>(poolObjPrefab.gameObject, initialCount, parent);
        _parent = parent;
    }

    public T PullAt<T>(Vector3 pos) where T : Component
    {
        return _pool.Pull(pos).GetComponent<T>();
    }

    public T Pull<T>() where T : Component
    {
        var item = _pool.Pull().GetComponent<T>();
        item.transform.SetParent(_parent);
        return item;
    }

    public GameObject PullGameObject()
    {
        var obj = _pool.PullGameObject();
        obj.transform.SetParent(_parent);
        return obj;
    }

    public T PullAt<T>(Vector3 pos, Quaternion rot) where T : Component
    {
        return _pool.Pull(pos, rot).GetComponent<T>();
    }
}