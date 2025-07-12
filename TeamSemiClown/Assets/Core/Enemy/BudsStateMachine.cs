using System.Transactions;
using UnityEngine;

public class BudsStateMachine : MonoBehaviour
{
    BudsBaseState currentState;
    BudsBaseState previousState;
    public BudsIdleState idleState =  new(); //Use as initial state so they dont start moving right away
    public BudsWalkState walkState = new(); 
    public BudsTurnAroundState turnAroundState = new();
    public BudsScaredState scaredState = new();    // Use when buddie scares the bud
    public BudsDiscoverState discoverState = new(); // Use when the bud discover buddie

    public float speed = 1; 
    public int turnChance = 30; // x in 100 chance 
    public bool doubleTake = false; 

    void Start()
    { 
        currentState = idleState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
        Debug.Log(currentState.ToString());
    }

    public void SwitchState(BudsBaseState state)
    {
        currentState.ExitState(this);
        previousState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other, this);
    }
}
