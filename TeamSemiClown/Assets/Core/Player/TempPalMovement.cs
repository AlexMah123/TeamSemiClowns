using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class TempPalMovement : MonoBehaviour
{

    public float speed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
