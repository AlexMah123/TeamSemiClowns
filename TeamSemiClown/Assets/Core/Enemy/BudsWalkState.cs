using System.Collections;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class BudsWalkState : BudsBaseState
{
    float m_TurnCheck = 1.5f;
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
            turnCheck = null;
        }
    }
    public override void UpdateState(BudsStateMachine buds) //Add walking stuff here
    {
        buds.transform.position = buds.transform.position + buds.speed * Time.deltaTime * Vector3.forward;

        if (Vector3.Distance(buds.transform.position, buds.GetPlayerTransform().position) > 30)
        {
            buds.loseCustom.SetActive(true);
            //Transititon to default lose level
        }
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
