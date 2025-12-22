using UnityEngine;

public class HomelessStateSearch : HomelessStateBase
{
    private Coin m_coin;
    private bool m_isSearchCoin = false;

    public override void EnterState(Homeless homeless)
    {
        m_isSearchCoin = false;
    }

    public override void UpdateState(Homeless homeless)
    {
        Moving(homeless);
        SearchToCoin(homeless);
    }

    private void Moving(Homeless homeless)
    {
        if (!m_isSearchCoin)
        {
            var distance = Vector3.Distance(homeless.transform.position, homeless.NavMeshAgent.destination);
            if (distance <= homeless.NavMeshAgent.stoppingDistance + 1f)
            {
                homeless.PathWalkContainer.UpdateDestination(homeless.NavMeshAgent);
            }
        }
    }

    private void SearchToCoin(Homeless homeless)
    {
        var colliders = Physics.OverlapSphere(homeless.transform.position, 5f);

        foreach (var collider in colliders)
        {
            Coin coin = collider.GetComponent<Coin>();

            if (coin != null)
            {
                m_coin = coin;
            }
        }

        if (m_coin != null)
        {
            m_isSearchCoin = true;
            MoveToCoin(homeless);
            return;
        }
        else
        {
            homeless.SwitchState(homeless.HomelessStateIdleWork);
        }
    }

    private void MoveToCoin(Homeless homeless)
    {
        homeless.TargetCoin = m_coin;
        homeless.SwitchState(homeless.HomelessStateCollectWork);
    }
}
