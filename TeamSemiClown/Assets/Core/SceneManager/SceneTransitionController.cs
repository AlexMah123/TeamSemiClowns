using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public enum SceneType
{
    Exit = -1,
    Menu,
    Tutorial,
    Game1,
    Game2,
    Game3,
}

[Serializable]
public enum Transition
{
    CrossFade,
    CircleWipe,
}

public class SceneTransitionController : MonoBehaviour
{
    public static SceneTransitionController Instance;

    [Header("Transition Configs")]
    public GameObject transitionsContainer;
    private SceneTransition[] transitions;

    private bool _isTransitioning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
    }

    public void LoadScene(SceneType scene, Transition transitionType)
    {
        //early return to prevent spamming
        if (_isTransitioning) return;

        StartCoroutine(LoadSceneAsync(scene, transitionType));
    }

    private IEnumerator LoadSceneAsync(SceneType sceneType, Transition transitionType)
    {
        //set flag to true
        _isTransitioning = true;


        if (sceneType == SceneType.Exit)
        {
            Application.Quit();

            //#DEBUG
            Debug.Log("Quitting Game");

            //early reset
            _isTransitioning = false;
            yield break;
        }

        //get the correct sceneObject
        SceneTransition transition = transitions.First(t => t.transitionType == transitionType);
        AsyncOperation scene;

        scene = SceneManager.LoadSceneAsync((int)sceneType);

        scene.allowSceneActivation = false;

        //play animation to transition into new scene
        yield return transition.AnimateTransitionIn();

        scene.allowSceneActivation = true;

        //play animation to transition out of new scene
        yield return transition.AnimateTransitionOut();

        //reset flag
        _isTransitioning = false;
    }
}