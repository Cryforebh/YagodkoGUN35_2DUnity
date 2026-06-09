using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Character _player;

    public override void InstallBindings()
    {
        Container.Bind<ICharacter>().To<Character>().FromInstance(_player).AsSingle();
    }
}
