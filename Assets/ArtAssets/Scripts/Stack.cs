using UnityEngine;

public class Stack : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Vector3 moveDirection = Vector3.right;
    public bool IsMoving { get; set; }

    public void Initialize(Vector3 direction)
    {
        moveDirection = direction;
        IsMoving = true;
    }

    private void Update()
    {
        if (IsMoving) transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }


    public bool Cut(Stack previousStack)
    {
        var deltaX = transform.position.x - previousStack.transform.position.x;
        var direction = deltaX > 0 ? 1f : -1f;

        var overlap = previousStack.transform.localScale.x - Mathf.Abs(deltaX);
        var tolerance = 0.1f; 
        if (Mathf.Abs(deltaX) <= tolerance)
        {
            transform.position = new Vector3(previousStack.transform.position.x, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(previousStack.transform.localScale.x, 1f, previousStack.transform.localScale.z);
            return true;
        }

        if (overlap <= 0f)
        {
            return false;
        }

        var newX = previousStack.transform.position.x + (direction * overlap / 2f);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(overlap, 1f, previousStack.transform.localScale.z);

        var cutSize = Mathf.Abs(deltaX);
        var cutX = previousStack.transform.position.x + (direction * (previousStack.transform.localScale.x / 2f + cutSize / 2f));
        var cutPos = new Vector3(cutX, transform.position.y, transform.position.z);
        var cutScale = new Vector3(cutSize, 1f, previousStack.transform.localScale.z);

        SpawnFallingPart(cutPos, cutScale);

        return true;
    }




    private void SpawnFallingPart(Vector3 pos, Vector3 scale)
    {
        var part = GameObject.CreatePrimitive(PrimitiveType.Cube);
        part.transform.position = pos;
        part.transform.localScale = scale;
        part.AddComponent<Rigidbody>();
        Destroy(part, 3f);
    }
}