using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Более оптимизированный способ определения видимости врага (Без использования Vector3.Angle)
public class FieldOfView : MonoBehaviour
{
    [SerializeField] protected float MaxViewDistance = 9f; // Максимальная дистанция обзора
    [SerializeField, Range(0,70)] protected float _desiredAngleDegrees = 70f; // угол 45 градусов
    [SerializeField] protected Transform _targetTransform; // Ссылка на объект игрока
    [SerializeField] protected MeshRenderer _iconWarning; // Иконка для отображения состояния видимости
    [SerializeField, Range(0f, 0.5f)] protected float _updateFrequency = 0.1f;
    [SerializeField] protected LayerMask _targetMask;
    [SerializeField] protected LayerMask _obstacleMask;

    protected Vector3 _defoultScale;
    protected Vector3 _thisPosition; // Позиция текущего обьекта
    protected bool _isView = false; // Флаг видимости игрока
    protected Vector3 _viewVector; // взгляд текущего обьекта
    protected float _desiredCosAngle;

    private Coroutine _coroutine;

    public event Action DetectedEvent;
    public event Action DetectedEnterEvent;
    public event Action DetectedExitEvent;

    public void SetView(bool set)
    {
        if (_isView == set) return;
        _isView = set;
        DetectedEvent?.Invoke();

        if (_isView == true)
        {
            DetectedEnterEvent?.Invoke();
            print($"{_targetTransform.name} - Обнаружен обьектом {this.name}.");
        }
            
        if (_isView == false) 
        { 
            DetectedExitEvent?.Invoke();
            print($"{_targetTransform.name} - Потерян обьектом {this.name}.");
        }
    }

