using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BudsTurnAroundState : BudsBaseState
{
    float m_StareTime;
    bool isTurned = false;
    Coroutine discoverCheck = null;
    public override void EnterState(BudsStateMachine buds)
    {
        discoverCheck = buds.StartCoroutine(PlayerDetectOnTurn(buds, Random.Range(0, 100) >= 60));
        m_StareTime = 2.0f;
    }

    public override void ExitState(BudsStateMachine buds)
    {
        if (discoverCheck != null)
        {
            buds.StopCoroutine(discoverCheck);
            discoverCheck = null;
        }
    }

    public override void UpdateState(BudsStateMachine buds)
    {
        if (isTurned)
        {
            RaycastHit[] hits = Physics.RaycastAll(buds.transform.position, (buds.GetPlayerTransform().position - buds.transform.position).normalized);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Player"))
                {
                    buds.SwitchState(buds.discoverState);
                    continue;
                }
            }
        }
    }
    public void TurnAround(BudsStateMachine buds, bool secondTime = false) //Is backup, redundant for now
    {
        buds.transform.DORotate(new(0, 180, 0), 0.2f).OnComplete(()=> 
        {
            Debug.DrawRay(buds.transform.position, (buds.GetPlayerTransform().position - buds.transform.position).normalized, Color.red, 1000f);
            if (Physics.Raycast(buds.transform.position, (buds.GetPlayerTransform().position - buds.transform.position).normalized, out RaycastHit hit))
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

    IEnumerator PlayerDetectOnTurn(BudsStateMachine buds, bool secondTime = false)
    {

        buds.animator.SetTrigger("Turn");
        yield return new WaitForSeconds(1f);
            
        isTurned = true;
        yield return new WaitForSeconds(m_StareTime);
        buds.animator.SetTrigger("TurnBack");
        yield return new WaitForSeconds(0.3f);
        isTurned = false;
        if (!buds.doubleTake || secondTime)
            buds.SwitchState(buds.walkState);
        else
            discoverCheck =  buds.StartCoroutine(PlayerDetectOnTurn(buds, true));
    }
}
