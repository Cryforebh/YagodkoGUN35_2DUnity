using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Стартовое состояние:")]
    [SerializeField] private IDPlayerState m_startState;
    [SerializeField] private bool m_isMovementRB;

    [Header("Настройки взаимодействия:")]
    [SerializeField] private float m_speedWalk = 1.4f;
    [SerializeField] private float m_speedRun = 5f;
    [SerializeField] private float m_forceJump = 0.2f;
    [SerializeField] private float m_forceSpeedRotation = 10f;

    [Header("Настройки воздействия гравитации:")]
    [SerializeField] private LayerMask m_groundMask;
    [SerializeField] private Transform m_groundCheckerTransform;
    [SerializeField] private float m_gravityForce = 0.1f;
    [SerializeField] private float m_groundDistance = 1f;

    [HideInInspector] public Camera Camera;
    [HideInInspector] public PlayerControls Controls;
    [HideInInspector] public CharacterController CharacterControllerPlayer;
    [HideInInspector] public Rigidbody RB;
    [HideInInspector] public PlayerAnimations Animations;
    [HideInInspector] public PlayerStateMachine StateMachine;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerMovementRB MovementRB;
    [HideInInspector] public float CurrentForceGravity;
    [HideInInspector] public Vector3 TargetMoving;
    [HideInInspector] public Vector3 Velocity = Vector3.zero;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera = Camera.main;

        Animations = GetComponent<PlayerAnimations>();
        CharacterControllerPlayer = GetComponent<CharacterController>();
        RB = GetComponent<Rigidbody>();

        Controls = new PlayerControls();
        Controls.PlayerMovement.Enable();

        if (m_isMovementRB)
        {
            MovementRB = new PlayerMovementRB(this);
            RB.useGravity = true;
            CharacterControllerPlayer.enabled = false;
        }
        else
        {
            if (RB != null)
            {
                RB.useGravity = false;
                RB.velocity = Vector3.zero;
            }
            Movement = new PlayerMovement(this);
        }

        StateMachine = new PlayerStateMachine(this);
        StateMachine.AddState(new IldePlayerState());
        StateMachine.AddState(new MovePlayerState());
        StateMachine.AddState(new FallsPlayerState());
        StateMachine.AddState(new JumpPlayerState());
        StateMachine.ChangeState(m_startState);
    }

    private void Update()
    {
        Movement?.Update();
        StateMachine.Update();

        if (RB != null && !m_isMovementRB)
        {
            RB.velocity = Vector3.zero;
        }
    }

    private void LateUpdate()
    {
        MovementRB?.Update();
    }

    private void FixedUpdate()
    {
        MovementRB?.FixedUpdate();
    }

    private void OnDisable() => Controls.PlayerMovement.Disable();

    public bool IsGrounded() => CharacterControllerPlayer.isGrounded;
    public float GetSpeedWalk() => m_speedWalk;
    public float GetSpeedRun() => m_speedRun;
    public float GetForceJump() => m_forceJump;
    public float GetForceSpeedRotation() => m_forceSpeedRotation;
    public float GetGravityForce() => m_gravityForce;
    public float GetGroundDistance() => m_groundDistance;
    public LayerMask GetGroundMask() => m_groundMask;
    public Transform GetGroundCheckerTransform() => m_groundCheckerTransform;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_groundCheckerTransform.position, new Vector3(m_groundCheckerTransform.position.x, -m_groundCheckerTransform.position.y * m_groundDistance, m_groundCheckerTransform.position.z));
    }
}
