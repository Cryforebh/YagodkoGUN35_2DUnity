using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameFloor _gameFloor;
    [SerializeField] private ExtinctionZoneGameAndBowlingGameLogic _gameZone;
    [Header("Настройки звука:")]
    [SerializeField] private AudioClip[] audioClips;
    [Header("Условия для воспроизведения звука:")]
    [SerializeField] private float _speedThresholdForSoundDrop = 2f; // Заданное пороговое значение скорости
    [SerializeField] private float _speedThresholdForSoundMove = 5f; // Заданное пороговое значение скорости

    private BallSoundRep _ballSoundRep;
    private AudioSource _audioSource;
    private AudioSource _audioSourceDrow;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _ballSoundRep = GetComponentInChildren<BallSoundRep>();
        _audioSourceDrow = _ballSoundRep.GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _gameZone.OnCollisionEnter += _gameZone_OnCollisionEnter;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GameFloor")
        {
            // Получаем текущую скорость мяча
            float currentSpeed = _rigidbody.velocity.magnitude;

            // Проверяем, превышает ли скорость заданное пороговое значение
            if (currentSpeed > _speedThresholdForSoundDrop)
            {
                _audioSourceDrow.clip = audioClips[1];
                _audioSourceDrow.PlayOneShot(_audioSourceDrow.clip);
            }
            if (currentSpeed > _speedThresholdForSoundMove)
            {
                _audioSource.clip = audioClips[0];
                _audioSource.Play();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "GameFloor")
        {

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "GameFloor")
        {
            _audioSource.Stop();
        }
    }

    private void _gameZone_OnCollisionEnter()
    {
        _audioSource.Stop();
    }

    private void OnDisable()
    {
        _gameZone.ReloadeEarlyEvent -= _gameZone_OnCollisionEnter;
    }
}
