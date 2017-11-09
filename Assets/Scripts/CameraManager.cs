﻿using Klak.Spout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject viewer;

    [SerializeField] Vector3 offsetPosition;
    [SerializeField] Vector3 offsetRotation;

    [SerializeField] Camera spoutCamera;

    private static CameraManager m_instance = null;

    public static CameraManager Instance
    {
        get
        {
            return m_instance;
        }
    }
    
    // Updates viewer object and updates post processing profile, sets playbackcontrol and assigns fade colours
    public void SetViewer(GameObject target, PostProcessingProfile profile = null)
    {
        viewer = target;

        PostProcessingBehaviour postProcess = transform.GetChild(0).GetComponent<PostProcessingBehaviour>();
        postProcess.profile = profile;

        PlaybackControl playback = gameObject.GetComponent<PlaybackControl>();

        playback.timeline = viewer.GetComponent<UnityEngine.Playables.PlayableDirector>();

        //Debug.Log("Now tracking " + target.name);
    }

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;

            spoutCamera.gameObject.AddComponent<SpoutSender>();

            PlaybackControl playback = gameObject.GetComponent<PlaybackControl>();

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