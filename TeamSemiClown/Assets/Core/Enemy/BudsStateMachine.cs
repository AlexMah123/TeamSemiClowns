using System.Transactions;
using UnityEngine;

public class BudsStateMachine : MonoBehaviour
{
    BudsBaseState currentState;
    public BudsIdleState idleState =  new(); //Use as initial state so they dont start moving right away
    public BudsWalkState walkState = new(); 
    public BudsTurnAroundState turnAroundState = new();
    public BudsScaredState scaredState = new();    // Use when buddie scares the bud
    public BudsDiscoverState discoverState = new(); // Use when the bud discover buddie

    public float speed = 1; 
    public int turnChance = 30; // x in 100 chance 
    public bool doubleTake = false; // bud will fake a turn around 

    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private float scareDistance = 3;
    void Start()
    {
        if (playerTransform == null)
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        currentState = idleState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= scareDistance && currentState != discoverState)
        {
            SwitchState(scaredState);
        }

        currentState.UpdateState(this);

        Debug.Log(currentState.ToString());
    }

    public void SwitchState(BudsBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
}
