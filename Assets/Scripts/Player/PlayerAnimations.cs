using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void AnimationFalls(bool isFalls)
    {
        m_animator.SetBool("Falls", isFalls);
    }

    public void AnimationMove(float speed)
    {
        m_animator.SetFloat("Movement", speed);
    }

    public void AnimationMove(Vector3 direction)
    {
        var currentSpeed = direction.magnitude / Time.deltaTime;
        m_animator.SetFloat("Movement", currentSpeed);
    }

    public void AnimationJump(bool isJump)
    {
        m_animator.SetBool("Jump", isJump);
    }

    public void AnimationOnGrounded(bool isGrounded)
    {
        m_animator.SetBool("OnGrounded", isGrounded);
    }
}
