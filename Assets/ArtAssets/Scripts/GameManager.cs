using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;


    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;
    
    private StacksController _stacksController;
    private CameraController _cameraController;
    private UIController _uiController;
    private CharacterController _characterController;
    private int _score;
    
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

    private void Start()
    {
        _uiController.UpdateScore(_score);
    }

    private void WinGame()
    {
        PlaySound(win);
        _characterController.WinState();
        _uiController.WinActivated();
    }

    private void LoseGame()
    {
        _characterController.Fall();
        _uiController.LoseActivated();
        _cameraController.Lose();
    }
    
    public void NextLevel()
    {
        _characterController.NexLevelState();
        _cameraController.NextLevel();
        _stacksController.NextLevel();
    }

    private int _perfectComboCount = 0;

    public void PlaySound(AudioClip clip, bool isPerfect=false)
    {
        if (isPerfect)
        {
            _perfectComboCount++;
            float pitch = 1.0f + (_perfectComboCount * 0.2f);
            pitch = Mathf.Clamp(pitch, 1f, 2f); 
            audioSource.pitch = pitch;
        }
        else
        {
            _perfectComboCount = 0; 
            audioSource.pitch = 1f; 
        }

        audioSource.PlayOneShot(clip);
    }

    public void ScoreIncrease(int amount)
    {
        _score += amount;
        _uiController.UpdateScore(_score);
    }
    
    public void SceneRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  
}
