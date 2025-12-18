using UnityEngine;

public class CameraMoveToPlayer : MonoBehaviour
{
    [SerializeField] private Transform m_targetTransform;
    private Vector3 m_startPosition;
    public float smoothSpeed = 5f; // Скорость сглаживания

    private void Awake()
    {
        m_startPosition = transform.localPosition;
    }

    private void Update()
    {
        Vector3 targetPosition = new Vector3(
            m_targetTransform.position.x,
            m_targetTransform.position.y + m_startPosition.y,
            m_targetTransform.position.z + m_startPosition.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
