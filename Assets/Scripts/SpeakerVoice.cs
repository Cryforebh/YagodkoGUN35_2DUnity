using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpeakerVoice : MonoBehaviour
{
    [SerializeField] private ExtinctionZoneGame _zoneGame;
    [SerializeField] private AudioClip[] _audioClips;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _zoneGame.SpareEvent += _zoneGame_SpareEvent;
        _zoneGame.StrikeEvent += _zoneGame_StrikeEvent;
    }

    private void _zoneGame_StrikeEvent()
    {
        _audioSource.PlayOneShot(_audioClips[0]);
    }

    private void _zoneGame_SpareEvent()
    {
        _audioSource.PlayOneShot(_audioClips[1]);
    }

    private void OnDisable()
    {
        _zoneGame.SpareEvent -= _zoneGame_SpareEvent;
        _zoneGame.StrikeEvent -= _zoneGame_StrikeEvent;
    }
}
