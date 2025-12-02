using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDetectedExitFriend : MonoBehaviour
{
    [SerializeField] private FieldOfView _detectionEnemy;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _detectionEnemy.DetectedExitEvent += _detection_DetectedExitEvent;
    }

    private void OnDisable()
    {
        _detectionEnemy.DetectedExitEvent -= _detection_DetectedExitEvent;
    }

    private void _detection_DetectedExitEvent()
    {
        int i = Random.Range(0, 2);
        if (i == 1) _audioSource.Play();
    }
}
