using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button sceneRestartBtn;
    

    private GameManager _gameManager;
    
    
    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        sceneRestartBtn.onClick.AddListener(SceneRestart);
    }

    private void SceneRestart()
    {
        _gameManager.SceneRestart();
    }
}
