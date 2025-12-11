using UnityEngine;

public class RobotScannerTrashRaycast : MonoBehaviour
{
    [Header("Before using, please turn off the other scanner!"), Space]
    [SerializeField] private float m_maxViewDistanceTrash = 10f;
    [SerializeField] private LayerMask m_trashMask;

    private Vector3 m_trashPosition;
    private Transform m_trashTransform;
    private bool m_isView;

    public Vector3 TrashPosition => m_trashPosition;
    public Transform TrashTransform => m_trashTransform;
    public bool IsViewTrash => m_isView;

    private void Update()
    {
        CalculationDetection();
        ProcessCleanToTrash();
    }

    private void CalculationDetection()
    {
        Ray rayForward = new Ray(transform.position, transform.forward);

        if (!m_isView)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayForward, out hit, m_maxViewDistanceTrash, m_trashMask))
            {
                m_trashTransform = hit.collider.transform;
                m_trashPosition = new Vector3(hit.collider.transform.position.x, transform.position.y, hit.collider.transform.position.z);
                m_isView = true;
            }
        }

        Debug.DrawRay(transform.position, transform.forward * 3f, Color.red);
    }

    private void ProcessCleanToTrash()
    {
        if (m_isView)
        {
            var distance = Vector3.Distance(transform.position, m_trashTransform.position);
            if (distance < 1f)
            {
                Debug.Log("Removed: " + m_trashTransform.name);
                Destroy(m_trashTransform.gameObject);
                m_isView = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_trashPosition, 0.05f);
    }
}
