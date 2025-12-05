using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class Skittle : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_audioClips.Length == 0)
            return;

        int sound = Random.Range(0, _audioClips.Length - 1);

        _audioSource.PlayOneShot(_audioClips[sound]);
    }
}
