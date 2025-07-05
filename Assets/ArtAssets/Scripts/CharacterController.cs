using UnityEngine;
using DG.Tweening;
using Zenject;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private static readonly int Win = Animator.StringToHash("Win");
    private readonly float _moveDuration = 0.5f;
    private CameraController _cameraController;
    public bool IsMoving { get; private set; } = false;
    

    [Inject]
    private void Construct(CameraController cameraController)
    {
        _cameraController = cameraController;
    }
    
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
    
    
    public void JumpTo(Vector3 targetPosition)
    {
     
        IsMoving = true;
        transform.DOJump(targetPosition, 2f, 1, 0.6f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _cameraController.Win();
                IsMoving = false;
            });
    }
    
    public void WinState()
    {
        anim.SetBool(Win, true);
    }

    public void NexLevelState()
    {
        anim.SetBool(Win, false);
    }
}
