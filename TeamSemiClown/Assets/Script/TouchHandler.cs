using UnityEngine;
using UnityEngine.InputSystem;

public class TouchHandler : MonoBehaviour
{
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        controls.TouchControls.HoldAction.started += ctx => Debug.Log("Touch started");
        controls.TouchControls.HoldAction.performed += ctx => Debug.Log("Touch held");
        controls.TouchControls.HoldAction.canceled += ctx => Debug.Log("Touch released");
        
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();
}