    private void Awake()
    {
        _thisPosition = transform.position;
        _defoultScale = _iconWarning.transform.localScale;
        _desiredCosAngle = Mathf.Cos(_desiredAngleDegrees * Mathf.Deg2Rad);
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(ProcessView());
    }
    private void OnDisable()
    {
        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private IEnumerator ProcessView()
    {
        while (_targetTransform != null)
        {
            CalculationDetection();

            yield return new WaitForSeconds(_updateFrequency);
            yield return null;
        }
    }

    protected void CalculationRaycast()
    {
        Vector3 targetPosition = _targetTransform.position;
        Vector3 directionToPlayer = targetPosition - _thisPosition;

        // Проверяем, есть ли прямая видимость (луч не пересекает препятствия)
        if (Physics.Raycast(_thisPosition, directionToPlayer, out RaycastHit hit, MaxViewDistance, _obstacleMask))
        {
            // Если луч попал не в игрока, значит, между врагом и игроком есть препятствие
            if (hit.collider != _targetTransform.GetComponent<Collider>())
            {
                SetView(false);
            }
            else
            {
                // Луч попал в игрока — препятствий нет
                SetView(true);
            }
        }
        else
        {
            // Луч не попал ни во что, но игрок в пределах дистанции — значит, видим
            if (directionToPlayer.magnitude < MaxViewDistance)
            {
                SetView(true);
            }
            else
            {
                SetView(false);
            }
        }
    }

    protected virtual void CalculationDetection()
    {
        // Обновляем позицию врага на каждом кадре
        _thisPosition = transform.position;

        // Получаем позицию игрока
        Vector3 targetPosition = _targetTransform.position;

        // Вычисляем вектор от врага к игроку
        Vector3 directionToPlayer = targetPosition - _thisPosition;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Определяем направление взгляда врага
        _viewVector = transform.forward;

        // Вычисляем косинус угла между направлением взгляда и направлением к игроку
        float cosAngle = Vector3.Dot(_viewVector, directionToPlayer.normalized);

        // Проверяем, находится ли игрок в конусе видимости и в пределах дистанции
        bool isInViewAngle = cosAngle > _desiredCosAngle;
        bool isInViewDistance = distanceToPlayer < MaxViewDistance;

        // Если игрок в зоне видимости по углу и расстоянию — проверяем препятствия
        if (isInViewAngle && isInViewDistance)
        {
            CalculationRaycast(); // Здесь уже вызывается SetView() на основе луча
        }
        else
        {
            SetView(false);
        }

        // Меняем размер иконки _iconWarning и направляем взгляд на игрока
        if (_isView)
        {
            _iconWarning.transform.localScale = _defoultScale;
            transform.LookAt(targetPosition);
        }
        else
        {
            _iconWarning.transform.localScale = Vector3.zero;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        _thisPosition = transform.position;
        _viewVector = transform.forward;

        if (_thisPosition == Vector3.zero)
            return;

        // Определяем направления для границ угла обзора
        Vector3 rightBoundaryDirection = CalculateBoundaryDirection(_viewVector, _desiredAngleDegrees, true);
        Vector3 leftBoundaryDirection = CalculateBoundaryDirection(_viewVector, _desiredAngleDegrees, false);

        Vector3 rightOneBoundaryDirection = CalculateBoundaryDirection(_viewVector, _desiredAngleDegrees / 1.35f, true);
        Vector3 leftOneBoundaryDirection = CalculateBoundaryDirection(_viewVector, _desiredAngleDegrees / 1.35f, false);

        Vector3 rightTwoBoundaryDirection = CalculateBoundaryDirection(_viewVector, _desiredAngleDegrees / 2, true);
        Vector3 leftTwoBoundaryDirection = CalculateBoundaryDirection(_viewVector, _desiredAngleDegrees / 2, false);

        Vector3 rightThreeBoundaryDirection = CalculateBoundaryDirection(_viewVector, _desiredAngleDegrees / 4, true);
        Vector3 leftThreeBoundaryDirection = CalculateBoundaryDirection(_viewVector, _desiredAngleDegrees / 4, false);

        // Определяем точки на границах угла обзора
        Vector3 rightBoundaryPoint = _thisPosition + rightBoundaryDirection * MaxViewDistance;
        Vector3 leftBoundaryPoint = _thisPosition + leftBoundaryDirection * MaxViewDistance;

        Vector3 rightOneBoundaryPoint = _thisPosition + rightOneBoundaryDirection * MaxViewDistance;
        Vector3 leftOneBoundaryPoint = _thisPosition + leftOneBoundaryDirection * MaxViewDistance;

        Vector3 rightTwoBoundaryPoint = _thisPosition + rightTwoBoundaryDirection * MaxViewDistance;
        Vector3 leftTwoBoundaryPoint = _thisPosition + leftTwoBoundaryDirection * MaxViewDistance;

        Vector3 rightThreeBoundaryPoint = _thisPosition + rightThreeBoundaryDirection * MaxViewDistance;
        Vector3 leftThreeBoundaryPoint = _thisPosition + leftThreeBoundaryDirection * MaxViewDistance;

        Vector3 LongBoundaryPoint = _thisPosition + _viewVector * MaxViewDistance;

        // Рисуем точки
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rightBoundaryPoint, 0.2f);
        Gizmos.DrawSphere(leftBoundaryPoint, 0.2f);

        Gizmos.DrawSphere(rightOneBoundaryPoint, 0.2f);
        Gizmos.DrawSphere(leftOneBoundaryPoint, 0.2f);

        Gizmos.DrawSphere(rightTwoBoundaryPoint, 0.2f);
        Gizmos.DrawSphere(leftTwoBoundaryPoint, 0.2f);

        Gizmos.DrawSphere(rightThreeBoundaryPoint, 0.2f);
        Gizmos.DrawSphere(leftThreeBoundaryPoint, 0.2f);

        Gizmos.DrawSphere(LongBoundaryPoint, 0.2f);

        // Рисуем линии
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_thisPosition, rightBoundaryPoint);
        Gizmos.DrawLine(_thisPosition, leftBoundaryPoint);

        if (_isView)
        {
            Gizmos.DrawLine(_thisPosition, _targetTransform.position);
        }
    }

    private Vector3 CalculateBoundaryDirection(Vector3 viewVector, float angle, bool isRight)
    {
        Quaternion rotation = Quaternion.AngleAxis(isRight ? angle : -angle, Vector3.up);
        return rotation * viewVector;
    }
#endif
}
