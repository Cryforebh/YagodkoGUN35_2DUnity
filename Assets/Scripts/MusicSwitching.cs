using UnityEngine;

public class MusicSwitching : MonoBehaviour
{
    [SerializeField] private AudioClip[] _musicClips;

    private AudioSource _source;
    private int _index = 0;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _index = Random.Range(0, _musicClips.Length);
    }

    private void Update()
    {
        if (_source.isPlaying) return;

        _index++;

        if (_index >= _musicClips.Length) _index = 0;

        _source.clip = _musicClips[_index];
        _source.Play();
    }
}
