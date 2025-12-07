using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MoveFriendAsync : MonoBehaviour
{

    [SerializeField] private float _speed = 5f;
    [SerializeField] private List<Vector3> m_waypoints = new();

    private Transform m_moveTransform;
    private CancellationTokenSource m_cts;

    private void Awake()
    {
        m_moveTransform = GetComponent<Transform>();

        Vector3 startPosition = transform.position;
        m_waypoints.Add(startPosition);
    }

    private void OnEnable()
    {
        m_cts = new CancellationTokenSource();
        MoveAlongWaypointsAsync(m_cts.Token);
    }

    private void OnDisable()
    {
        m_cts?.Cancel();
        m_cts?.Dispose();
        m_cts = null;
    }

    private async Task MoveAlongWaypointsAsync(CancellationToken token)
    {
        try
        {
            int currentIndex = m_waypoints.Count - 1;

            while (!token.IsCancellationRequested)
            {
                int nextIndex = (currentIndex + 1) % m_waypoints.Count;

                Vector3 target;

                target = m_waypoints[nextIndex];

                await MoveToPositionAsync(target, token);

                currentIndex = nextIndex;
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("MoveAlongWaypoints cenceled");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private async Task MoveToPositionAsync(Vector3 target, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Vector3 newPosinion = Vector3.MoveTowards(
                m_moveTransform.position,
                target,
                _speed * Time.deltaTime
                );

            m_moveTransform.position = newPosinion;
            m_moveTransform.LookAt(target);

            if (Vector3.Distance(m_moveTransform.position, target) < 0.01f)
                break;

            await Task.Yield();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for (int i = 0; i < m_waypoints.Count; i++)
        {
            Gizmos.DrawSphere(m_waypoints[i],0.5f);
        }
        
    }
}
