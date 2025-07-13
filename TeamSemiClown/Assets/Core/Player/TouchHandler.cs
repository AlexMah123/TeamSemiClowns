
using UnityEngine;


public class TouchHandler : MonoBehaviour
{
    //Speed Contorl
    public float moveSpeed = 0.2f;

    //Player Controlls
    private PlayerControls controls;
    private bool isTouching = false;

    public static bool canMove = true;
    void Awake()
    {
        canMove = true;
        controls = new PlayerControls();
        controls.TouchControls.HoldAction.started += ctx => isTouching = true;
        controls.TouchControls.HoldAction.canceled += ctx => isTouching = false;

    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        if (isTouching && canMove)
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        }   
    }
}