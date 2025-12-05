using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameFloor _gameFloor;
    [SerializeField] private ExtinctionZoneGame _gameZone;
    [SerializeField] private AudioClip[] audioClips;

    private BallSoundRep _ballSoundRep;
    private AudioSource _audioSource;
    private AudioSource _audioSourceDrow;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _ballSoundRep = GetComponentInChildren<BallSoundRep>();
        _audioSourceDrow = _ballSoundRep.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _gameFloor.CollisionEnterEvent += _gameFloor_CollisionEnterEvent;
        _gameFloor.CollisionExitEvent += _gameFloor_CollisionExitEvent;

        _gameZone.OnCollisionEnter += _gameZone_OnCollisionEnter;
    }

    private void _gameZone_OnCollisionEnter()
    {
        _audioSource.Stop();
    }

    private void _gameFloor_CollisionExitEvent()
    {
        _audioSource.Stop();
    }

    private void _gameFloor_CollisionEnterEvent()
    {
        _audioSourceDrow.clip = audioClips[1];
        _audioSourceDrow.PlayOneShot(_audioSourceDrow.clip);

        _audioSource.clip = audioClips[0];
        _audioSource.Play();
    }

    private void OnDisable()
    {
        _gameFloor.CollisionEnterEvent -= _gameFloor_CollisionEnterEvent;
        _gameFloor.CollisionExitEvent -= _gameFloor_CollisionExitEvent;
        _gameZone.OnCollisionExitEarly -= _gameZone_OnCollisionEnter;
    }
}
