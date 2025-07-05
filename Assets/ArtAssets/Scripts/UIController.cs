using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button sceneRestartBtn;
    [SerializeField] private CanvasGroup win;
    [SerializeField] private CanvasGroup lose;

    private GameManager _gameManager;


    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        sceneRestartBtn.onClick.AddListener(SceneRestart);
        DeActiveGroup(win);
        DeActiveGroup(lose);
    }

    private void SceneRestart()
    {
        _gameManager.SceneRestart();
    }

    public void WinActivated()
    {
        ActiveGroup(win);
    }

    public void LoseActivated()
    {
        ActiveGroup(lose);
    }

    private void ActiveGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void DeActiveGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}