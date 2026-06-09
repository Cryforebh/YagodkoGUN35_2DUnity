using UnityEngine;
using Zenject;

[CreateAssetMenu(
        fileName = "GameSystemInstaller",
        menuName = "Installers/New GameSystemInstaller"
    )]
public class GameSystemInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MoveInput>().AsSingle();         // Create new (no MonoBehavior)
        Container.Bind<GameManager>().AsSingle();

        Container.BindInterfacesTo<MoveController>().AsCached();    // Create new (no MonoBehavior) with all interfaces (to Update-Tick)
        Container.BindInterfacesTo<DeathObserver>().AsCached();
    }
}
