using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

public class StacksController : MonoBehaviour
{
    [SerializeField] private Stack stackPrefab;
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private GameObject finishPrefab;
    [SerializeField] private int totalStackCount;
    [SerializeField] private Stack firstStack;
    [SerializeField] private Material[] materials;
    
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

    private DiContainer _diContainer;
    private PoolManager _poolManager;

    [Inject]
    private void Construct(CharacterController characterController,DiContainer diContainer,PoolManager poolManager)
    {
        _characterController = characterController;
        _diContainer = diContainer;
        _poolManager = poolManager;
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

        SpawnCoin(0,finishPos.z);
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

        var startX = dir == 1 ? -3f : 3f;
        var spawnPos = new Vector3(startX, 0f, _stackZ);

        var obj = Instantiate(stackPrefab, spawnPos, Quaternion.identity, transform);
        _diContainer.Inject(obj);
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
    
    private void SpawnCoin(float startZ, float endZ)
    {
        var coinCount = 3;

        for (var i = 0; i < coinCount; i++)
        {
            var t = (float)(i + 1) / (coinCount + 1); 
            var zPos = Mathf.Lerp(startZ, endZ, t);
            var xPos = Random.Range(-2f, 2f);

            var spawnPos = new Vector3(xPos, 0.5f, zPos);
            var coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity,transform);
            _diContainer.Inject(coin);
        }
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
        SpawnCoin(_finishInstance.transform.position.z,newFinishPos.z);
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
