using UnityEngine;

public class HomelessStateIdle : HomelessStateBase
{
    private float m_waitTime = 5f;

    public override void EnterState(Homeless homeless)
    {
        m_waitTime = 5f;
    }

    public override void UpdateState(Homeless homeless)
    {
        homeless.CharacterAnimator.SetFloat("Movement", homeless.NavMeshAgent.velocity.magnitude);

        m_waitTime -= Time.deltaTime;

        if (m_waitTime <= 0)
        {
            homeless.SwitchState(homeless.HomelessStateSearchWork);
        }
    }
}
