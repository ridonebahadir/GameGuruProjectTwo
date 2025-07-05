using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
   

    [SerializeField] private GameManager gameManager;
    [SerializeField] private CharacterController characterController;
    
    
    public override void InstallBindings()
    {
       
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        Container.Bind<CharacterController>().FromInstance(characterController).AsSingle();
    }
}