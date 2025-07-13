using System.Collections;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class BudsWalkState : BudsBaseState
{
    float m_TurnCheck = 3f;
    Coroutine turnCheck = null;
    int plusChance;
    public override void EnterState(BudsStateMachine buds)
    {
        plusChance = 0;
        turnCheck = buds.StartCoroutine(TurnTriggerLoop(buds));
    }

    public override void ExitState(BudsStateMachine buds)
    {
        if (turnCheck != null)
        {
            buds.StopCoroutine(turnCheck);
        }
    }
    public override void UpdateState(BudsStateMachine buds) //Add walking stuff here
    {
        buds.transform.position = buds.transform.position + Vector3.forward * buds.speed * Time.deltaTime;
    }

    IEnumerator TurnTriggerLoop(BudsStateMachine buds)
    {
        yield return new WaitForSeconds(m_TurnCheck);
    
        while (Random.Range(0, 100) > buds.turnChance + plusChance)
        {
            yield return new WaitForSeconds(m_TurnCheck);
            Debug.Log(plusChance);
            plusChance++;
        }
        
        buds.SwitchState(buds.turnAroundState);
    }
}
