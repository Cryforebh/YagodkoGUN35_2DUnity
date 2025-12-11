using UnityEngine;

public class RobotCollisionLogic : MonoBehaviour
{
    [SerializeField] private float m_moveDistanceFromObstacle = 2f;
    [SerializeField] private float m_moveBackDistanceFromObstacle = 3f;
    [SerializeField] private LayerMask m_obstacleMask;

    private bool m_isThereAnObstacle;
    private bool m_isWayOutOfObstaclesBeenFound;
    private bool m_isBack = false;
    private Collision m_currentObstacle;

    private int m_freeSide = 0;

    public bool IsThereAnObstacle { get => m_isThereAnObstacle; set => m_isThereAnObstacle = value; }
    public bool IsWayOutOfObstaclesBeenFound { get => m_isWayOutOfObstaclesBeenFound; set => m_isWayOutOfObstaclesBeenFound = value; }
    
    public float MoveDistanceFromObstacle => m_moveDistanceFromObstacle;
    public bool IsBack => m_isBack;

    public Vector3 FreeSide()
    {
        if (m_freeSide == -1) return -Vector3.right;
        else if (m_freeSide == 1) return Vector3.right;
        else return -Vector3.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Trash" && collision.gameObject.tag != "Floor")
        {
            m_currentObstacle = collision;
            m_isBack = true;
            m_isThereAnObstacle = true;
            m_isWayOutOfObstaclesBeenFound = false;
            print("Я с чем-то столкнулся!");
        }
    }

    public void Update()
    {
        if (m_isThereAnObstacle == true)
        {
            if (m_isWayOutOfObstaclesBeenFound == false)
            {
                ProcessOfTurningAwayFromAnObstacle();
            }
        }
    }

    public void ProcessOfTurningAwayFromAnObstacle()
    {
        Ray rayLeft = new Ray(transform.position, -transform.right);
        Ray rayRight = new Ray(transform.position, transform.right);

        Debug.DrawRay(rayLeft.origin, rayLeft.direction * m_moveDistanceFromObstacle, Color.blue);
        Debug.DrawRay(rayRight.origin, rayRight.direction * m_moveDistanceFromObstacle, Color.blue);

        if (m_isBack == true)
        {

            Debug.DrawLine(transform.position, m_currentObstacle.transform.position * m_moveBackDistanceFromObstacle, Color.blue);
            if (Vector3.Distance(transform.position, m_currentObstacle.transform.position) >= m_moveBackDistanceFromObstacle)
            {
                m_isBack = false;
            }
            m_freeSide = 0;
        }
        else
        {
            if (!Physics.Raycast(rayLeft, m_moveDistanceFromObstacle, m_obstacleMask))
            {
                m_isBack = false;
                m_isWayOutOfObstaclesBeenFound = true;
                m_freeSide = -1;
            }
            else if (!Physics.Raycast(rayRight, m_moveDistanceFromObstacle, m_obstacleMask))
            {
                m_isBack = false;
                m_isWayOutOfObstaclesBeenFound = true;
                m_freeSide = 1;
            }
            else
            {
                m_isBack = true;
                m_freeSide = 0;
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, -transform.right * m_moveDistanceFromObstacle);
        Gizmos.DrawRay(transform.position, transform.right * m_moveDistanceFromObstacle);
    }
}
