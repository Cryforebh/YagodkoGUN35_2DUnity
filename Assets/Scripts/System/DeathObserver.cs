using UnityEngine;
using Zenject;

public class DeathObserver : IInitializable, ILateDisposable
{
    private readonly ICharacter _character;
    private readonly GameManager _gameManager;

    public DeathObserver(ICharacter character, GameManager gameManager)
    {
        _character = character;
        _gameManager = gameManager;
    }

    void IInitializable.Initialize()
    {
        _character.OnDeath += OnDeath;
        Debug.Log("Subscrabe On Death");
    }

    void ILateDisposable.LateDispose()
    {
        _character.OnDeath -= OnDeath;
    }

    public void OnDeath()
    {
        _gameManager.FinishGame();
    }
}
