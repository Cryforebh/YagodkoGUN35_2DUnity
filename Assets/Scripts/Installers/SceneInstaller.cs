using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Character _player;

    public override void InstallBindings()
    {
        Container.Bind<MoveInput>().AsSingle();         // Create new (no MonoBehavior)
        Container.Bind<ICharacter>().To<Character>().FromInstance(_player).AsSingle();
        Container.BindInterfacesTo<MoveController>().AsCached();    // Create new (no MonoBehavior) with all interfaces (to Update-Tick)
    }
}
