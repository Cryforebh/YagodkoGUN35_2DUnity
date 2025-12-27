using DG.Tweening;
using UnityEngine;

public class BotAIMovement : MonoBehaviour
{
    [SerializeField] private Transform m_waypointOneTransform;
    [SerializeField] private Transform m_waypointTwoTransform;
    [SerializeField] private Transform m_waypointThreeTransform;
    [SerializeField] private Transform m_waypointFourTransform;
    [SerializeField] private Transform m_waypointFiveTransform;

    [SerializeField] private float m_durationMove = 1f;
    [SerializeField] private float m_durationJump = 0.5f;
    [SerializeField] private Ease m_easeJump = Ease.Linear;
    [SerializeField, Range(1, 5)] private int m_timePause = 3;
    [SerializeField] private LayerMask m_ostacleLayer;

    private Vector3 m_rotateEnd = new Vector3(0, 180, 0);
    private Vector3 m_standartForm;
    private Quaternion m_rotateStart;
    private Transform m_targetWaypoint;
    private bool m_isEndWaypoint;
    private bool m_isJumpRotate;

    private StateAI m_currentStateAI;
    private StateWaypoint m_currentWaypoint;

    private enum StateWaypoint
    {
        WaypointOne,
        WaypointTwo,
        WaypointThree,
        WaypointFour,
        WaypointFive
    }

    private enum StateAI
    {
        Idle,
        Walk,
        Jump
    }

    private StateAI State
    {
        get
        {
            return m_currentStateAI;
        }
        set
        {
            m_currentStateAI = value;
            NextState(m_currentStateAI);
        }
    }

    private void OnEnable()
    {
        m_standartForm = transform.localScale;
        m_rotateStart = transform.rotation;
        m_waypointOneTransform.position = transform.position;
        m_currentWaypoint = StateWaypoint.WaypointTwo;
        State = StateAI.Walk;
    }
    private void NextState(StateAI state)
    {
        switch (state)
        {
            case StateAI.Idle:
                StateIdle();
                break;
            case StateAI.Walk:
                StateWalk();
                break;
            case StateAI.Jump:
                StateJump();
                break;
            default:
                break;
        }
    }

    private void StateIdle()
    {
        ScaleDeformOnIdle();
    }

    private void ScaleDeformOnIdle()
    {
        Sequence animations = DOTween.Sequence();

        animations.Append
            (transform.DOScale(new Vector3(1.3f, 1, 1.3f), 0.5f).SetLoops(m_timePause, LoopType.Yoyo))
            .Append
            (transform.DOScale(m_standartForm, 0.5f))
            .OnComplete(() => State = StateAI.Walk);
    }

    private void StateWalk()
    {
        NextWaypoint(m_currentWaypoint);

        if (CheckObstacleOnFront())
        {
            State = StateAI.Jump;
        }
        else
        {
            Sequence animations = DOTween.Sequence();

            animations.Append
                (transform.DOScale(transform.localScale + new Vector3(0, 0, 0.5f), 0.5f).SetLoops(1, LoopType.Yoyo))
                .Join
                (transform.DOMove(m_targetWaypoint.position, m_durationMove).SetEase(Ease.Flash))
                .Append
                (transform.DOScale(m_standartForm, 0.5f))
                .OnComplete(() => HandleWaypointReached());
        }
    }

    private void StateJump()
    {
        Sequence animations = DOTween.Sequence();

        animations.Append
            (transform.DOJump(m_targetWaypoint.position, 2f, 1, m_durationJump).SetEase(m_easeJump))
            .Join
            (transform.DOScale(transform.localScale + new Vector3(0, 0.5f, 0), m_durationJump))
            .Append
            (transform.DOScale(m_standartForm, m_durationJump / 2))
            .OnComplete(() => HandleWaypointReached());
    }

    private void HandleWaypointReached()
    {
        JumpRotate(m_isEndWaypoint);

        State = StateAI.Idle;

        if (m_currentStateAI != StateAI.Jump)
        {
            if (m_isEndWaypoint)
                m_currentWaypoint -= 1;
            else
                m_currentWaypoint += 1;
        }
    }

    private void NextWaypoint(StateWaypoint nextWaypoint)
    {
        switch (nextWaypoint)
        {
            case StateWaypoint.WaypointOne:
                m_targetWaypoint = m_waypointOneTransform;
                m_isEndWaypoint = false;
                m_isJumpRotate = true;
                break;
            case StateWaypoint.WaypointTwo:
                m_targetWaypoint = m_waypointTwoTransform;
                m_isJumpRotate = false;
                break;
            case StateWaypoint.WaypointThree:
                m_targetWaypoint = m_waypointThreeTransform;
                m_isJumpRotate = false;
                break;
            case StateWaypoint.WaypointFour:
                m_targetWaypoint = m_waypointFourTransform;
                m_isJumpRotate = false;
                break;
            case StateWaypoint.WaypointFive:
                m_targetWaypoint = m_waypointFiveTransform;
                m_isEndWaypoint = true;
                m_isJumpRotate = true;
                break;
            default:
                break;
        }
    }

    private void JumpRotate(bool end)
    {
        if (end)
            transform.DORotate(m_rotateStart.eulerAngles + m_rotateEnd, 0.7f);
        else
            transform.DORotate(m_rotateStart.eulerAngles, 0.7f);

        if (m_isJumpRotate)
        {
            Sequence animations = DOTween.Sequence();

            animations = transform.DOJump(transform.position, 2f, 1, m_durationJump).SetEase(m_easeJump)
            .Join
            (transform.DOScale(transform.localScale + new Vector3(0, 0.5f, 0), m_durationJump))
            .Append
            (transform.DOScale(m_standartForm, m_durationJump / 2));
        }

    }

    private bool CheckObstacleOnFront()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, (transform.position - m_targetWaypoint.position).magnitude, m_ostacleLayer))
        {
            return true;
        }
        return false;
    }
}
