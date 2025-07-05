using UnityEngine;

public class StacksController : MonoBehaviour
{
    public Stack stack;
    public Transform cameraTransform;
    public CharacterController character;

    private Stack _currentStack;
    private Stack _lastStack;

    private float stackZ = 0f;
    private float stackLenght = 2.63f;

    private void Start()
    {
        SpawnStack();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBlock();
        }
    }

    private void SpawnStack()
    {
        stackZ += stackLenght;
        var spawnPos = new Vector3(-2f, 0f, stackZ);
        var obj = Instantiate(stack, spawnPos, Quaternion.identity);
        _currentStack = obj;
        _currentStack.Initialize(Vector3.right);

        if (_lastStack == null) return;

        var lastScale = _lastStack.transform.localScale;
        obj.transform.localScale = new Vector3(lastScale.x, 1f, lastScale.z);
    }

    private void PlaceBlock()
    {
        _currentStack.IsMoving = false;

        if (_lastStack != null)
        {
            bool alive = _currentStack.Cut(_lastStack);
            if (!alive)
            {
                Debug.Log("GAME OVER");
                character.Fall();
                return;
            }
        }
        
        character.MoveTo(new Vector3(_currentStack.transform.position.x, character.transform.position.y, _currentStack.transform.position.z));

        _lastStack = _currentStack;
        SpawnStack();
    }
}