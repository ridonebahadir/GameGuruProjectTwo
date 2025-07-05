
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    
    private GameManager _gameManager;
    private PoolManager _poolManager;

    [Inject]
    private void Construct(GameManager gameManager,PoolManager poolManager)
    {
        _gameManager = gameManager;
        _poolManager = poolManager;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(0, 2));
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterController controller))
        {
            Collect();
        }
    }

    private void Collect()
    {
        _gameManager.PlaySound(clip);
        _gameManager.ScoreIncrease(10);
        var particle = _poolManager.CoinParticle.PullGameObject();
        particle.transform.position = transform.position;
        gameObject.SetActive(false);
    }
}
