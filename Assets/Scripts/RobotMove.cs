using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RobotCheckTrasher), typeof(RobotCollisionLogic), typeof(Rigidbody))]
public class RobotMove : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 60f;
    [SerializeField] private float m_rotationSpeed = 20f;
    [SerializeField] private float m_distanceMoveBack = 2f;

    private RobotCheckTrasher m_checkTrasher;
    private RobotCollisionLogic m_obstacle;
    private Rigidbody m_rb;
    private bool m_isWaitNextJob = false;
    private bool m_isRotate;

    private Vector3 m_startPoritionOnObstacle;

    private void Awake()
    {
        m_checkTrasher = GetComponent<RobotCheckTrasher>();
        m_obstacle = GetComponent<RobotCollisionLogic>();
        m_rb = GetComponent<Rigidbody>();
    }

    private void ProcessMovement()
    {
        if (!m_isWaitNextJob)
        {
            bool currentJob;

            if (m_checkTrasher.IsViewTrasher == true)
            {
                if (m_obstacle.IsThereAnObstacle == true)
                {
                    // Препятствие нашел - срабатывает
                    if (m_obstacle.IsWayOutOfObstaclesBeenFound == false)
                    {
                        // Реагирует на найденный путь - срабатывает

                        if (m_obstacle.IsBack == true)
                        {
                            m_rb.velocity = -transform.forward * m_moveSpeed * Time.fixedDeltaTime;
                            m_startPoritionOnObstacle = transform.position;
                            print("Отьезжаю назад");
                        }
                        else
                        {
                            if (Vector3.Distance(transform.position, m_startPoritionOnObstacle) <= m_obstacle.MoveDistanceFromObstacle)
                            {
                                m_rb.velocity = m_obstacle.FreeSide() * m_moveSpeed * Time.fixedDeltaTime;
                                print("Обьезжаю препятствие");
                            }
                            else
                            {
                                m_obstacle.IsThereAnObstacle = false;
                                print("Путь свободен");
                            }
                        }
                    }
                }
                else
                {
                    //m_turnAwayFromAnObstacle = false;
                    transform.LookAt(m_checkTrasher.TrasherPosition);
                    m_rb.velocity = transform.forward * m_moveSpeed * Time.fixedDeltaTime;

                }
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

    //private void ProcessOfTurningAwayFromAnObstacle()
    //{
    //    m_currentYAngle = transform.eulerAngles.y;


    //    if (m_turnAwayFromAnObstacle)
    //    {
    //        if (!m_moveForward)
    //        {
    //            // Рассчитываем разницу углов
    //            float angleDifference = m_targetRotationAngleBack - m_currentYAngle;

    //            // Проверяем, достигнут ли целевой угол
    //            if (Mathf.Abs(angleDifference) > 0)
    //            {
    //                // Определяем направление поворота
    //                float rotationDirection = Mathf.Sign(angleDifference);
    //                transform.Rotate(Vector3.up, rotationDirection * m_rotationSpeed * Time.deltaTime);
    //            }
    //            else
    //            {
    //                // Целевой угол достигнут, прекращаем поворот
    //                m_moveForward = true;
    //                m_startPoritionOnObstacle = transform.position;
    //                print("Продолжаю двигаться!");
    //            }
    //        }
    //        else
    //        {
    //            if (Vector3.Distance(m_startPoritionOnObstacle, transform.position) < m_moveDistanceFromObstacle)
    //                m_rb.velocity = transform.forward * m_moveSpeed * Time.fixedDeltaTime;
    //            else
    //            {
    //                m_obstacle.isThereAnObstacle = false;
    //                m_turnAwayFromAnObstacle = false;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        // Определяем целевой угол
    //        m_targetRotationAngleBack = m_currentYAngle - m_rotationAngleBack;
    //    }

    //}

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    private IEnumerator TimeStopDelay()
    {
        m_isWaitNextJob = true;
        yield return new WaitForSeconds(0.5f);
        m_isWaitNextJob = false;
    }

    private bool IsWayClear()
    {
        if (!m_obstacle.IsThereAnObstacle) return true;



        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

        }

        return true;
    }
}
