using System.Collections;
using UnityEngine;

public class BotMovementToWaypoints : MonoBehaviour
{
    [Header("Функционал простого Твина:")]
    [SerializeField] private Transform[] m_waypointsTransform;
    [SerializeField] private float m_intervalToMoving = 3f;
    [SerializeField] private float m_maxDistanceFromWaypoint = 1f;
    [SerializeField] private float m_speedMoving = 3f;

    private Vector3 m_targetPosition;
    private bool m_isMoving;
    private int m_currentIndexWaypoint = 0;
    private float m_currentInterval;

    //******************************************

    [Header("Анимационная кривая:")]
    [SerializeField] private bool m_isAnimationCurveOff = true;
    [SerializeField] private AnimationCurve m_animationCurve;

    private float m_timer = 0f;
    private Vector3 m_lerpOffset = new Vector3(0, 3, 0);
    private Vector3 m_startPosition;

    //******************************************

    private void OnEnable()
    {
        m_startPosition = transform.position;
        m_currentInterval = m_intervalToMoving;
        m_targetPosition = m_waypointsTransform[m_currentIndexWaypoint].position;
    }


    private void Update()
    {
        if (m_isAnimationCurveOff)
        {
            TwinMoving();
        }
        else
        {
            AnimationCurveMoving();
        }
    }

    private void AnimationCurveMoving()
    {
        m_timer += Time.deltaTime;


        if (m_timer >= m_intervalToMoving)
        {
            m_timer = 0; // Сброс таймера
            NextIndexWaypoint();
            m_startPosition = transform.position;
        }

        float _lerpRation = m_timer / m_intervalToMoving;

        Vector3 positionOffset = m_animationCurve.Evaluate(_lerpRation) * m_lerpOffset;
        transform.position = Vector3.Lerp(m_startPosition, m_targetPosition, _lerpRation) + positionOffset;
    }

    private void TwinMoving()
    {
        if (Time.time >= m_currentInterval && !m_isMoving)
        {
            m_isMoving = true;
            m_currentInterval += m_intervalToMoving;
            StartCoroutine(ProcessMovingToWaypoint());
        }
    }

    private IEnumerator ProcessMovingToWaypoint()
    {
        while (Vector3.Distance(transform.position, m_targetPosition) > m_maxDistanceFromWaypoint)
        {
            transform.position = Vector3.Lerp(transform.position, m_targetPosition, m_speedMoving * Time.deltaTime);
            yield return null;
        }
        m_isMoving = false;

        NextIndexWaypoint();
    }

    private void NextIndexWaypoint()
    {
        m_currentIndexWaypoint++;
        if (m_waypointsTransform.Length <= m_currentIndexWaypoint)
            m_currentIndexWaypoint = 0;

        m_targetPosition = m_waypointsTransform[m_currentIndexWaypoint].position;
    }
}
