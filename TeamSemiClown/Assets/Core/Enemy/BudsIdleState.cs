using UnityEngine;

public class BudsIdleState : BudsBaseState
{
    float m_IdleTime;

    public override void EnterState(BudsStateMachine buds)
    {
        m_IdleTime = 0.2f;
    }

    public override void ExitState(BudsStateMachine buds)
    {
    }

    public override void OnTriggerEnter(Collider colliderInfo, BudsStateMachine buds)
    {

    }

    public override void UpdateState(BudsStateMachine buds) //Check for scared in case of AI bugged
    {
        if (m_IdleTime > 0)
        {
            m_IdleTime -= Time.deltaTime;
        }
        else
        { 
            buds.SwitchState(buds.walkState);
        }
    }
}
