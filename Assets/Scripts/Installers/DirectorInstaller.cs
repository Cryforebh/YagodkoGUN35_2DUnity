using Netologia.Quest.Characters.Player;
using UnityEngine;
using Zenject;

public class DirectorInstaller : MonoInstaller
{
    [SerializeField] private InterfaceManager _managerInterface;
    [SerializeField] private ManagerBots _managerBots;
    [SerializeField] private ManagerObjects _managerObjects;
    [SerializeField] private PlayerController _player;

    private Controls _controls;

    public override void InstallBindings()
    {
        _controls = new Controls();
        Container.BindInstance(_controls).AsSingle();
        Container.BindInstance(_managerObjects).AsSingle();
        Container.BindInstance(_managerBots).AsSingle();
        Container.BindInstance(_player).AsSingle();
        Container.BindInstance(_managerInterface).AsSingle();
    }
}