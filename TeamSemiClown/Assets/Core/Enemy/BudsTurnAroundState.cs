using DG.Tweening;
using UnityEngine;
using static DG.Tweening.DOTweenModuleUtils;

public class BudsTurnAroundState : BudsBaseState
{
    float m_StareTime;
    public override void EnterState(BudsStateMachine buds)
    {
        TurnAround(buds, Random.Range(0, 100) >= 60);
        m_StareTime = 2.0f;
    }

    public override void ExitState(BudsStateMachine buds)
    {
    }

    public override void UpdateState(BudsStateMachine buds)
    {

    }
    public void TurnAround(BudsStateMachine buds, bool secondTime = false)
    {
        buds.transform.DORotate(new(0, 180, 0), 0.2f).OnComplete(()=> 
        {
            Debug.DrawRay(buds.transform.position, (buds.transform.position + buds.GetPlayerTransform().position).normalized, Color.red, 1000f);
            if (UnityEngine.Physics.Raycast(buds.transform.position, (buds.transform.position + buds.GetPlayerTransform().position).normalized, out RaycastHit hit))
            {
                buds.SwitchState(buds.discoverState);
                return;
            }
                
            buds.transform.DORotate(new(0, 0, 0), 0.2f).SetDelay(m_StareTime).OnComplete(() => 
            {
                if (!buds.doubleTake || secondTime)
                    buds.SwitchState(buds.walkState);
                else
                    TurnAround(buds, true);
            });
        }
        );

        
        // Play Animation 
    }
}
