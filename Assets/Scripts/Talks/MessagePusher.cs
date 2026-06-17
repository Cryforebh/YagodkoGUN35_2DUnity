using Netologia.Quest.Characters.Player;
using Netologia.Quest.Interfaces;
using Netologia.Quest.Objects;
using UnityEngine;
using Zenject;

namespace Netologia.Quest.Talks
{
	public class MessagePusher : MonoBehaviour
	{
        [SerializeField] private MessageElement _message;
        [SerializeField] private WorkerFeatureFlag _workerFeature;
        [SerializeField] private CharacterFeatureFlag _characterFeature;
        [SerializeField, Space] private float _talkDelay = 7f;
        [SerializeField] private Interval _silenceDelay = new(5f, 10f);
        [SerializeField] private float _talkRadius = 2f;

        private Transform _playerTransform;
        private Camera _camera;
        private bool _silence;
        private bool _lock;
        private float _delay;

        [Inject]
        private void Construct(PlayerController playerController)
        {
            _playerTransform = playerController.transform;
        }

        private void Awake()
        {
            _message = Instantiate(_message, FindObjectOfType<MessageRootTag>().transform);
            _message.Disable();

            _silence = true;
            _delay = _silenceDelay.Random * 2;
            _talkRadius *= _talkRadius;

            _camera = Camera.main;
        }

        private void Update()
        {
            if (!TimeManager.IsGame) return;
            if (_lock) return;
            var time = TimeManager.Time;

            if (_delay < time)
            {
                if(_silence)
                {
                    _silence = false;
                    _delay = time + _talkDelay;

                    if (Vector3.SqrMagnitude(transform.position - _playerTransform.position) < _talkRadius)
                    {
                        _message.Enable();
                        _message.WorkPush(Director.Work[(int)_workerFeature]);
                    }
                    return;
                }
                _silence = true;
                _delay = time + _silenceDelay.Random;
                _message.Disable();
            }

            if (!_silence)
            {
                _message.transform.position = _camera.WorldToScreenPoint(transform.position);
            }
        }
        public void LockMessage(bool value)
        {
            if (value)
            {
                _message.Disable();
                _lock = true;
            }
            else
            {
                _delay = TimeManager.Time + _silenceDelay.Random;
                _lock = false;
            }
                
            _silence = true;
        }

        public void CharacterPush()
        {
            //var message = Instantiate(_message, FindObjectOfType<MessageElement>().transform);
            //message.CharacterPush(Director.Personal[(int)_characterFeature]);
            //Destroy(message.gameObject, _talkDelay);

            _message.Enable();
            _delay = TimeManager.Time + _talkDelay;
            _silence = false;

            _message.CharacterPush(Director.Personal[(int)_characterFeature]);
        }
    }
}