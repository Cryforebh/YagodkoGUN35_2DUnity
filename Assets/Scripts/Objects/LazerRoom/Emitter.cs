using Cysharp.Threading.Tasks;
using Netologia.Quest;
using Netologia.Quest.Characters.Player;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Emitter : LaserElementBase
{
    [SerializeField, Tooltip("Установить - true, если нужен конкретный Приемник. Укажите Receiver заранее!")] 
    private bool UseSpecificReceiver = false;
    [SerializeField]
    private Receiver _targetActiveReceiver;
    [SerializeField]
    private Transform _startLaser;

    public Color _laserColor;
    public LayerMask _layersToHit;

    private LineRenderer _lineRenderer;

    private UniTask _updateTask; // задача для отслеживания цикла
    private float _updateInterval = 0.02f; // интервал (в секундах)

    public bool MoveAccess { get; private set; } = true;

    protected override void Start()
    {
        base.Start();

        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.startColor = _laserColor;
        _lineRenderer.endColor = _laserColor;
        _updateTask = UpdateLoopAsync();
    }

    private async UniTask UpdateLoopAsync()
    {
        while (true)
        {
            // Ждем заданный интервал
            await UniTask.Delay((int)(_updateInterval * 1000)); // конвертируем в миллисекунды

            base.Update();
            DeterminingTheOperatingMethodOfTheReceiver();
        }
    }

    private void DeterminingTheOperatingMethodOfTheReceiver()
    {
        if (UseSpecificReceiver)
            CastLaserWithSpecificReceiver();
        else
            CastLaser();
    }

    private void CastLaserWithSpecificReceiver()
    {
        _targetActiveReceiver.Deactivate(ref _lineRenderer, _laserColor);

        Vector3 start = _startLaser.position;
        Vector3 dir = _startLaser.forward;
        List<Vector3> points = new List<Vector3> { start };

        int maxIterations = 10; // Максимальное количество отражений
        int iterations = 0;

        while (iterations < maxIterations)
        {
            iterations++;

            Ray ray = new Ray(start, dir);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _layersToHit)) break;

            points.Add(hit.point);

            if (hit.transform.CompareTag("Receiver"))
            {
                if (_targetActiveReceiver == null)
                {
                    Debug.LogError("TargetActiveReceiver == null!");
                    return;
                }
                bool coincidence = _targetActiveReceiver == hit.transform.GetComponent<Receiver>();
                if (coincidence /*&& receiver.expectedColor == laserColor*/)
                {
                    _targetActiveReceiver.Activate(ref _lineRenderer, _laserColor);
                }
                break; // Выход из цикла
            }
            else if (hit.transform.CompareTag("Reflector"))
            {
                Reflector reflector = hit.transform.GetComponent<Reflector>();
                if (reflector != null) // Проверка на наличие компонента
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

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    private void CastLaser()
    {
        if (_targetActiveReceiver != null)
        {
            _targetActiveReceiver.Deactivate(ref _lineRenderer, _laserColor);
            _targetActiveReceiver = null;
        }

        Vector3 start = _startLaser.position;
        Vector3 dir = _startLaser.forward;
        List<Vector3> points = new List<Vector3> { start };

        int maxIterations = 10;
        int iterations = 0;

        while (iterations < maxIterations)
        {
            iterations++;

            Ray ray = new Ray(start, dir);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _layersToHit)) break;

            points.Add(hit.point);

            if (hit.transform.CompareTag("Receiver"))
            {
                var target = hit.transform.GetComponent<Receiver>();
                if (target != null /*&& receiver.expectedColor == laserColor*/)
                {
                    _targetActiveReceiver = target;
                    _targetActiveReceiver.Activate(ref _lineRenderer, _laserColor);
                }
                break; // Выход из цикла
            }
            else if (hit.transform.CompareTag("Reflector"))
            {
                Reflector reflector = hit.transform.GetComponent<Reflector>();
                if (reflector != null) // Проверка на наличие компонента
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

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _updateTask.ToCancellationToken(); // отмена задачи
        _lineRenderer = null;
    }
}
