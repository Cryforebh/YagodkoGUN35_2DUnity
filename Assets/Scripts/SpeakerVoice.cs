using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpeakerVoice : MonoBehaviour
{
    [SerializeField] private ExtinctionZoneGameAndBowlingGameLogic _zoneGame;
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
        _zoneGame.WinEvent += _zoneGame_WinEvent;
    }

    private void _zoneGame_WinEvent()
    {
        DelaySound();
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
        _zoneGame.WinEvent -= _zoneGame_WinEvent;
    }
    
    private IEnumerator DelaySound()
    {
        yield return new WaitForSeconds(0.1f);
        _audioSource.PlayOneShot(_audioClips[2]);
    }
}
