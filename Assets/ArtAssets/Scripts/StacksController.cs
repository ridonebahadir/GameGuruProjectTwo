using System;
using Cinemachine;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class StacksController : MonoBehaviour
{
    [SerializeField] private Stack stack;
    [SerializeField] private Stack firstStack;
    [SerializeField] private Material[] materials;

    public Action OnLoseGame;
    private CharacterController _characterController;
    private CinemachineVirtualCamera _cineMachineVirtualCamera;
    private Stack _currentStack;
    private Stack _lastStack;
    private float _stackZ;
    private readonly float _stackLength = 2.63f;

    [Inject]
    private void Construct(CharacterController characterController)
    {
        _characterController = characterController;
      
    }
    

    private void Start()
    {
        _lastStack = firstStack;
        _lastStack.IsMoving = false;

        var startPos = new Vector3(_lastStack.transform.position.x, _characterController.transform.position.y,
            _lastStack.transform.position.z);
        _characterController.MoveTo(startPos);

        _stackZ = _lastStack.transform.position.z;
        SpawnStack();
    }

    private void Update()
    {
        if (!_characterController.IsMoving && Input.GetMouseButtonDown(0))
        {
            PlaceBlock();
        }
    }

    private void SpawnStack()
    {
        _stackZ += _stackLength;

        var dir = Random.value > 0.5f ? 1 : -1;
        var direction = dir == 1 ? Vector3.right : Vector3.left;

        var startX = dir == 1 ? -5f : 5f;
        var spawnPos = new Vector3(startX, 0f, _stackZ);

        var obj = Instantiate(stack, spawnPos, Quaternion.identity, transform);
        _currentStack = obj;
        _currentStack.Initialize(direction, materials[Random.Range(0, materials.Length)]);

        if (_lastStack != null)
        {
            var lastScale = _lastStack.transform.localScale;
            obj.transform.localScale = new Vector3(lastScale.x, 1f, lastScale.z);
        }
    }


    private void PlaceBlock()
    {
        _currentStack.IsMoving = false;

        if (_lastStack != null)
        {
            var alive = _currentStack.Cut(_lastStack);
            if (!alive)
            {
                OnLoseGame?.Invoke();
                return;
            }
        }

        _characterController.MoveTo(new Vector3(
            _currentStack.transform.position.x,
            _characterController.transform.position.y,
            _currentStack.transform.position.z
        ));

        _lastStack = _currentStack;
        SpawnStack();
    }
}