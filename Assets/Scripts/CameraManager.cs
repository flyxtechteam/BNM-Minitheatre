using Klak.Spout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject viewer;

    [SerializeField] Vector3 offsetPosition;
    [SerializeField] Vector3 offsetRotation;

    private static CameraManager m_instance = null;

    public static CameraManager Instance
    {
        get
        {
            return m_instance;
        }
    }
    
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

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;

            gameObject.AddComponent<SpoutSender>();

            DontDestroyOnLoad(transform.gameObject);
        }

        else
        {
            DestroyImmediate(gameObject);
        }
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