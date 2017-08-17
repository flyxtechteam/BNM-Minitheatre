using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelect : MonoBehaviour {

    KeyCode englishKey = KeyCode.A;
    KeyCode malayKey = KeyCode.D;

    // How long to hold key before scene loads
    [SerializeField]
    float delayTime = 2f;
    float currentTime = 0f;

    // How faded image and text appears when not selected
    [SerializeField]
    float fadeAlpha = 0.5f;
    float currentAlpha = 1f;

    // Recession rate
    [SerializeField]
    float damping = 0.1f;

    // Radial fill images and flag images
    [SerializeField]
    UnityEngine.UI.Image fillLeft, fillRight, flagLeft, flagRight;

    [SerializeField]
    UnityEngine.UI.Text textLeft, textRight;

    [SerializeField]
    Animator sceneTransition;

    bool transitioning = false;

    int language = 0; //0 - English, 1 - Malay
    
    void Update ()
    {   
        if (!transitioning)
        {
            // ENGLISH KEY IS PRESSED
            if (Input.GetKey(englishKey))
            {
                if (language == 1)
                {
                    language = 0;
                    currentTime = 0;
                }
                else
                {
                    if (currentTime < delayTime)
                    {
                        currentTime += Time.deltaTime;
                    }
                    else
                    {
                        CueSceneChange();
                    }
                }

                // set radial fill amount to reflect current progress
                fillLeft.fillAmount = currentTime / delayTime;
                fillRight.fillAmount = Mathf.Lerp(fillRight.fillAmount, 0f, damping);

                // fade out unselected flag image and text
                textRight.color = new Color(textRight.color.r, textRight.color.g, textRight.color.b, Mathf.Lerp(textRight.color.a, fadeAlpha, damping));
                flagRight.color = new Color(flagRight.color.r, flagRight.color.g, flagRight.color.b, Mathf.Lerp(flagRight.color.a, fadeAlpha, damping));

                // fade in selected flag
                textLeft.color = new Color(textLeft.color.r, textLeft.color.g, textLeft.color.b, Mathf.Lerp(textLeft.color.a, 1f, damping));
                flagLeft.color = new Color(flagLeft.color.r, flagLeft.color.g, flagLeft.color.b, Mathf.Lerp(flagLeft.color.a, 1f, damping));
            }
            // MALAY KEY IS PRESSED
            else if (Input.GetKey(malayKey))
            {
                if (language == 0)
                {
                    language = 1;
                    currentTime = 0;
                }
                else
                {
                    if (currentTime < delayTime)
                    {
                        currentTime += Time.deltaTime;
                    }
                    else
                    {
                        CueSceneChange();
                    }
                }

                // set radial fill amount to reflect current progress
                fillRight.fillAmount = currentTime / delayTime;
                fillLeft.fillAmount = Mathf.Lerp(fillLeft.fillAmount, 0f, damping);

                // fade out unselected flag image and text
                textLeft.color = new Color(textLeft.color.r, textLeft.color.g, textLeft.color.b, Mathf.Lerp(textLeft.color.a, fadeAlpha, damping));
                flagLeft.color = new Color(flagLeft.color.r, flagLeft.color.g, flagLeft.color.b, Mathf.Lerp(flagLeft.color.a, fadeAlpha, damping));

                // fade in selected flag
                textRight.color = new Color(textRight.color.r, textRight.color.g, textRight.color.b, Mathf.Lerp(textRight.color.a, 1f, damping));
                flagRight.color = new Color(flagRight.color.r, flagRight.color.g, flagRight.color.b, Mathf.Lerp(flagRight.color.a, 1f, damping));
            }
            // NO KEYS PRESSED
            else
            {
                // reset progress and both radial fills
                currentTime = 0f;
                fillLeft.fillAmount = Mathf.Lerp(fillLeft.fillAmount, 0f, damping);
                fillRight.fillAmount = Mathf.Lerp(fillRight.fillAmount, 0f, damping);

                // fade back in both flags and text
                textLeft.color = new Color(textLeft.color.r, textLeft.color.g, textLeft.color.b, Mathf.Lerp(textLeft.color.a, 1f, damping));
                textRight.color = new Color(textRight.color.r, textRight.color.g, textRight.color.b, Mathf.Lerp(textRight.color.a, 1f, damping));
                flagLeft.color = new Color(flagLeft.color.r, flagLeft.color.g, flagLeft.color.b, Mathf.Lerp(flagLeft.color.a, 1f, damping));
                flagRight.color = new Color(flagRight.color.r, flagRight.color.g, flagRight.color.b, Mathf.Lerp(flagRight.color.a, 1f, damping));
            }
        }
    }

    void CueSceneChange()
    {
        sceneTransition.enabled = true;

        StartCoroutine("SceneChange");
        transitioning = true;
    }

    IEnumerator SceneChange()
    {
        yield return new WaitUntil(() => sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_01");
    }
}