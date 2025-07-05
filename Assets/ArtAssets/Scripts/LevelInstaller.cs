using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private StacksController stacksController;
    [SerializeField] private UIController uiController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PoolManager poolManager;
    


    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        Container.Bind<CharacterController>().FromInstance(characterController).AsSingle();
        Container.Bind<CameraController>().FromInstance(cameraController).AsSingle();
        Container.Bind<StacksController>().FromInstance(stacksController).AsSingle();
        Container.Bind<UIController>().FromInstance(uiController).AsSingle();
        Container.Bind<PoolManager>().FromInstance(poolManager).AsSingle();
    }
}