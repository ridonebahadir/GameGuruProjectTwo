using UnityEngine;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    private readonly float _moveDuration = 0.5f;

    public bool IsMoving { get; private set; } = false;

    public void MoveTo(Vector3 targetPosition)
    {
        if (IsMoving) return;

        IsMoving = true;

        var finalPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        transform.DOMove(finalPos, _moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                IsMoving = false;
            });
    }

    public void Fall()
    {
        var target =transform.position + new Vector3(0,-10,5);
        transform.DOJump(target, 10, 0,2).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        
    }
}
