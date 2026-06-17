using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    public Color laserColor;
    public LayerMask layersToHit; // Слои для Raycast (стены, отражатели, приёмники)
    private LineRenderer lineRenderer;

    void Start() => lineRenderer = gameObject.AddComponent<LineRenderer>();

    void Update() => CastLaser();

    void CastLaser()
    {
        Vector3 start = transform.position;
        Vector3 dir = transform.forward;
        List<Vector3> points = new List<Vector3> { start };

        while (true)
        {
            Ray ray = new Ray(start, dir);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)) break;

            points.Add(hit.point);

            if (hit.transform.CompareTag("Receiver"))
            {
                Receiver receiver = hit.transform.GetComponent<Receiver>();
                if (receiver.expectedColor == laserColor) receiver.Activate();
                break;
            }
            else if (hit.transform.CompareTag("Reflector"))
            {
                Reflector reflector = hit.transform.GetComponent<Reflector>();
                dir = Vector3.Reflect(dir, reflector.GetNormal());
                start = hit.point;
            }
            else break; // Попадание в стену
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
    }
}
