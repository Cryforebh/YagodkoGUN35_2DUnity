using System.Collections;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class Skittle : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;

    private AudioSource _audioSource;
    private int _indexSound = 0;
    private bool _isIntermediatePause = false;

    private bool _skittleOut = false;

    public bool SkittleOut { get => _skittleOut; set => _skittleOut = value; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ExtinctionZone") return;
        StartCoroutine(ProcessPlaySound());
    }

    private IEnumerator ProcessPlaySound()
    {
        if (!_isIntermediatePause)
        {
            if (_audioClips.Length == 0) yield return null;

            _indexSound = Random.Range(0, _audioClips.Length - 1);

            _audioSource.PlayOneShot(_audioClips[_indexSound]);

            _isIntermediatePause = true;

            yield return new WaitForSeconds(0.5f);

            _isIntermediatePause = false;
        }
    }
}
