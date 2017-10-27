using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelect : MonoBehaviour {

    // How long to hold key before scene loads
    [SerializeField]
    float delayTime = 2f;
    float currentTime = 0f;

    // Recession rate
    [SerializeField]
    float damping = 0.1f;

    // Radial fill images and flag images
    [SerializeField]
    Animator fillLeft, fillRight;

    [SerializeField]
    UITextHandler textThankYou1, textThankYou2;

    [SerializeField]
    CanvasGroup selectionUI, thankYouUI;

    [SerializeField]
    float confirmedScreenTime = 5f;

    [SerializeField]
    float UIFadeTime = 1f;

    [SerializeField]
    Animator sceneTransition;

    [SerializeField]
    GameObject leftArrow, rightArrow;

    bool selectionUIVisible = true;
    bool thankYouUIVisible = false;
    bool blockInput = false;
    bool MODE2_selected = false;

    int language = 0; //0 - English, 1 - Malay

    bool isSeated = false;
    float seatedStateChangeTime = 0f;
    float brakeInputDelay = 1f;

    [Header("Screen Saver Properties")]
    [SerializeField]
    private SlideShowController screenSaver;

    [SerializeField]
    private float screenSaverDelay = 5f;

    private static int loadCount = 0;

    void Start()
    {
        fillLeft.speed = 0f;
        fillRight.speed = 0f;
        
        if (GlobalData.didTimeOut || loadCount == 0)
        {
            screenSaver.Activate(true);
        }

        GlobalData.didTimeOut = false;
        seatedStateChangeTime = Time.time;

        loadCount++;
    }

    void Update()
    {
        // Two modes of input; use /**/ to comment out the unwanted mode of input

        {
            /*
            // ENGLISH KEY IS PRESSED
            if ((Input.GetKey(GlobalData.key_left)) && (!blockInput))
            {
                if (!blockInput)
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
                            GlobalData.language = 0;
                            SelectionConfirmed();
                        }
                    }
                }

                // set radial fill amount to reflect current progress
                fillLeft.Play(0, 0, currentTime / delayTime);
                fillRight.Play(0, 0, Mathf.Lerp(fillRight.GetCurrentAnimatorStateInfo(0).normalizedTime, 0f, damping));

                leftArrow.SetActive(true);
                rightArrow.SetActive(false);
            }
            // MALAY KEY IS PRESSED
            else if ((Input.GetKey(GlobalData.key_right)) && (!blockInput))
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
                        GlobalData.language = 1;
                        SelectionConfirmed();
                    }
                }

                // set radial fill amount to reflect current progress
                fillLeft.Play(0, 0, Mathf.Lerp(fillLeft.GetCurrentAnimatorStateInfo(0).normalizedTime, 0f, damping));
                fillRight.Play(0, 0, currentTime / delayTime);

                leftArrow.SetActive(false);
                rightArrow.SetActive(true);
            }
            // NO KEYS PRESSED
            else
            {
                // reset progress and both radial fills
                currentTime = 0f;
                fillLeft.Play(0, 0, Mathf.Lerp(fillLeft.GetCurrentAnimatorStateInfo(0).normalizedTime, 0f, damping));
                fillRight.Play(0, 0, Mathf.Lerp(fillRight.GetCurrentAnimatorStateInfo(0).normalizedTime, 0f, damping));

                leftArrow.SetActive(false);
                rightArrow.SetActive(false);
            }

            // Fade in, out if transition due
            if ((!selectionUIVisible) && (selectionUI.alpha > 0f))
            {
                selectionUI.alpha -= Time.deltaTime;
            }

            if ((selectionUIVisible) && (selectionUI.alpha < 1f))
            {
                selectionUI.alpha += Time.deltaTime;
            }

            if ((!thankYouUIVisible) && (thankYouUI.alpha > 0f))
            {
                thankYouUI.alpha -= Time.deltaTime;
            }

            if ((thankYouUIVisible) && (thankYouUI.alpha < 1f))
            {
                thankYouUI.alpha += Time.deltaTime;
            }
        */
        } // MODE 1: Press and hold to confirm

        bool currentSeated = Input.GetKey(GlobalData.key_seat);

        bool didSeatedStateChanged = currentSeated != isSeated;

        if (didSeatedStateChanged)
        {
            seatedStateChangeTime = Time.time;
        }

        isSeated = currentSeated;

        if (isSeated)
        {
            screenSaver.Deactivate();
        }

        {
            if (MODE2_selected)
            {
                screenSaver.Deactivate();

                AutoFillSelection();
            }
            else
            {
                if ((Input.GetKeyDown(GlobalData.key_left)) && (!blockInput))
                {
                    if (screenSaver.isActive)
                    {
                        screenSaver.Deactivate();
                        seatedStateChangeTime = Time.time;

                        StartCoroutine(BlockBrakeInput());
                    }
                    else
                    {
                        GlobalData.language = 0;
                        MODE2_selected = true;
                    }
                }
                else if ((Input.GetKeyDown(GlobalData.key_right)) && (!blockInput))
                {
                    if (screenSaver.isActive)
                    {
                        screenSaver.Deactivate();
                        seatedStateChangeTime = Time.time;

                        StartCoroutine(BlockBrakeInput());
                    }
                    else
                    {
                        GlobalData.language = 1;
                        MODE2_selected = true;
                    }
                }

                if (!MODE2_selected && !blockInput && !isSeated)
                {
                    if (Time.time >= seatedStateChangeTime + screenSaverDelay)
                    {
                        screenSaver.Activate();
                    }
                }
            }
        } // MODE 2: Press once to confirm selection
    }

    // Only used for MODE 2
    void AutoFillSelection()
    {
        if (currentTime < delayTime)
        {
            if (GlobalData.language == 0)
            {
                fillLeft.Play(0, 0, currentTime / delayTime);
            }
            else
            {
                fillRight.Play(0, 0, currentTime / delayTime);
            }

            currentTime += Time.deltaTime;
        }
        else
        {
            MODE2_selected = false;
            SelectionConfirmed();            
        }
    }


    void SelectionConfirmed()
    {
        selectionUIVisible = false;
        blockInput = true;

        textThankYou1.InitLanguage();
        textThankYou2.InitLanguage();

        StartCoroutine(CueSceneChange());
        StartCoroutine(CueThankYouFadeIn());

        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }

    IEnumerator BlockBrakeInput()
    {
        blockInput = true;

        yield return new WaitForSeconds(brakeInputDelay);

        blockInput = false;        
    }

    IEnumerator CueThankYouFadeIn()
    {
        thankYouUIVisible = true;

        while (selectionUI.alpha > 0f)
        {
            selectionUI.alpha -= Time.deltaTime;

            yield return null;
        }
        
        yield return new WaitForSeconds(UIFadeTime);

        while (thankYouUI.alpha < 1f)
        {
            thankYouUI.alpha += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator CueSceneChange()
    {
        yield return new WaitForSeconds(confirmedScreenTime);

        //sceneTransition.enabled = true;

        sceneTransition.SetBool("out", true);
    }
}