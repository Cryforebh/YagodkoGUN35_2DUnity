using TMPro;
using UnityEngine;

public class TestGravity : MonoBehaviour
{
    public float JumpHeight = 3;
    public float Gravity = 20;
    public float AirControl = 0.5f;
    public float JumpDamp = 0.5f;
    public float GroundSpeed = 1;
    public float StepDown = 0.3f;

    //private Animator m_animator;
    private CharacterController m_characterController;
    private PlayerControls m_playerControls;
    private Vector3 m_input;

    Vector3 targetPosition;
    Vector3 velocity;
    bool isJumping;

    //int JumpParam = Animator.StringToHash("Jump");

    private void Start()
    {
        //m_animator = GetComponent<Animator>();
        m_characterController = GetComponent<CharacterController>();
        m_playerControls = new PlayerControls();
        m_playerControls.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        m_playerControls.PlayerMovement.Disable();
    }

    private void Update()
    {
        m_input.x = m_playerControls.PlayerMovement.Moving.ReadValue<Vector3>().x;
        m_input.z = m_playerControls.PlayerMovement.Moving.ReadValue<Vector3>().z;

        UpdateIsSprinting();

        if (m_playerControls.PlayerMovement.Jump.ReadValue<float>() > 0.3f)
        {
            Jump();
        }

        MoveCharacter(Time.deltaTime);
    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        targetPosition = isSprinting ? (m_input * 0.3f) : (m_input * 0.1f);
    }

    private bool IsSprinting()
    {
        bool isSprinting = m_playerControls.PlayerMovement.Acceleration.ReadValue<float>() > 0.25f;
        return isSprinting;
    }

    void MoveCharacter(float deltaTime)
    {
        if (isJumping)
        { // IsInAir state
            UpdateInAir(deltaTime);
            Debug.Log("Падает");
        }
        else
        { // IsGrounded state
            UpdateOnGround();
        }
    }

    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = targetPosition * GroundSpeed;
        Vector3 stepDownAmount = Vector3.down * StepDown;

        m_characterController.Move(stepForwardAmount + stepDownAmount);
        targetPosition = Vector3.zero;

        if (!m_characterController.isGrounded)
        {
            m_characterController.Move(-stepDownAmount);
            SetInAir(0);
        }
    }

    private void UpdateInAir(float deltaTime)
    {
        velocity.y -= Gravity * deltaTime;
        Vector3 displacement = velocity * deltaTime;
        displacement += CalculateAirControl();
        m_characterController.Move(displacement);
        isJumping = !m_characterController.isGrounded;
        targetPosition = Vector3.zero;
        //m_animator.SetBool(JumpParam, isJumping);
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = targetPosition * JumpDamp * GroundSpeed;
        velocity.y = jumpVelocity;
        //m_animator.SetBool(JumpParam, true);
    }

    private Vector3 CalculateAirControl()
    {
        return ((transform.forward * m_input.z) + (transform.right * m_input.x)) * (AirControl / 100);
    }

    void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * Gravity * JumpHeight);
            SetInAir(jumpVelocity);
        }
    }
}
