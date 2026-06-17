using System.Collections.Generic;
using UnityEngine;

public class ManagerMusic : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _audios = new List<AudioClip>(10);

    private AudioSource _audioSource;
    private int _audioIndex = 0;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _audios[_audioIndex];
        _audioSource.Play();
    }

    private void Update()
    {
        if (_audioSource.isPlaying || _audios.Count == 0) return;
        _audioIndex = _audios.Count <= _audioIndex + 1 ? 0 : _audioIndex + 1;
        _audioSource.clip = _audios[_audioIndex];
        _audioSource.Play();
    }
}
