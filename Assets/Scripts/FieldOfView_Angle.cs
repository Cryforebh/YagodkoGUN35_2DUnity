using UnityEngine;

// Менее производительная реализация за счет метода Angle, но более функциональная.
// (Если логика обнаружения требует работы с углами в градусах, и реализации дополнительных механик, зависящих от угла)
// Например:
// - Изменение скорости реакции врага в зависимости от угла, под которым виден игрок;
// - Расчёт силы атаки или урона, который зависит от угла попадания.
public class FieldOfView_Angle : FieldOfView
{
    protected override void CalculationDetection()
    {
        _thisPosition = transform.position;

        Vector3 targetPosition = _targetTransform.position;

        Vector3 directionToPlayer = targetPosition - _thisPosition;
        float distanceToPlayer = directionToPlayer.magnitude;

        _viewVector = transform.forward;

        float angle = Vector3.Angle(_viewVector, directionToPlayer);

        bool isInViewAngle = angle <= _desiredAngleDegrees;
        bool isInViewDistance = distanceToPlayer < MaxViewDistance;

        if (isInViewAngle && isInViewDistance)
        {
            CalculationRaycast();
        }
        else
        {
            SetView(false);
        }

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
}
