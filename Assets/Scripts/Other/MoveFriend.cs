using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoveFriend : MonoBehaviour
{

    private Transform _friendTransform;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _endTwoPosition;

    [SerializeField] private float _speed = 4f;
    [SerializeField] private Vector3 _targetPosition = new Vector3(5, 0, 0);
    [SerializeField] private Vector3 _targetTwoPosition = new Vector3(5, 0, 0);

    private Coroutine _coroutine;
    private bool _movingOneTarget = true;
    private bool _movingTwoTarget = true;

    private void Awake()
    {
        _friendTransform = GetComponent<Transform>();
        _startPosition = _friendTransform.position;  // Запоминаем начальную позицию
        _endPosition = _friendTransform.position + _targetPosition;
        _endTwoPosition = _friendTransform.position + _targetTwoPosition;
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(Process());
    }
    private void OnDisable()
    {
        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private IEnumerator Process()
    {
        while (true)
        {
            if (_movingOneTarget)
            {
                // Двигаемся к целевой точке
                _friendTransform.position = Vector3.MoveTowards(
                    _friendTransform.position,
                    _endPosition,
                    _speed * Time.deltaTime
                );

                _friendTransform.LookAt(_endPosition);
            }
            else if (_movingTwoTarget)
            {
                // Двигаемся к целевой точке
                _friendTransform.position = Vector3.MoveTowards(
                    _friendTransform.position,
                    _endTwoPosition,
                    _speed * Time.deltaTime
                );

                _friendTransform.LookAt(_endTwoPosition);
            }
            else if (!_movingOneTarget && !_movingTwoTarget)
            {
                // Двигаемся к целевой точке
                _friendTransform.position = Vector3.MoveTowards(
                    _friendTransform.position,
                    _startPosition,
                    _speed * Time.deltaTime
                );

                _friendTransform.LookAt(_startPosition);
            }

            // Меняем направление, если достигли цели
            if (Vector3.Distance(_friendTransform.position, _endPosition) < 0.01f)
            {
                _movingTwoTarget = true;
                _movingOneTarget = false;
            }
            else if (Vector3.Distance(_friendTransform.position, _endTwoPosition) < 0.01f)
            {
                _movingOneTarget = false;
                _movingTwoTarget = false;
            }
            else if (Vector3.Distance(_friendTransform.position, _startPosition) < 0.01f)
            {
                _movingOneTarget = true;
            }

            yield return null;
        }
    }
}
