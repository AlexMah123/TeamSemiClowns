using UnityEngine;

public class BudsDiscoverState : BudsBaseState
{
    public override void EnterState(BudsStateMachine buds)
    {
        buds.animator.SetTrigger("Discovered");
        //play animation
        //end game
    }

    public override void ExitState(BudsStateMachine buds)
    {
        
    }

    public override void UpdateState(BudsStateMachine buds)
    {
        
    }
}

