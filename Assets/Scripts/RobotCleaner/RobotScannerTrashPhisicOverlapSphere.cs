using System.Collections.Generic;
using UnityEngine;

public class RobotScannerTrashPhisicOverlapSphere : MonoBehaviour
{
    [Header("Before using, please turn off the other scanner!"), Space]
    [SerializeField] private float m_maxViewDistanceTrash = 10f;
    [SerializeField] private string m_trashTag = "Trash";

    private List<Collider>  m_trashesColliders = new List<Collider>();
    private Collider m_currentTrashCollider;
    private bool m_isView = false;

    public Transform CurrentTrashTransform => m_currentTrashCollider.transform;
    public bool IsViewTrash => m_isView;

    private void OnEnable()
    {
        var allColliders = Physics.OverlapSphere(transform.position, m_maxViewDistanceTrash);

        foreach (Collider hit in allColliders)
        {
            if (hit.tag == m_trashTag)
                m_trashesColliders.Add(hit);
        }
        Debug.Log("Garbage Trash: " + m_trashesColliders.Count);
    }

    private void ProcessCheckNearestTrash()
    {
        if (!m_isView)
        {
            if (m_trashesColliders != null && m_trashesColliders.Count > 0)
            {
                Collider nearestCollider = null;
                float nearestDistance = m_maxViewDistanceTrash;

                foreach (var trash in m_trashesColliders)
                {
                    float currentDistance = Vector3.Distance(transform.position, trash.transform.position);

                    if (nearestDistance > currentDistance)
                    {
                        nearestDistance = currentDistance;
                        nearestCollider = trash;
                    }
                }

                m_currentTrashCollider = nearestCollider;
                m_isView = true;

                Debug.Log("Target - " + m_currentTrashCollider.name);
            }
            else
            {
                Debug.Log("Everything is clear!");
            }
        }
    }

    private void Update()
    {
        ProcessCheckNearestTrash();
        ProcessCleanToTrash();
    }

    private void ProcessCleanToTrash()
    {
        if (m_currentTrashCollider != null && m_isView)
        {
            var distance = Vector3.Distance(transform.position, m_currentTrashCollider.transform.position);
            if (distance < 1f)
            {
                Debug.Log("Removed: " + m_currentTrashCollider.name);
                m_trashesColliders.Remove(m_currentTrashCollider);
                Destroy(m_currentTrashCollider.gameObject);
                m_isView = false;
            }
        }
    }
}
