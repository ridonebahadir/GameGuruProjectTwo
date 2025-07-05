using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

public class StacksController : MonoBehaviour
{
    [SerializeField] private Stack stackPrefab;
    [SerializeField] private Stack firstStack;
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject finishPrefab;
    [SerializeField] private int totalStackCount;

    private bool _isGameFinished;
    private bool _isFinishPanel;
    private int _stackCount;
    private CharacterController _characterController;
    private CinemachineVirtualCamera _cineMachineVirtualCamera;
    private Stack _currentStack;
    private Stack _lastStack;
    private float _stackZ;
    private readonly float _stackLength = 2.66f;
    private GameObject _finishInstance;

    public Action OnLoseGame;
    public Action OnWinGame;

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

        var finishZ = _stackZ + (_stackLength * totalStackCount);
        var finishPos = new Vector3(0f, 0.5f, finishZ);
        _finishInstance = Instantiate(finishPrefab, finishPos, Quaternion.identity);
    }


    private void Update()
    {
        if (_isFinishPanel) return;
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

        var obj = Instantiate(stackPrefab, spawnPos, Quaternion.identity, transform);
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
        if (_isGameFinished)
        {
            _isFinishPanel = true;
            JumpCharacter();
            return;
        }

        _currentStack.IsMoving = false;

        if (_lastStack != null)
        {
            var alive = _currentStack.Cut(_lastStack);
            if (!alive)
            {
                LoseGame();
                return;
            }
        }

        _characterController.MoveTo(new Vector3(
            _currentStack.transform.position.x,
            _characterController.transform.position.y,
            _currentStack.transform.position.z
        ));

        _lastStack = _currentStack;
        _stackCount++;

        if (_stackCount == totalStackCount)
        {
            WinGame();
            return;
        }

        SpawnStack();
    }

    private void LoseGame()
    {
        OnLoseGame?.Invoke();
    }

    private void WinGame()
    {
        _isGameFinished = true;
    }

    private void JumpCharacter()
    {
        OnWinGame?.Invoke();

        if (_finishInstance == null) return;

        var targetJump = new Vector3(
            _finishInstance.transform.position.x,
            _characterController.transform.position.y,
            _finishInstance.transform.position.z
        );

        _characterController.JumpTo(targetJump);
    }

    public void NextLevel()
    {
        _isGameFinished = false;
        _isFinishPanel = false;
        _stackCount = 0;
        
        var newStackZ = _finishInstance.transform.position.z + _stackLength;
        var newStackPos = new Vector3(0f, 0f, newStackZ);

        firstStack = Instantiate(stackPrefab, newStackPos, Quaternion.identity, transform);
        firstStack.Initialize(Vector3.zero, materials[Random.Range(0, materials.Length)]);
        firstStack.IsMoving = false;

        _lastStack = firstStack;

      
        _stackZ = newStackZ + (_stackLength * totalStackCount);
        var newFinishPos = new Vector3(0f, 0.5f, _stackZ+ _stackLength);
        _finishInstance = Instantiate(finishPrefab, newFinishPos, Quaternion.identity);

      
        var charStartPos = new Vector3(
            firstStack.transform.position.x,
            _characterController.transform.position.y,
            firstStack.transform.position.z
        );
        _characterController.MoveTo(charStartPos);

       
        _stackZ = firstStack.transform.position.z;

        SpawnStack();
    }







}
