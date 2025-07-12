using UnityEngine;

public class BudsTurnAroundState : BudsBaseState
{
    float m_StareTime;

    public override void EnterState(BudsStateMachine buds)
    {
        TurnAround();
        m_StareTime = 2.0f;
    }

    public override void ExitState(BudsStateMachine buds)
    {
    }

    public override void UpdateState(BudsStateMachine buds) // Add detection stuff here
    {
        if(!buds.doubleTake)
        {
            if (m_StareTime > 0)
            {
                m_StareTime -= Time.deltaTime;
            }   
            else
            {
                buds.SwitchState(buds.walkState);
            } 
        }
    }

    public override void OnTriggerEnter(Collider colliderInfo, BudsStateMachine buds) // If too hard, can add some i-frames
    {
        if (colliderInfo.CompareTag("Player"))
        {
            buds.SwitchState(buds.discoverState);
        }
    }
    public void TurnAround()
    {
        // Play Animation 
        //Detection 
        //Switch to Discover state
        // or back to walk 

    }
}
