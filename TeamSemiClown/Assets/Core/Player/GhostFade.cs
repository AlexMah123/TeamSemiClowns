using System.Collections;
using UnityEngine;

public class GhostFade : MonoBehaviour
{
    // Sprite
    public SpriteRenderer ghostSprite;
    public float fadeInDuration = 2f;
    public float checkInterval = 0.1f;
    public float movementThreshold = 0.01f;
    public float visiblitlity = 0.1f;
    public float yellow = 0.65f;
    private Color oriColor;

    // Bobbing
    private Transform parentTransform;
    private Vector3 originalLocalPosition;
    private bool isMoving = false;
    public float bobAmount = 0.05f;
    public float bobSpeed = 1f;
    public float forwardDrift = 0.03f;

    // Extra
    private Vector3 lastPosition;
    private Coroutine fadeCoroutine;
    private bool isVisible = false;
    public Collider ghostCollider;
    public bool forceVisible = false;
    private bool previousForceVisible;

    void Start()
    {
        parentTransform = transform.parent;
        lastPosition = parentTransform.position;
        originalLocalPosition = transform.localPosition;
        oriColor = ghostSprite.color;

        previousForceVisible = forceVisible;
        ApplyGhostColor(visiblitlity); // Start faded
        StartCoroutine(CheckMovement());
    }

    IEnumerator CheckMovement()
    {
        while (true)
        {
            float distanceMoved = Vector3.Distance(parentTransform.position, lastPosition);

            if (distanceMoved < movementThreshold)
            {
                isMoving = false;

                if (!forceVisible)
                {
                    if (fadeCoroutine != null)
                    {
                        StopCoroutine(fadeCoroutine);
                        fadeCoroutine = null;
                    }

                    ApplyGhostColor(visiblitlity);
                    isVisible = false;
                }
            }
            else
            {
                isMoving = true;

                if (!isVisible && fadeCoroutine == null)
                {
                    fadeCoroutine = StartCoroutine(FadeIn());
                }
            }

            lastPosition = parentTransform.position;
            yield return new WaitForSeconds(checkInterval);
        }
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            float alpha = Mathf.Lerp(visiblitlity, 1f, elapsed / fadeInDuration);
            ApplyGhostColor(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ApplyGhostColor(1f);
        isVisible = true;
        fadeCoroutine = null;
    }

    void ApplyGhostColor(float alpha)
    {
        Color targetColor;

        if (forceVisible)
        {
            targetColor = new Color(oriColor.r, oriColor.g, yellow, alpha);
        }
        else
        {
            targetColor = new Color(oriColor.r, oriColor.g, oriColor.b, alpha);
        }

        ghostSprite.color = targetColor;

        if (ghostCollider != null)
        {
            ghostCollider.enabled = alpha > visiblitlity;
        }
    }

    void Update()
    {
        // Bobbing effect
        if (isMoving && ghostSprite.color.a > visiblitlity)
        {
            float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
            Vector3 driftOffset = transform.forward * forwardDrift;
            transform.localPosition = originalLocalPosition + new Vector3(0f, bobOffset, 0f) + driftOffset;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPosition, 5f * Time.deltaTime);
        }

        // Detect forceVisible change and update color immediately
        if (forceVisible != previousForceVisible)
        {
            ApplyGhostColor(ghostSprite.color.a);
            previousForceVisible = forceVisible;
        }
    }
}