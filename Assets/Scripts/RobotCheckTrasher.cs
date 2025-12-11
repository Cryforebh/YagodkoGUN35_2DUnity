using System;
using UnityEngine;

public class RobotCheckTrasher : MonoBehaviour
{
    [SerializeField] private float m_maxViewDistanceTrasher = 10f;
    [SerializeField] private LayerMask m_trasherMask;

    private Vector3 m_trasherPosition;
    private Collider m_trasherCollider;
    private bool m_isView;

    public event Action DetectedEnterEvent;
    public event Action DetectedExitEvent;

    public Vector3 TrasherPosition => m_trasherPosition;
    public bool IsViewTrasher => m_isView;

    private void Update()
    {
        CalculationDetection();
        ProcessCleanToTrash();
    }

    private void CalculationDetection()
    {
        Ray rayForward = new Ray(transform.position, transform.forward);
        //Ray rayLeft = new Ray(transform.position, transform.forward - transform.right / 8);
        //Ray rayRight = new Ray(transform.position, transform.forward + transform.right / 8);

        if (!m_isView)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayForward, out hit, m_maxViewDistanceTrasher, m_trasherMask) /* ||
                Physics.Raycast(rayLeft, out hit, m_maxViewDistanceTrasher, m_trasherMask) ||
                Physics.Raycast(rayRight, out hit, m_maxViewDistanceTrasher, m_trasherMask)*/)
            {
                m_trasherCollider = hit.collider;
                m_trasherPosition = new Vector3(hit.collider.transform.position.x, transform.position.y, hit.collider.transform.position.z);
                m_isView = true;
                DetectedEnterEvent?.Invoke();
            }
        }

        Debug.DrawRay(transform.position, transform.forward * 3f, Color.red);
        //Debug.DrawRay(transform.position, (transform.forward - transform.right / 8) * 3f, Color.red);
        //Debug.DrawRay(transform.position, (transform.forward + transform.right / 8) * 3f, Color.red);
    }

    private void ProcessCleanToTrash()
    {
        if (m_isView)
        {
            var distance = Vector3.Distance(transform.position, m_trasherCollider.transform.position);
            if (distance < 1f)
            {
                Debug.Log("Убрано!");
                Destroy(m_trasherCollider.gameObject);
                m_isView = false;
                DetectedExitEvent?.Invoke();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_trasherPosition, 0.05f);
    }

    //private void Update()
    //{
    //    FindNearestTrashCollider();
    //}

    //private void FindNearestTrashCollider()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, m_maxViewDistanceTrasher * 5, m_trasherMask);
    //    Collider nearestTrashCollider = null;
    //    float minDistance = Mathf.Infinity;

    //    foreach (Collider collider in colliders)
    //    {
    //        float distance = Vector3.Distance(transform.position, collider.transform.position);

    //        if (distance < minDistance)
    //        {
    //            minDistance = distance;
    //            nearestTrashCollider = collider;
    //        }
    //    }

    //    if (nearestTrashCollider != null)
    //    {
    //        CalculationDetection(nearestTrashCollider);
    //    }
    //    else
    //    {
    //        Debug.Log("Мусора в зоне поиска нет");
    //    }
    //}

    //private void CalculationDetection(Collider target)
    //{
    //    if (target.tag == m_trashTag)
    //    {
    //        Vector3 thisPosition = transform.position;

    //        m_trasherPosition = target.transform.position;

    //        Vector3 directionToPlayer = m_trasherPosition - thisPosition;
    //        float distanceToPlayer = directionToPlayer.magnitude;

    //        Vector3 viewVector = transform.forward;

    //        float angle = Vector3.Angle(viewVector, directionToPlayer);

    //        bool isInViewAngle = angle <= m_desiredAngleDegrees;
    //        bool isInViewDistance = distanceToPlayer < m_maxViewDistanceTrasher;

    //        if (isInViewAngle && isInViewDistance)
    //        {
    //            StatusViewDebagLog(true);
    //            CalculationRaycastTarget(m_trasherPosition);
    //        }
    //        else
    //        {
    //            StatusViewDebagLog(false);
    //        }
    //    }
    //}

    //private void CalculationRaycastTarget(Vector3 trasherPorition)
    //{
    //    Vector3 directionToTrasher = trasherPorition - transform.position;

    //    if (Physics.Raycast(transform.position, directionToTrasher, out RaycastHit hit, m_maxViewDistanceTrasher/*, m_obstacleMask*/))
    //    {
    //        if (hit.collider.gameObject.tag == m_trashTag)
    //        {
    //            SetView(true);
    //            Debug.DrawRay(transform.position, hit.point);
    //            if (m_trasherPosition != null)
    //            {
    //                m_rigidbody.velocity = (m_trasherPosition - transform.position) * 40f;
    //            }
    //        }
    //        else
    //        {
    //            SetView(false);
    //        }
    //    }
    //}

    //private void SetView(bool isView)
    //{
    //    if (m_isView == isView) return;
    //    m_isView = isView;
    //    DetectedEvent?.Invoke();

    //    if (m_isView == true)
    //    {
    //        DetectedEnterEvent?.Invoke();
    //        print($"Мусор обнаружен.");
    //    }

    //    if (m_isView == false)
    //    {
    //        DetectedExitEvent?.Invoke();
    //        print($"Мусор потерян.");
    //    }
    //}

    //private void StatusViewDebagLog(bool isView)
    //{
    //    if (isView) Debug.Log("Обьект замечен");
    //}
}
