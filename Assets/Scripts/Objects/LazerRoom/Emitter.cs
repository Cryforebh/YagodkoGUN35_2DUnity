using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Emitter : LaserElementBase
{
    [SerializeField, Tooltip("Установить - true, если нужен конкретный Приемник. Тогда цвет не будет учитываться! Укажите Receiver заранее!")]
    private bool UseSpecificReceiver = false;
    [SerializeField]
    private Receiver _targetActiveReceiver;
    [SerializeField]
    private Transform _startLaser;
    [SerializeField]
    private LayerMask _layersToHit;
    [SerializeField]
    private Color LaserColor = Color.red;

    private LineRenderer _lineRenderer;
    private Color _defaultColor;

    private Vector3[] _pointsBuffer;
    private const int MAX_ITERATIONS = 10;

    private float _updateInterval = 0.02f; // интервал (в секундах)
    private CancellationTokenSource _cts;
    private CancellationToken _cancellationToken;

    public bool MoveAccess { get; private set; } = true;

    protected override void Start()
    {
        base.Start();

        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.startColor = LaserColor;
        _lineRenderer.endColor = LaserColor;
        _defaultColor = LaserColor;

        _pointsBuffer = new Vector3[MAX_ITERATIONS + 1]; // +1 для стартовой точки

        if (!UseSpecificReceiver)
            _targetActiveReceiver = null;
        else
            if (_targetActiveReceiver == null) Debug.LogError("Please, setup TargetActiveReceiver!");

        _cancellationToken = this.GetCancellationTokenOnDestroy();
        _cts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken);
        UpdateLoopAsync().Forget();
    }

    private async UniTask UpdateLoopAsync()
    {
        try
        {
            while (true)
            {
                await UniTask.Delay(
                    (int)(_updateInterval * 1000),
                    cancellationToken: _cts.Token);

                base.Update();
                DeterminingTheOperatingMethodOfTheReceiver();
            }
        }
        catch (OperationCanceledException)
        {
            // Отмена — ожидаемое завершение
        }
    }

    private void DeterminingTheOperatingMethodOfTheReceiver()
    {
        if (UseSpecificReceiver)
            CastLaserWithSpecificReceiver();
        else
            CastLaserByColor();
    }

    private void CastLaserCore(Action preCastAction, Func<RaycastHit, bool> handleReceiver)
    {
        preCastAction(); // Выполняем предварительные действия

        Vector3 start = _startLaser.position;
        Vector3 dir = _startLaser.forward;

        int pointIndex = 0;
        _pointsBuffer[pointIndex] = start; // Стартовая точка
        pointIndex++;

        int iterations = 0;

        while (iterations < MAX_ITERATIONS)
        {
            iterations++;

            Ray ray = new Ray(start, dir);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _layersToHit))
                break;

            _pointsBuffer[pointIndex] = hit.point;
            pointIndex++;

            if (hit.transform.CompareTag("Receiver"))
            {
                bool shouldBreak = handleReceiver(hit);
                if (shouldBreak) break;
            }
            else if (hit.transform.CompareTag("Reflector"))
            {
                Reflector reflector = hit.transform.GetComponent<Reflector>();
                if (reflector != null)
                {
                    Vector3 normal = hit.normal;
                    reflector.SetNormal(normal);
                    dir = Vector3.Reflect(dir, normal);
                    start = hit.point;
                }
                else
                    break; // Некорректный объект
            }
            else
                break; // Попадание в стену
        }

        _lineRenderer.positionCount = pointIndex;
        _lineRenderer.SetPositions(_pointsBuffer);
    }

    private void CastLaserWithSpecificReceiver()
    {
        CastLaserCore(
            () => _targetActiveReceiver.Deactivate(ref _lineRenderer, _defaultColor), // Предварительные действия
            hit =>
            {
                Receiver target = hit.transform.GetComponent<Receiver>();
                if (_targetActiveReceiver == target)
                {
                    _targetActiveReceiver.Activate(ref _lineRenderer, LaserColor);
                    return true; // Прервать цикл
                }
                return false;
            }
        );
    }

    private void CastLaserByColor()
    {
        Action preCast = () =>
        {
            if (_targetActiveReceiver != null)
            {
                _targetActiveReceiver.Deactivate(ref _lineRenderer, LaserColor);
                _targetActiveReceiver = null;
            }
        };

        Func<RaycastHit, bool> handleReceiver = hit =>
        {
            Receiver target = hit.transform.GetComponent<Receiver>();
            if (target != null && target.DefaultColor == LaserColor)
            {
                _targetActiveReceiver = target;
                _targetActiveReceiver.Activate(ref _lineRenderer, LaserColor);
                return true; // Прервать цикл
            }
            return false;
        };

        CastLaserCore(preCast, handleReceiver);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _cts?.Cancel();
        _lineRenderer = null;
    }

    private void OnDestroy()
    {
        OnDisable();
        _cts?.Dispose();
    }
}
