using System.Collections;
using UnityEngine;

public class SceneTransitionHandler : MonoBehaviour
{
    public SceneType sceneToTransition;
    public Transition transitionType;

    private Coroutine coroutine;

    public void LoadScene()
    {
        Time.timeScale = 1;
        if (coroutine != null) return;

        //AudioManager.PlaySFX(AEnum.whoosh);
        coroutine = StartCoroutine(HandleLoadScene(0.1f));
    }

    private IEnumerator HandleLoadScene(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        SceneTransitionController.Instance.LoadScene(sceneToTransition, transitionType);
        coroutine = null;
    }
}