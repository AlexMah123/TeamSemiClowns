using System.Collections;
using UnityEngine;

public class GhostFade : MonoBehaviour
{
    public SpriteRenderer ghostSprite;
    public float fadeInDuration = 2f;
    public float checkInterval = 0.1f;
    public float movementThreshold = 0.01f;
    public float visiblitlity = 0.1f;

    private Vector3 lastPosition;
    private Coroutine fadeCoroutine;
    private bool isVisible = false;

    void Start()
    {
        lastPosition = transform.position;
        SetAlpha(visiblitlity); // Start invisible
        StartCoroutine(CheckMovement());
    }

    IEnumerator CheckMovement()
    {
        while (true)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);

            if (distanceMoved < movementThreshold)
            {
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                    fadeCoroutine = null;
                }

                SetAlpha(visiblitlity); // Instantly invisible
                isVisible = false;
            }
            else
            {
                if (!isVisible && fadeCoroutine == null)
                {
                    fadeCoroutine = StartCoroutine(FadeIn());
                }
            }

            lastPosition = transform.position;
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
            ghostSprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ghostSprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        isVisible = true;
        fadeCoroutine = null;
    }

    void SetAlpha(float alpha)
    {
        Color color = ghostSprite.color;
        ghostSprite.color = new Color(color.r, color.g, color.b, alpha);
    }
}