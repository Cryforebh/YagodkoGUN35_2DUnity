using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Netologia.Quest.Characters.Player
{
    public class PlayerController : BaseMovement
    {
        private LineRenderer _renderer;
        private Controls _controls; //Inject
        private Camera _camera;
        private Transform _mouseTrans;      //курсор мышки
        private Transform _movePointTrans;  //точка текущего следования 

        private readonly Vector3[] _path = new Vector3[8];
        private float _maxCameraDistance;
        private Transform _currentTarget;
        private Character _currentCharacter;
        private bool _tryInteract;
        private bool _moveLock;

        [SerializeField]
        private LayerMask _hitcastMask;//"Floor" | "Interactables"

        [SerializeField]
        private float _offsetMarkY = .15f;
        [SerializeField, Range(0f, 10f)]
        private float _stoppingDistanceForCharacter = 4f;
        [SerializeField, Range(1f, 5f)]
        private float _stoppingDistanceForInteract = 2.5f;

        [SerializeField]
        private MeshRenderer _mouseFocus;
        [SerializeField]
        private MeshRenderer _movePoint;

        public List<QuestInfo> ActiveQuests { get; } = new(8);
        public float StoppingDistanceForInteract => _stoppingDistanceForInteract;

        private bool _isNotFall = true;

        [Inject]
        private void Construct(Controls controls)
        {
            _controls = controls;
        }

        protected override void Awake()
        {
            base.Awake();
            _renderer = GetComponent<LineRenderer>();
            _renderer.positionCount = _path.Length;
            //todo disable if pause
            //_controls = new Controls();
            _controls.Enable();
            _controls.Mouse.Click.performed += OnMoveClick;

            _camera = Camera.main;
            _maxCameraDistance = Vector3.SqrMagnitude(_camera.transform.position - transform.position);

            _mouseTrans = _mouseFocus.transform;
            _movePointTrans = _movePoint.transform;

            _mouseTrans.parent = null;
            _movePointTrans.parent = null;

            InformationBureau.OnDialogClose += OnCancelDialog;
        }

        private void Update()
        {
            var position = (Vector3)_controls.Mouse.Position.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out var hit, _maxCameraDistance, _hitcastMask, QueryTriggerInteraction.Ignore))
            {
                _currentTarget = hit.transform;

                // Все это дорого, но для иного решения нужно менять подход и логику.
                bool isCharacter = _currentTarget.TryGetComponent<Character>(out Character character);
                bool isEmitter = _currentTarget.TryGetComponent<Emitter>(out Emitter emitter);
                bool isReflector = _currentTarget.TryGetComponent<Reflector>(out Reflector reflector);

                Vector3 targetPos = hit.point;
                if (isCharacter || isEmitter || isReflector)
                {
                    if (isCharacter)
                    {
                        targetPos = character.CirclePosition; // Специфичная позиция для Character
                    }
                    else if (isEmitter)
                    {
                        // Для Emitter и Reflector используем точку попадания (можно изменить на _currentTarget.position)
                        targetPos = emitter.CirclePosition;
                    }
                    else if (isReflector)
                    {
                        targetPos = reflector.CirclePosition;
                    }
                    _isNotFall = false;
                }
                else
                {
                    _isNotFall = true;
                }

                targetPos.y += _offsetMarkY;
                _mouseTrans.position = targetPos;
                _mouseFocus.enabled = true;
            }
            else
                _mouseFocus.enabled = false;

            if (IsMove)
            {
                var count = GetPath(_path);
                Array.Fill(_path, _path[count - 1], count, _path.Length - count);
            }
            else
            {
                Array.Fill(_path, default);
                //Disable move mark and talk
                if (_tryInteract)
                {
                    if (_currentCharacter != null && _currentCharacter.OnInteract(this))
                        (_movePoint.enabled, _moveLock) = (false, true);
                    _tryInteract = false;
                }
            }

            _renderer.SetPositions(_path);
        }

        private void OnMoveClick(InputAction.CallbackContext obj)
        {
            if (_moveLock) return;

            var point = default(Vector3);
            _movePoint.enabled = true;

            // Дорого, но для клика сойдет.
            bool isEmitter = _currentTarget.TryGetComponent<Emitter>(out Emitter emitter);
            bool isReflector = _currentTarget.TryGetComponent<Reflector>(out Reflector reflector);

            //Floor click
            if (!_currentTarget.TryGetComponent(out _currentCharacter) && !isEmitter && !isReflector)
            {
                _tryInteract = false;
                point = _mouseTrans.position;
                _movePointTrans.position = point;
                SetPosition(point);
                _agent.stoppingDistance = 0f;
                return;
            }

            //Character/emitter/reflector click
            _tryInteract = true;
            point = _currentTarget.position;

            if (emitter != null)
            {
                _movePointTrans.position = emitter.CirclePosition;
                _agent.stoppingDistance = _stoppingDistanceForInteract;
            }
            else if (reflector != null)
            {
                _movePointTrans.position = reflector.CirclePosition;
                _agent.stoppingDistance = _stoppingDistanceForInteract;
            }
            else if (_currentCharacter != null)
            {
                _movePointTrans.position = _currentCharacter.CirclePosition;
                _agent.stoppingDistance = _stoppingDistanceForCharacter;
            }
            SetPosition(point);
        }

        private void OnCancelDialog() => _moveLock = false;

        private void OnDestroy()
        {
            _controls.Mouse.Click.performed -= OnMoveClick;
            InformationBureau.OnDialogClose += OnCancelDialog;
        }
    }
}
