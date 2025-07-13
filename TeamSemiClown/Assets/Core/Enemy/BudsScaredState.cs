using UnityEngine;

public class BudsScaredState : BudsBaseState
{
    public override void EnterState(BudsStateMachine buds) 
    {
        buds.animator.SetTrigger("Scared");
        TouchHandler.canMove = false;
        buds.winScreen.SetActive(true);
        //SceneTransitionController.Instance.LoadScene()
        //Trigger Score
        //Go next level 
    }

    public override void ExitState(BudsStateMachine buds)
    {
        
    }
    public override void UpdateState(BudsStateMachine buds)
    {
        
    }
}
