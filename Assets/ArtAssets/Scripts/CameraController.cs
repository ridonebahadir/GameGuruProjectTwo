using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private float height = 2f;

    
    private CharacterController _characterController;
    private float _currentAngle;
    private bool _isWin;
    private CinemachineVirtualCamera _virtualCamera;


    [Inject]
    private void Construct(CharacterController characterController)
    {
        _characterController = characterController;
    }
    
    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Lose()
    {
        _virtualCamera.Follow = null;
        _virtualCamera.LookAt = null;
    }

    public void Win()
    {
        _virtualCamera.Follow = null;
        _isWin = true;
    }


    private void LateUpdate()
    {
        if (!_isWin) return;
        var pivot = _characterController.transform.position + Vector3.up * height;
        transform.RotateAround(pivot, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}