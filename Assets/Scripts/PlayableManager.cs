using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayableManager : MonoBehaviour
{
    [SerializeField]
    PlayableAsset[] playables;

    int sceneCounter = 0;

    PlayableDirector timeline;
    PlaybackControl playbackControl;
    AsyncSceneLoader asyncSceneLoader;

    void Awake()
    {
        // Set viewer to be persistent across scenes
        // and delegate sceneLoaded to OnSceneLoad function (for tracking when scene changes)
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;

        timeline = GetComponent<PlayableDirector>();
        playbackControl = GetComponent<PlaybackControl>();
        asyncSceneLoader = GetComponent<AsyncSceneLoader>();
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        timeline.playableAsset = playables[sceneCounter];
        timeline.time = 0f;
        timeline.Play();

        asyncSceneLoader.levelToLoad = "Scene_0" + (sceneCounter + 2).ToString();

        sceneCounter++;
    }
}