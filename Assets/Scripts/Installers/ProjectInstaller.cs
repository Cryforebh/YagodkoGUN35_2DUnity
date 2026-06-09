using UnityEngine;
using Zenject;

[CreateAssetMenu(
        fileName = "ProjectInstaller",
        menuName = "Installers/New ProjectInstaller"
    )]
public class ProjectInstaller : ScriptableObjectInstaller
{
    [SerializeField] public bool isTest = false;

    public override void InstallBindings()
    {
        Container.Bind<MoveInput>().AsSingle();         // Create new (no MonoBehavior)
        Debug.Log("PROJECT INSTALLER - TRUE");
    }
}
