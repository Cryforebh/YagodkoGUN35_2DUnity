using Cysharp.Threading.Tasks;
using Netologia.Quest;
using Netologia.Quest.Characters.Player;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class Emitter : LaserElementBase
{
    public Color _laserColor;
    public LayerMask _layersToHit;

    private Receiver _targetActiveReceiver;
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

        InformationBureau.OnDialogClose += OnCancel;

        _updateTask = UpdateLoopAsync();
    }

    private async UniTask UpdateLoopAsync()
    {
        while (true)
        {
            // Ждём заданный интервал
            await UniTask.Delay((int)(_updateInterval * 1000)); // конвертируем в миллисекунды

            // Вызываем метод
            CastLaser();

            // Если нужно, вызываем базовый Update
            base.Update();
        }
    }

    //protected override void Update()
    //{
    //    OptimaizerUpdate();
    //}

    //private void OptimaizerUpdate()
    //{
    //    CastLaser();
    //    base.Update();
    //}

    private void CastLaser()
    {
        if (_targetActiveReceiver != null)
        {
            _targetActiveReceiver.Deactivate(ref _lineRenderer, _laserColor);
            _targetActiveReceiver = null;
        }

        Vector3 start = transform.position;
        Vector3 dir = transform.forward;
        List<Vector3> points = new List<Vector3> { start };

        int maxIterations = 100; // Максимальное количество отражений
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

    public bool OnInteract(PlayerController controller)
    {
        if (MoveAccess)
        {
            MoveAccess = false;
            return true;
        }
        return false;
    }

    private void OnCancel()
    {
        if (MoveAccess) return;
        MoveAccess = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _updateTask.ToCancellationToken(); // отмена задачи
        _lineRenderer = null;
        InformationBureau.OnDialogClose -= OnCancel;
    }
}
