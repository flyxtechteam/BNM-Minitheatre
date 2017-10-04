using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlaybackControl : MonoBehaviour
{
    [SerializeField] Animator pauseOverlay;
    [SerializeField] UnityEngine.UI.Image fadeOverlay;

    public FadeColor fadeColorIn;
    public FadeColor fadeColorOut;

    public PlayableDirector timeline;

    [SerializeField] float pauseDelay = 5f;
    [SerializeField] bool autoCycle = false;

    bool pause = false;
    float scaleRate = 0.05f;
    float targetTimeScale = 1f;
    float LerpThreshold = 0.05f;
    float fadeDuration = 2f;

    

    // For debug only
    [HideInInspector]
    public float timeLeft = 0f;

    /*void InitFadeOverlay(FadeColor fadeType)
    {
        if (fadeType == FadeColor.white)
        {
            fadeOverlay.color = Color.white;
        }
        else
        {
            fadeOverlay.color = Color.black;
        }
    }*/

    void Update ()
	{
        if (!autoCycle)
        {

        }
        // Update timeleft (for debug)
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0f;
        }

		if (timeline != null && (!Input.GetKey(GlobalData.key_cycle)) && (!pause) && ((!autoCycle) && (Application.isEditor)))
		{
            StartCoroutine("DelayPause");
            timeLeft = pauseDelay;
            pause = true;
        }
		
		if (timeline != null && (Input.GetKey(GlobalData.key_cycle)) || ((autoCycle) && (Application.isEditor)))
		{ 
            if (pause)
            {
                StopCoroutine("DelayPause");
                targetTimeScale = 1f;
                timeLeft = 0f;
                pause = false;

                if (pauseOverlay)
                {
                    pauseOverlay.SetBool("paused", false);
                }                
            }
        }

        if (targetTimeScale == 1f)
        {
            if (Time.timeScale < (1f - LerpThreshold))
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, scaleRate);
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        else
        {
            if (targetTimeScale == 0f)
            {
                if (Time.timeScale > LerpThreshold)
                {
                    Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, scaleRate);
                }
                else
                {
                    Time.timeScale = 0f;
                }
            }
        }

        if (timeline != null)
        {
            // Fade audio and visuals as timeline starts off and approaches end
            // At start of scene
            if (timeline.time < fadeDuration)
            {
                float fade = (float)(timeline.time / fadeDuration);

                AudioListener.volume = fade;
                fadeOverlay.color = new Color((float)fadeColorIn, (float)fadeColorIn, (float)fadeColorIn, 1 - fade);
            }
            // At end of scene
            else if (timeline.time >= timeline.duration - fadeDuration)
            {
                float fade = (float)(1f - ((timeline.time - (timeline.duration - fadeDuration)) / fadeDuration));

                AudioListener.volume = fade;
                fadeOverlay.color = new Color((float)fadeColorOut, (float)fadeColorOut, (float)fadeColorOut, 1 - fade);
            }
        }

        else
        {
            fadeOverlay.color = Color.clear;
        }
	}

    IEnumerator DelayPause()
    {
        yield return new WaitForSecondsRealtime(pauseDelay);

        if (pause)
        {
            targetTimeScale = 0f;

            if (pauseOverlay)
            {
                pauseOverlay.SetBool("paused", true);
                pauseOverlay.gameObject.SetActive(true);
            }
        }
    }
}
