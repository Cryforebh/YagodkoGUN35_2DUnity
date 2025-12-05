using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class GatesUpDown : MonoBehaviour
{

    [SerializeField] private ExtinctionZoneGame _extinctionZoneGame;
    [SerializeField] private AnimationClip _clipUp;
    [SerializeField] private AnimationClip _clipDown;
    [SerializeField] private AudioClip[] _audioClips;

    private AudioSource _audioSource;
    private Animation _animation;
    private bool _reverb;


    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _extinctionZoneGame.OnCollisionEnter += _extinctionZoneGame_OnCollisionEnter;
        _extinctionZoneGame.OnCollisionExit += _extinctionZoneGame_OnCollisionEnter;
        UpDown();
    }

    private void _extinctionZoneGame_OnCollisionEnter()
    {
        UpDown();
    }

    private void UpDown()
    {
        _reverb = !_reverb;

        if (_reverb)
        {
            _animation.clip = _clipUp;
            _audioSource.clip = _audioClips[0];
        }
        else
        {
            _animation.clip = _clipDown;
            _audioSource.clip = _audioClips[0];
        }

        _animation.Play();
        _audioSource.Play();
    }

    private void OnDisable()
    {
        _extinctionZoneGame.OnCollisionEnter -= _extinctionZoneGame_OnCollisionEnter;
        _extinctionZoneGame.OnCollisionExit -= _extinctionZoneGame_OnCollisionEnter;
    }
}
