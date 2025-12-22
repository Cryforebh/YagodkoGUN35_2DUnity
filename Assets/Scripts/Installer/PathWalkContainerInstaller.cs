using UnityEngine;
using Zenject;

public class PathWalkContainerInstaller : MonoInstaller
{
    [SerializeField] private PathWalkContainer pathWalkContainer;

    public override void InstallBindings()
    {
        Container.BindInstance(pathWalkContainer).AsSingle();
    }
}