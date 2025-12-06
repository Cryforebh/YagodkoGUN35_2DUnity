using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonTutorial : MonoBehaviour
{
    [SerializeField] private PlayerInteractObject _player;
    [SerializeField] private Canvas _canvasTutorial;
    [SerializeField] private AudioClip _audioClick;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _player.ButtonClickEvent += _interactObject_ButtonClickEvent;
    }

    private void _interactObject_ButtonClickEvent()
    {
        _canvasTutorial.enabled = !_canvasTutorial.enabled;
        _audioSource.PlayOneShot(_audioClick);
    }

    private void OnDisable()
    {
        _player.ButtonClickEvent -= _interactObject_ButtonClickEvent;
    }
}
