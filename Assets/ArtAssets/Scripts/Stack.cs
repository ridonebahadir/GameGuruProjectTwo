using System;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private FallPart fallPart;

    private readonly float moveSpeed = 2f;
    private Vector3 _moveDirection = Vector3.right;

    public bool IsMoving { get; set; }

    public void Initialize(Vector3 direction, Material material)
    {
        _moveDirection = direction;
        IsMoving = true;

        meshRenderer.material = material;
    }

    private void Update()
    {
        if (IsMoving) transform.position += _moveDirection * moveSpeed * Time.deltaTime;
    }


    public bool Cut(Stack previousStack)
    {
        if (!meshRenderer.bounds.Intersects(previousStack.GetMeshRenderer().bounds))
        {
            return false;
        }

        var deltaX = transform.position.x - previousStack.transform.position.x;
        var direction = deltaX > 0 ? 1f : -1f;

        var overlap = previousStack.transform.localScale.x - Mathf.Abs(deltaX);

        var tolerance = 0.1f;
        if (Mathf.Abs(deltaX) <= tolerance)
        {
            transform.position = new Vector3(previousStack.transform.position.x, transform.position.y,
                transform.position.z);
            transform.localScale = new Vector3(previousStack.transform.localScale.x, 1f,
                previousStack.transform.localScale.z);

            Debug.Log("PERFECT!");

            return true;
        }

        if (overlap <= 0f)
        {
            return false;
        }

        float centerX = (transform.position.x + previousStack.transform.position.x) / 2f;
        transform.position = new Vector3(centerX, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(overlap, 1f, previousStack.transform.localScale.z);

        var cutSize = Mathf.Abs(deltaX);
        var cutX = previousStack.transform.position.x +
                   (direction * (previousStack.transform.localScale.x / 2f + cutSize / 2f));
        var cutPos = new Vector3(cutX, transform.position.y, transform.position.z);
        var cutScale = new Vector3(cutSize, 1f, previousStack.transform.localScale.z);

        SpawnFallingPart(cutPos, cutScale);
        return true;
    }

    private void SpawnFallingPart(Vector3 pos, Vector3 scale)
    {
        var part = Instantiate(fallPart, pos, Quaternion.identity);
        part.transform.position = pos;
        part.transform.localScale = scale;
        part.Init(meshRenderer.material);
    }

    private MeshRenderer GetMeshRenderer() => meshRenderer;
}