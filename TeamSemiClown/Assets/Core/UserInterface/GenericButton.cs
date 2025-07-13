using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GenericButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Scale Settings")]
    [SerializeField] private float defaultScale = 1f;
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float clickScale = 1.2f;
    [SerializeField] private float tweenDuration = 0.15f;

    [Header("Additional OnClick Settings")]
    [SerializeField] private AudioClip audioToPlay;
    [SerializeField] private bool isPauseButton = false;

    private Vector3 originalScale;
    private Tween currentTween;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleTo(hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleTo(defaultScale);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(audioToPlay)
        {
            SFXController.Instance.PlaySoundFXClip(audioToPlay, transform, 1.0f);
        }

        // Reset everything, play a ferent scale animation for click
        currentTween?.Kill();
        transform.DOKill();

        Sequence clickSequence = DOTween.Sequence();
        clickSequence.Append(transform.DOScale(originalScale * clickScale, tweenDuration / 2).SetEase(Ease.OutQuad));
        clickSequence.Append(transform.DOScale(originalScale * hoverScale, tweenDuration / 2).SetEase(Ease.InQuad));
        clickSequence.Play();

        bool isInGameLevel = SceneManager.GetActiveScene().buildIndex == (int)SceneType.Game1 ||
                             SceneManager.GetActiveScene().buildIndex == (int)SceneType.Game2 ||
                             SceneManager.GetActiveScene().buildIndex == (int)SceneType.Game3;

        if (isInGameLevel && isPauseButton)
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }

    private void ScaleTo(float scaleFactor)
    {
        // Reset any existing tweens
        currentTween?.Kill();
        transform.DOKill();
        currentTween = transform.DOScale(originalScale * scaleFactor, tweenDuration).SetEase(Ease.OutBack);
    }
}
