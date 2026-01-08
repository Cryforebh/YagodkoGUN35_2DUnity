using UnityEngine;
using Zenject;

public class SpawnerControlBotWalkingInstaller : MonoInstaller
{
    [SerializeField] private SpawnerControlBotWalking spawnerControlBotWalking;

    public override void InstallBindings()
    {
        Container.BindInstance(spawnerControlBotWalking).AsSingle().NonLazy();
    }
}