using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject viewer;

    [SerializeField] Vector3 offsetPosition;
    [SerializeField] Vector3 offsetRotation;
    
    // Updates viewer object and updates post processing profile, sets playbackcontrol and assigns fade colours
    public void SetViewer(GameObject target, PostProcessingProfile profile = null, FadeColor fadeIn = FadeColor.white, FadeColor fadeOut = FadeColor.white)
    {
        viewer = target;

        PostProcessingBehaviour postProcess = transform.GetChild(0).GetComponent<PostProcessingBehaviour>();
        postProcess.profile = profile;

        PlaybackControl playback = gameObject.GetComponent<PlaybackControl>();

        playback.fadeColorIn = fadeIn;
        playback.fadeColorOut = fadeOut;
        playback.timeline = viewer.GetComponent<UnityEngine.Playables.PlayableDirector>();

        Debug.Log("Now tracking " + target.name);
    }

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        TrackObj();
    }  

    void TrackObj()
    {
        if (viewer != null)
        {
            transform.position = viewer.transform.position + offsetPosition;

            transform.rotation = viewer.transform.rotation;
            transform.Rotate(offsetRotation);
        }
    }
}