using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StacksController : MonoBehaviour
{
    [SerializeField] private Stack stackObj;
    [SerializeField] private Stack startStack;
    private Stack _currentStack;

    private void Start()
    {
        _currentStack = startStack;
        _currentStack.IsMoving = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnStack();
        }
    }


    private void SpawnStack()
    {
        var nextPos = _currentStack.transform.position + Vector3.forward * _currentStack.GetBoundZ();
        _currentStack = Instantiate(stackObj, nextPos, Quaternion.identity, transform);
        _currentStack.Init(_currentStack);
        

    }
}
