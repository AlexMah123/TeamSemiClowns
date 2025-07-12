using System.Collections;
using UnityEngine;

public class GhostFade : MonoBehaviour
{   //Sprite
    public SpriteRenderer ghostSprite;
    public float fadeInDuration = 2f;
    public float checkInterval = 0.1f;
    public float movementThreshold = 0.01f;
    public float visiblitlity = 0.1f;

    //Bobbing
    private Transform parentTransform;
    private Vector3 originalLocalPosition;
    private bool isMoving = false;
    public float bobAmount = 0.05f;
    public float bobSpeed = 1f;
    public float forwardDrift = 0.03f;

    //Extra
    private Vector3 lastPosition;
    private Coroutine fadeCoroutine;
    private bool isVisible = false;
    public Collider ghostCollider;
    public bool forceVisible = false;

    void Start()
    {
        parentTransform = transform.parent;
        lastPosition = parentTransform.position;
        originalLocalPosition = transform.localPosition;
        SetAlpha(visiblitlity); // Start invisible
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

                    SetAlpha(visiblitlity); // Instantly invisible
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
        float elapsed = visiblitlity;
        Color originalColor = ghostSprite.color;

        while (elapsed < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            SetAlpha(alpha);
            //ghostSprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ghostSprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        isVisible = true;
        fadeCoroutine = null;

    }

    void SetAlpha(float alpha)
    {
        if (forceVisible)
        {
            alpha = 1f;
        }
        Color color = ghostSprite.color;
        ghostSprite.color = new Color(color.r, color.g, color.b, alpha);

        if (ghostCollider != null)
        {
            ghostCollider.enabled = alpha > visiblitlity;
        }
    }
    
    void Update()
    {
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
    }
}