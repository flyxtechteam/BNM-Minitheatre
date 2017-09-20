using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    public string levelToLoad;

    [SerializeField]
    PlayableDirector timeline;

    [SerializeField]
    Animator sceneTransition;

    float timelineThreshold = 0.1f;
    float currentCounter = 0f;

    // Init async operation - will start loading once set from null
    AsyncOperation async = null;

    // Start loading on startup
    void Start()
    {
        StartCoroutine(LoadLevel(levelToLoad));
    }

    void Update()
    {
        // If a playable is assigned, check if it's done playing (playhead reached end)
        // and if so, allow scene activation (switch scenes)
        if (async != null)
        {
            if (timeline != null)
            {
                if (timeline.time >= timeline.duration - timelineThreshold)
                {
                    ActivateScene();
                }
            }
            // Besides playable, also accept Animator class for activating scene after a scene transition
            else if (sceneTransition != null)
            {
                if (sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    ActivateScene();
                }
            }
            // If neither are assigned, ActivateScene() must be called externally
        }

        
    }

    public void ActivateScene()
    {   
        // Activate scene
        async.allowSceneActivation = true;
    }

    // Start loading, but disallow scene activation (won't switch scenes once done loading)
	IEnumerator LoadLevel(string levelToLoad)
    {
        async = SceneManager.LoadSceneAsync(levelToLoad, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        async.allowSceneActivation = false;

        yield return async;

        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.UnloadSceneAsync(currentScene);
    }
}