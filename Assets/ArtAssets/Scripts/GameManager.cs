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
    private CinemachineVirtualCamera _virtualCamera;
    private UIController _uiController;
    private CharacterController _characterController;
    
    [Inject]
    private void Construct(StacksController stacksController,CinemachineVirtualCamera virtualCamera
        ,UIController uiController,CharacterController characterController)
    {
        _stacksController = stacksController;
        _virtualCamera = virtualCamera;
        _uiController = uiController;
        _characterController = characterController;
    }

    private void OnEnable()
    {
        _stacksController.OnLoseGame += LoseGame;
    }

    private void OnDisable()
    {
        _stacksController.OnLoseGame -= LoseGame;
    }

    private void WinGame()
    {
      
        _uiController.WinActivated();
    }

    private void LoseGame()
    {
        _characterController.Fall();
        _uiController.LoseActivated();
        _virtualCamera.Follow = null;
        _virtualCamera.LookAt = null;
    }
    
    
    public void SceneRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
