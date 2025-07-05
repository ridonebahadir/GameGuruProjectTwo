using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCamera;
    [SerializeField] private StacksController stacksController;
    [SerializeField] private UIController uiController;
    


    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        Container.Bind<CharacterController>().FromInstance(characterController).AsSingle();
        Container.Bind<CinemachineVirtualCamera>().FromInstance(cineMachineVirtualCamera).AsSingle();
        Container.Bind<StacksController>().FromInstance(stacksController).AsSingle();
        Container.Bind<UIController>().FromInstance(uiController).AsSingle();
    }
}