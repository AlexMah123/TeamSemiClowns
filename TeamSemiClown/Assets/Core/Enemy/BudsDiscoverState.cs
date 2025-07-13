using UnityEngine;

public class BudsDiscoverState : BudsBaseState
{
    public override void EnterState(BudsStateMachine buds)
    {
        buds.animator.SetTrigger("Discovered");
        TouchHandler.canMove = false;
        buds.loseCustom.SetActive(true);
        //SceneTransitionController.Instance.LoadScene()
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

