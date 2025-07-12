using UnityEngine;
public abstract class BudsBaseState
{
    public abstract void EnterState(BudsStateMachine buds);
    public abstract void UpdateState(BudsStateMachine buds);
    public abstract void ExitState(BudsStateMachine buds);
}
