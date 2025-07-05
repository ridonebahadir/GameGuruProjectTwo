using UnityEngine;

public class CharacterController: MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 0.05f;

    private Vector3 _targetPosition;
    private bool _isMoving = false;
    private bool _isFalling = false;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (_isMoving && !_isFalling)
        {
            MoveToTarget();
        }
    }

    public void MoveTo(Vector3 position)
    {
        _targetPosition = position;
        _isMoving = true;
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) <= stoppingDistance)
        {
            _isMoving = false;
        }
    }

    public void Fall()
    {
        if (_isFalling) return;

        _isFalling = true;
        _isMoving = false;

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
            _rigidbody = gameObject.AddComponent<Rigidbody>();
    }
}