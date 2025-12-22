using UnityEngine;

public class HomelessStateCollect : HomelessStateBase
{
    private float m_timeCollect;
    private bool m_isCollected;

    public override void EnterState(Homeless homeless)
    {
        Debug.Log("О, манетка!");
        m_timeCollect = 0.1f;
        m_isCollected = false;
    }

    public override void UpdateState(Homeless homeless)
    {
        homeless.CharacterAnimator.SetFloat("Movement", homeless.NavMeshAgent.velocity.magnitude);

        if (!m_isCollected)
        {
            homeless.NavMeshAgent.destination = homeless.TargetCoin.transform.position;

            var distanceToCoin = Vector3.Distance(homeless.transform.position, homeless.TargetCoin.transform.position);

            if (distanceToCoin <= 1f)
            {
                homeless.TargetCoin.OnDestroy();
                homeless.CharacterAnimator.SetBool("Collected", true);
                m_isCollected = true;
            }
        }

        if (m_isCollected)
        {
            m_timeCollect -= Time.deltaTime;
            
            if (m_timeCollect <= 0f)
            {
                homeless.Voice.VoiceHomelessPlay(homeless);
                homeless.CharacterAnimator.SetBool("Collected", false);
                homeless.SwitchState(homeless.HomelessStateIdleWork);
            }
        }
    }
}
