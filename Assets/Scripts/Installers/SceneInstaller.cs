using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Character _player;
    [SerializeField] private MoveInput _moveInput;

    public override void InstallBindings()
    {
        Container.Bind<MoveInput>().FromInstance(_moveInput).AsSingle();
        Container.Bind<ICharacter>().To<Character>().FromInstance(_player).AsSingle();
    }

    public void Update()
    {
        _player.ManualUpdate();
    }
}
