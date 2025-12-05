using UnityEngine;

public class PointBallSpawner : MonoBehaviour
{

    [SerializeField] private ExtinctionZoneGame _gameZone;
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable()
    {
        _gameZone.OnCollisionExitEarly += _gameZone_OnCollisionExitEarly;
    }

    private void _gameZone_OnCollisionExitEarly()
    {
        _audioSource.Play();
    }

    private void OnDisable()
    {
        _gameZone.OnCollisionExitEarly -= _gameZone_OnCollisionExitEarly;
    }
}
