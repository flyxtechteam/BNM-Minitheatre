﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlaybackControl : MonoBehaviour
{
    [SerializeField] Animator pauseOverlay;
    [SerializeField] UnityEngine.UI.Image fadeOverlay;

    public PlayableDirector timeline;

    [SerializeField] float pauseDelay = 5f;
    [SerializeField] bool autoCycle = false;

    [SerializeField] private float timeOutDelay = 15f;

    [SerializeField] UnityEngine.UI.Text timerText;

    [SerializeField] float fadePadding = 0.1f;

    bool pause = false;
    float scaleRate = 0.05f;
    float targetTimeScale = 1f;
    float LerpThreshold = 0.05f;
    float fadeDuration = 2f;

    bool hasTimedOut = false;
    
    // For debug only
    [HideInInspector]
    public float timeLeft = 0f;

    void Update ()
	{
        if (hasTimedOut)
        {
            return;
        }

        if (Input.GetKeyDown(GlobalData.key_toggleAutoCycle))
        {
            autoCycle = !autoCycle;
        }

        // if (!autoCycle) {} what's this for????

        // Update timeleft (for debug)
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0f;
        }

        if (timeline != null)
        {
            if (!pause)
            {
                if (((!Application.isEditor) && (!Debug.isDebugBuild)) || !autoCycle)
                {
                    if (!Input.GetKeyDown(GlobalData.key_cycle))
                    {
                        StartCoroutine("DelayPause");
                        timeLeft = pauseDelay;
                        pause = true;
                    }
                }
            }

            else
            {
                if ((((Application.isEditor) || (Debug.isDebugBuild)) && autoCycle) || Input.GetKey(GlobalData.key_cycle))
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
        }

        else
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

        /* original code
		if ((timeline != null && (!Input.GetKey(GlobalData.key_cycle)) && (!pause)) || ((!autoCycle) && (Application.isEditor)))
		{
            StartCoroutine("DelayPause");
            timeLeft = pauseDelay;
            pause = true;
        }
		
		if ((timeline != null && (Input.GetKey(GlobalData.key_cycle))) || ((autoCycle) && (Application.isEditor)))
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
        */

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
                fadeOverlay.color = new Color(fadeOverlay.color.r, fadeOverlay.color.g, fadeOverlay.color.b, 1 - fade);
            }
            // At end of scene
            else if (timeline.time >= timeline.duration - fadePadding - fadeDuration)
            {
                float fade = (float)(1f - ((timeline.time - (timeline.duration - fadePadding - fadeDuration)) / fadeDuration));

                AudioListener.volume = fade;
                fadeOverlay.color = new Color(fadeOverlay.color.r, fadeOverlay.color.g, fadeOverlay.color.b, 1 - fade);
            }
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
                //pauseOverlay.gameObject.SetActive(true);
            }

            float timeOutEndTime = Time.unscaledTime + timeOutDelay;

            while (Time.unscaledTime < timeOutEndTime)
            {
                timerText.text = Mathf.RoundToInt(timeOutEndTime - Time.unscaledTime).ToString();
                yield return null;
            }

            timerText.text = "0";

            hasTimedOut = true;
            GlobalData.didTimeOut = true;

            if (pauseOverlay)
            {
                pauseOverlay.SetBool("paused", false);
            }

            float fadeStartTime = Time.unscaledTime;
            float fadeEndTime = fadeStartTime + fadeDuration;

            while (Time.unscaledTime < fadeEndTime)
            {
                float t = (Time.unscaledTime - fadeStartTime) / fadeDuration;

                AudioListener.volume = Mathf.Min(AudioListener.volume, 1 - t);
                fadeOverlay.color = new Color(fadeOverlay.color.r, fadeOverlay.color.g, fadeOverlay.color.b, t);

                yield return null;
            }

            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.UnloadSceneAsync(currentScene);

            SceneManager.LoadScene("LanguageSelectv2");

            //pauseOverlay.gameObject.SetActive(false);

            hasTimedOut = false;

            targetTimeScale = 1f;
            timeLeft = 0f;
            pause = false;

            Time.timeScale = 1f;
        }
    }
}
