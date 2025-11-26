using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class SoundRun : MonoBehaviour
{
    private AudioSource _sound;
    private PlayerMove _player;

    public AudioClip MoveSound;
    public AudioClip StandSound;

    private void Awake()
    {
        _player = GetComponent<PlayerMove>();
        _sound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _player.MoveEvent += _player_MoveEvent;
        _player.DontMoveEvent += _player_DontMoveEvent;
    }

    private void OnDisable()
    {
        _player.MoveEvent -= _player_MoveEvent;
        _player.DontMoveEvent -= _player_DontMoveEvent;
    }

    private void _player_DontMoveEvent()
    {
        _sound.clip = StandSound;
        _sound.Play();
    }

    private void _player_MoveEvent()
    {
        _sound.clip = MoveSound;
        _sound.Play();
    }
}
