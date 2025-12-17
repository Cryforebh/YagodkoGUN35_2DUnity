using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(RobotScannerTrashRaycast), typeof(NavMeshAgent), typeof(RobotScannerTrashPhisicOverlapSphere))]
public class RobotCleanerMovement : MonoBehaviour
{
    [Header("Character movement:")]
    [SerializeField] private float m_rotationSpeed = 20f;

    private NavMeshAgent m_agentNM;
    private RobotScannerTrashRaycast m_scannerTrasher;
    private RobotScannerTrashPhisicOverlapSphere m_scannerTrasherOverlapSphere;

    private bool m_isWaitNextJob = false;
    private bool m_isRotate = false;
    private WaitForSeconds m_timeWait;
    private void Awake()
    {
        m_agentNM = GetComponent<NavMeshAgent>();
        m_scannerTrasher = GetComponent<RobotScannerTrashRaycast>();
        m_scannerTrasherOverlapSphere = GetComponent<RobotScannerTrashPhisicOverlapSphere>();
        m_timeWait = new WaitForSeconds(1f);
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (!m_isWaitNextJob)
        {
            bool currentJob;

            // Если работает способ Сканера Мусора на Рейкасте
            // If the Raycast Trash Scanner class works
            if (m_scannerTrasher.enabled == true && m_scannerTrasher.IsViewTrash == true)
            {
                m_agentNM.destination = m_scannerTrasher.TrashTransform.position;
                currentJob = false;
            }
            // Если работает способ Сканера Мусора на OverlapSphere
            // If the OverlapSphere Garbage Scanner class works
            else if (m_scannerTrasherOverlapSphere.enabled == true && m_scannerTrasherOverlapSphere.IsViewTrash == true 
                && m_scannerTrasherOverlapSphere.CurrentTrashTransform != null)
            {
                m_agentNM.destination = m_scannerTrasherOverlapSphere.CurrentTrashTransform.position;
                currentJob = false;
            }
            else
            {
                transform.Rotate(Vector3.up, m_rotationSpeed * Time.deltaTime);
                currentJob = true;
            }

            if (currentJob != m_isRotate)
            {
                StartCoroutine(TimeStopDelay());
                m_isRotate = currentJob;
            }
        }
    }

    private IEnumerator TimeStopDelay()
    {
        m_isWaitNextJob = true;
        yield return m_timeWait;
        m_isWaitNextJob = false;
    }
}
