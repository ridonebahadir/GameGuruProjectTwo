using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-10)]
public class PoolManager : MonoBehaviour
{
    [SerializeField] private PoolData perfectParticle;
    public PoolData PerfectParticle => perfectParticle;


    private void Awake()
    {
        Create(PerfectParticle, "Perfect Particle");
    }

    private void Create(PoolData data, string poolParentName)
    {
        var existingChild = transform.Find(poolParentName);
        existingChild = existingChild != null ? transform : new GameObject(poolParentName).transform;
        existingChild.SetParent(transform);
        data.CreatePool(existingChild);
    }
}