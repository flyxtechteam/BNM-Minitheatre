using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ViewerManager : MonoBehaviour {

    // Responsible for setting the camera to track its own movement at the start of a scene
    // also stores scene-specific information - e.g. post processing profile setting, fade in/out type

    [SerializeField] PostProcessingProfile scenePostProcessing;
    [SerializeField] FadeColor fadeInColor;
    [SerializeField] FadeColor fadeOutColor;

    // Set the camera's current tracking object to this object
    void Start ()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("camera");

        if (camera)
        {
            CameraManager camManager = camera.GetComponent<CameraManager>();
            camManager.SetViewer(transform.gameObject, scenePostProcessing, fadeInColor, fadeOutColor);
        }
	}
}

public enum FadeColor {white = 1, black = 0}