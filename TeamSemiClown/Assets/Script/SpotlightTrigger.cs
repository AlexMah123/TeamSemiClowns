using UnityEngine;

public class SpotlightTrigger : MonoBehaviour
{
    public GameObject ghost;
    public bool triggerOnce = true;
    private bool hasTriggered = false;

    private SpriteRenderer ghostSprite;
    private Collider ghostCollider;
    private GhostFade ghostFade;

    public float spotlightAlpha = 1f; // Full visibility

    void Start()
    {
        if (ghost != null)
        {
            ghostSprite = ghost.GetComponent<SpriteRenderer>();
            ghostCollider = ghost.GetComponent<Collider>();
            ghostFade = ghost.GetComponent<GhostFade>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject == ghost && (!hasTriggered || !triggerOnce))
        {
            hasTriggered = true;
            Debug.Log("Ghost entered spotlight!");

            if (ghostFade != null)
                ghostFade.forceVisible = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject == ghost)
        {
            Debug.Log("Ghost exited spotlight!");

            if (ghostFade != null)
                ghostFade.forceVisible = false;

            hasTriggered = false;
        }
    }
}