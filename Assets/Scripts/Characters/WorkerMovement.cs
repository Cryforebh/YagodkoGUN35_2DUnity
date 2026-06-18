
using Netologia.Quest.Characters.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Netologia.Quest.Characters
{
    // Поведение рабочего в фоновом режиме
    public class WorkerMovement : BaseMovement
    {
        [SerializeField, Header("Work Idle Action (Optional):")] private List<MovementPointData> _movementPoints = new List<MovementPointData>(5);
        [SerializeField] private List<MovementPointData> _movementPointsStateProgress = new List<MovementPointData>(5);
        [SerializeField] private List<MovementPointData> _movementPointsStateComplite = new List<MovementPointData>(5);
        [SerializeField] private float _radiusSerchIdle = 15f;
        [SerializeField] private float _maxStuckTimeThreshold = 4f;
        [SerializeField, Header("Debag:")] private bool _isDebagInfo = false;

        private List<MovementPointData> _currentUseIdlePoints;
        private List<MovementPointData> _newUseIdlePoints;
        private ManagerObjects _managerObjects;
        private Character _thisCharacter;
        private float _currentTime = 0f;
        private float _accumulatedStuckTime = 0f;
        private float _previousRoundedDistance;
        private MovementPointData _currentPoint;
        private PlayerController _playerController;
        private float _timeDelayIdlePoint;
        private bool _isOnPoint = false;
        private bool _isDialogue = false;
        private int _indexPoint = -1;
        private Transform _currentTargetPointPosition;

        public event Action OnEnterPoint;
        public event Action OnExitPoint;

        public float RadiusSerchIdle => _radiusSerchIdle;

        [Inject]
        private void Construct(ManagerObjects managerObjects, PlayerController playerController)
        {
            _managerObjects = managerObjects;
            _playerController = playerController;
        }

        protected override void Awake()
        {
            base.Awake();
            _thisCharacter = GetComponent<Character>();
        }

        private void Start()
        {
            //GenerateIdlePoint();

            _currentUseIdlePoints = _movementPoints;
            if (_currentUseIdlePoints.Count == 0) return;
            ProcessSetupNextPoint();
        }

        public override void ManualUpdate()
        {
            if (_currentUseIdlePoints.Count == 0) return;

            CheckArrivalAtCurrentPoint();
            IdleUpdate();
            PauseWorkOnDialog();
        }

        public void SetIdlePointsForTargetOnCompleteStatus(List<MovementPointData> newIdlePoints) => _newUseIdlePoints = newIdlePoints;

        private void PauseWorkOnDialog()
        {
            if (_thisCharacter.MoveAccess && _isDialogue)
            {
                _isDialogue = false;
                //_isOnPoint = true;
                ResetIdlePoint();
            }

            if (!_thisCharacter.MoveAccess)
            {
                _isDialogue = true;
                _agent.destination = transform.position;
                RotateTowardsDirection((_playerController.transform.position - transform.position).normalized);
            }
        }

        private void CheckArrivalAtCurrentPoint()
        {
            if (_isOnPoint || _isDialogue) 
                return; 

            float distanceToDestination = Vector3.Distance(transform.position, _currentPoint.ActionPointPosition);
            HandleStuckAgentWithTimeout(distanceToDestination);

            if (_isDebagInfo)
            {
                Debug.Log(name + "- Текущая точка: " + _currentPoint.name);
                Debug.Log(name + "- Расстояние до точки: " + distanceToDestination);
            }

            if (distanceToDestination <= _currentPoint.Radius)
            {
                _agent.destination = transform.position;
                _isOnPoint = true;
                OnEnterPoint?.Invoke();
            }
        }

        private void ResetIdlePoint()
        {
            CheckCurrentStateQuest();
            _isOnPoint = false;
            _currentTime = 0;
            ProcessSetupNextPoint();
            OnExitPoint?.Invoke();
        }

        private void IdleUpdate()
        {
            if (!_isOnPoint || _isDialogue) return;

            RotateTowardsDirection(_currentPoint.GetDirection(this.transform.position));

            _currentTime += TimeManager.DeltaTime;
            if (_currentTime >= _timeDelayIdlePoint)
            {
                ResetIdlePoint();
            }
        }

        private void CheckCurrentStateQuest()
        {
            if (_thisCharacter.CurrentActiveQuest != null)
                switch (_thisCharacter.CurrentActiveQuest.State)
                {
                    case QuestInfo.Status.New:
                        _currentUseIdlePoints = _movementPoints;
                        break;
                    case QuestInfo.Status.Progress:
                        _currentUseIdlePoints = _movementPointsStateProgress.Count > 0 ? _movementPointsStateProgress : _movementPoints;
                        break;
                    case QuestInfo.Status.Complete:
                        _currentUseIdlePoints = _movementPointsStateComplite.Count > 0 ? _movementPointsStateComplite : _movementPoints;
                        break;
                    default:
                        break;
                }

            if (_newUseIdlePoints == null || _newUseIdlePoints.Count == 0) return;
            _currentUseIdlePoints = _newUseIdlePoints;
            _newUseIdlePoints = null;

        }

        private void ProcessSetupNextPoint()
        {
            _indexPoint = (_indexPoint + 1) % _currentUseIdlePoints.Count;
            _currentPoint = _currentUseIdlePoints[_indexPoint];
            _timeDelayIdlePoint = _currentPoint.TimeDelay;
            _agent.destination = _currentPoint.ActionPointPosition;
        }

        private void RotateTowardsDirection(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    TimeManager.DeltaTime * 5f
                );
            }
        }

        private void HandleStuckAgentWithTimeout(float currentDistance)
        {
            float currentRoundedDistance = Mathf.Round(currentDistance * 10) / 10;

            if (currentRoundedDistance == _previousRoundedDistance && _agent.hasPath)
            {
                _previousRoundedDistance = currentRoundedDistance;
                _accumulatedStuckTime += TimeManager.DeltaTime;

                if (_accumulatedStuckTime >= _maxStuckTimeThreshold)
                {
                    _accumulatedStuckTime = 0;

                    Vector3 forwardTargetPosition = transform.forward + transform.position;
                    Vector3 currentPosition = transform.position;
                    Vector3 movementDirection = forwardTargetPosition - currentPosition;


                    // Нормализация нулевого вектора
                    // Метод Normalize() вычисляет единичный вектор, деля координаты на длину вектора.Если вектор нулевой((0, 0, 0)),
                    // длина равна нулю -> деление на ноль -> ошибка выполнения.
                    if (movementDirection.sqrMagnitude > 0.0001f) // Проверяем ненулевое направление
                    {
                        movementDirection.Normalize();
                        Vector3 offsetTargetPosition = currentPosition + movementDirection * 2.0f;

                        NavMeshHit navMeshHit;
                        if (NavMesh.SamplePosition(offsetTargetPosition, out navMeshHit, 1.0f, NavMesh.AllAreas))
                        {
                            // Устанавливаем позицию на ближайшую точку NavMesh
                            transform.position = navMeshHit.position;
                            Debug.LogWarning(this.name + " - Новая позиция: " + navMeshHit.position);
                        }
                        else
                        {
                            transform.position = _agent.pathEndPosition;
                            Debug.LogWarning(this.name + " - Не удалось найти NavMesh; перемещение в pathEndPosition: " + _agent.pathEndPosition);
                        }
                    }
                }
            }
            else
            {
                _accumulatedStuckTime = 0;
                _previousRoundedDistance = currentRoundedDistance;
            }
        }

        private void GenerateIdlePoint()
        {
            if (_movementPoints.Count == 0)
            {
                var idlePointOne = ManagerBots.RandomIdlePointGenerate(this, _managerObjects);
                var idlePointTwo = ManagerBots.RandomIdlePointGenerate(this, _managerObjects);
                if (idlePointOne != null) _movementPoints.Add(idlePointOne);
                if (idlePointTwo != null) _movementPoints.Add(idlePointTwo);
            }
        }
    }
}