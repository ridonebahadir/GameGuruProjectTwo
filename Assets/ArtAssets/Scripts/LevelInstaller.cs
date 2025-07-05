using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
   

    [SerializeField] private GameManager gameManager;
    
    public override void InstallBindings()
    {
       
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
    }
}