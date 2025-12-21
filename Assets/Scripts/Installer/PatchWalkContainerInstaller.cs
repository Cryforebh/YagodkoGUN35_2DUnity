using UnityEngine;
using Zenject;

public class PatchWalkContainerInstaller : MonoInstaller
{
    [SerializeField] private PatchWalkContainer patchWalkContainer;

    public override void InstallBindings()
    {
        Container.BindInstance(patchWalkContainer).AsSingle();
    }
}