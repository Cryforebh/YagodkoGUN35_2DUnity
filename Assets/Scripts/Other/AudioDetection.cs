using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfView), typeof(AudioSource))]
public class AudioDetection : MonoBehaviour
{
    private FieldOfView _detection;
    private AudioSource _audioSource;

    private void Awake()
    {
        _detection = GetComponent<FieldOfView>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _detection.DetectedEnterEvent += _detection_DetectedEnterEvent;
    }

    private void OnDisable()
    {
        _detection.DetectedEnterEvent -= _detection_DetectedEnterEvent;
    }

    private void _detection_DetectedEnterEvent()
    {
        _audioSource.Play();
    }
}
