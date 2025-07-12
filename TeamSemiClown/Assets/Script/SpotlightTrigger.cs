using UnityEngine;

public class SpotlightTrigger : MonoBehaviour
{
    public GameObject ghost;
    public bool triggerOnce = true;
    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ghost && (!hasTriggered || !triggerOnce))
        {
            hasTriggered = true;
            Debug.Log("Ghost entered spotlight!");
            // Trigger your action here — scare, sound, animation, etc.
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ghost)
        {
            Debug.Log("Ghost exited spotlight!");
            // Optional: reset or trigger something else
        }
    }
}