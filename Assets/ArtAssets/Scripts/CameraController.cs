using System;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private float height = 2f;

    private CharacterController _characterController;
    private bool _isWin;
    private CinemachineVirtualCamera _virtualCamera;

    private Transform _defaultFollowTarget;
    private Transform _defaultLookAtTarget;
    private Vector3 _defaultCameraPosition;
    private Quaternion _defaultCameraRotation;

    [Inject]
    private void Construct(CharacterController characterController)
    {
        _characterController = characterController;
    }

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        _defaultFollowTarget = _virtualCamera.Follow;
        _defaultLookAtTarget = _virtualCamera.LookAt;
        
        _defaultCameraPosition = transform.position;
        _defaultCameraRotation = transform.rotation;
    }

    public void Lose()
    {
        _virtualCamera.Follow = null;
        _virtualCamera.LookAt = null;
    }

    public void Win()
    {
        _isWin = true;
        
        _virtualCamera.Follow = null;
        _virtualCamera.LookAt = null;

       
    }

    public void NextLevel()
    {
        _isWin = false;
        
        _virtualCamera.Follow = _defaultFollowTarget;
        _virtualCamera.LookAt = _defaultLookAtTarget;
        
        transform.position = _defaultCameraPosition;
        transform.rotation = _defaultCameraRotation;
    }

    private void LateUpdate()
    {
        if (!_isWin) return;

        var pivot = _characterController.transform.position + Vector3.up * height;
        transform.RotateAround(pivot, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
