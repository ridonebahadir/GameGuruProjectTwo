using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-10)]
public class PoolManager : MonoBehaviour
{
    [SerializeField] private PoolData perfectParticle;
    [SerializeField] private PoolData coin;
    [SerializeField] private PoolData coinParticle;
    
    
    public PoolData PerfectParticle => perfectParticle;
    public PoolData CoinParticle=> coinParticle;


    private void Awake()
    {
        Create(PerfectParticle, "Perfect Particle");
        Create(CoinParticle, "Coin");
    }

    private void Create(PoolData data, string poolParentName)
    {
        var existingChild = transform.Find(poolParentName);
        existingChild = existingChild != null ? transform : new GameObject(poolParentName).transform;
        existingChild.SetParent(transform);
        data.CreatePool(existingChild);
    }
}