using System;
using UnityEngine;

public class FallPart : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    
    public void Init(Material material)
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = material;
    }
}