using System;
using UnityEngine;

public interface IPoolable<T>
{
    void Initialize(Action<T> returnAction);
    void ReturnToPool();
}

public class PoolableObj : MonoBehaviour, IPoolable<PoolableObj>
{
    private Action<PoolableObj> _returnAction;

    // When disabled, calls the action to be pushed
    public void Initialize(Action<PoolableObj> returnAction) => _returnAction = returnAction;
    public void ReturnToPool() => _returnAction?.Invoke(this);
    private void OnDisable() => ReturnToPool();
}