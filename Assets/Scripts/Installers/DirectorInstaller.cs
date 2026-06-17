using Netologia.Quest.Characters.Player;
using UnityEngine;
using Zenject;

public class DirectorInstaller : MonoInstaller
{
    [SerializeField] private ManagerBots _managerBots;
    [SerializeField] private ManagerObjects _managerObjects;
    [SerializeField] private PlayerController _player;

    public override void InstallBindings()
    {
        Container.BindInstance(_managerObjects).AsSingle();
        Container.BindInstance(_managerBots).AsSingle();
        Container.BindInstance(_player).AsSingle();
    }
}