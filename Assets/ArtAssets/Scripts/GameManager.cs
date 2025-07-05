using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{

    private StacksController _stacksController;
    private CameraController _cameraController;
    private UIController _uiController;
    private CharacterController _characterController;
    
    [Inject]
    private void Construct(StacksController stacksController,CameraController cameraController
        ,UIController uiController,CharacterController characterController)
    {
        _stacksController = stacksController;
        _cameraController = cameraController;
        _uiController = uiController;
        _characterController = characterController;
    }

    private void OnEnable()
    {
        _stacksController.OnLoseGame += LoseGame;
        _stacksController.OnWinGame += WinGame;
    }

    private void OnDisable()
    {
        _stacksController.OnLoseGame -= LoseGame;
        _stacksController.OnWinGame -= WinGame;
    }

    private void WinGame()
    {
        _cameraController.Win();
        _uiController.WinActivated();
    }

    private void LoseGame()
    {
        _characterController.Fall();
        _uiController.LoseActivated();
        _cameraController.Lose();
       
    }
    
    
    public void SceneRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
