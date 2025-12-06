using UnityEngine;

public class PointBallSpawner : MonoBehaviour
{

    [SerializeField] private ExtinctionZoneGameAndBowlingGameLogic _gameZone;
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable()
    {
        _gameZone.ReloadeEarlyEvent += _gameZone_OnCollisionExitEarly;
    }

    private void _gameZone_OnCollisionExitEarly()
    {
        _audioSource.Play();
    }

    private void OnDisable()
    {
        _gameZone.ReloadeEarlyEvent -= _gameZone_OnCollisionExitEarly;
    }
}
