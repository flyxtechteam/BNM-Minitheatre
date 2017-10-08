using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlaybackControl : MonoBehaviour
{
    [SerializeField] Animator pauseOverlay;
    [SerializeField] UnityEngine.UI.Image fadeOverlay;

    public FadeColor fadeColorIn;
    public FadeColor fadeColorOut;

    public PlayableDirector timeline;

    [SerializeField] float pauseDelay = 5f;
    [SerializeField] bool autoCycle = false;

    [SerializeField]
    private float timeOutDelay = 15f;

    bool pause = false;
    float scaleRate = 0.05f;
    float targetTimeScale = 1f;
    float LerpThreshold = 0.05f;
    float fadeDuration = 2f;

    bool hasTimedOut = false;
    
    // For debug only
    [HideInInspector]
    public float timeLeft = 0f;

    public void InitFadeOverlay(FadeColor fadeType)
    {
        if (fadeType == FadeColor.white)
        {
            fadeOverlay.color = Color.white;
        }
        else
        {
            fadeOverlay.color = Color.black;
        }
    }

    void Update ()
	{
        if (hasTimedOut)
        {
            return;
        }

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

        if (timeline != null)
        {
            if (!pause)
            {
                if (!Application.isEditor || !autoCycle)
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
                if ((Application.isEditor && autoCycle) || Input.GetKey(GlobalData.key_cycle))
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

            float timeOutEndTime = Time.unscaledTime + timeOutDelay;

            while (Time.unscaledTime < timeOutEndTime)
            {
                yield return null;
            }

            hasTimedOut = true;
            GlobalData.didTimeOut = true;

            if (pauseOverlay)
            {
                pauseOverlay.SetBool("paused", false);
            }
            
            fadeColorIn = FadeColor.white;
            fadeColorOut = FadeColor.white;

            float fadeStartTime = Time.unscaledTime;
            float fadeEndTime = fadeStartTime + fadeDuration;

            while (Time.unscaledTime < fadeEndTime)
            {
                float t = (Time.unscaledTime - fadeStartTime) / fadeDuration;

                AudioListener.volume = Mathf.Min(AudioListener.volume, 1 - t);
                fadeOverlay.color = new Color((float)fadeColorOut, (float)fadeColorOut, (float)fadeColorOut, t);

                yield return null;
            }

            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.UnloadSceneAsync(currentScene);

            SceneManager.LoadScene("LanguageSelectv2");

            pauseOverlay.gameObject.SetActive(false);

            hasTimedOut = false;

            targetTimeScale = 1f;
            timeLeft = 0f;
            pause = false;

            Time.timeScale = 1f;
        }
    }
}
