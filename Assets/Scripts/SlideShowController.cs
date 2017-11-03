using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShowController : MonoBehaviour {

    [SerializeField]
    private CanvasGroup self;

    [Header("Slide Properties")]

    [SerializeField]
    private CanvasGroup [] slides;

    [SerializeField]
    private float durationPerSlide = 3f;

    [SerializeField]
    private float slideCrossfadeDuration = 1f;

    [Header("Instructions Properties")]

    [SerializeField]
    private CanvasGroup[] instructions;

    [SerializeField]
    private float durationPerInstruction;

    [SerializeField]
    private float instructionFadeDuration = 1f;

    public bool isActive { private set; get; }

    private int activeSlideIndex = 0;
    private float lastSlideChangeTime = 0f;

    private int activeInstructionIndex = 0;
    private float lastInstructionChangeTime = 0f;

    public void Activate(bool _isInstant = false)
    {
        if (isActive)
        {
            return;
        }

        Reset();

        isActive = true;

        if (_isInstant)
        {
            self.alpha = 1f;
        }
    }

    public void Deactivate(bool _isInstant = false)
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;

        if (_isInstant)
        {
            self.alpha = 0f;
        }
    }

	private void Update ()
    {
        if (isActive)
        {
            self.alpha += Time.deltaTime;

            //slides
            if (Time.time >= lastSlideChangeTime + durationPerSlide + slideCrossfadeDuration)
            {
                activeSlideIndex = (activeSlideIndex + 1) % slides.Length;
                lastSlideChangeTime = Time.time;
            }

            if (Time.time <= lastSlideChangeTime + slideCrossfadeDuration)
            {
                CanvasGroup activeSlide = slides[activeSlideIndex];
                CanvasGroup lastSlide = slides[((activeSlideIndex + slides.Length) - 1) % slides.Length];

                activeSlide.alpha += Time.deltaTime * 1f / slideCrossfadeDuration;
                lastSlide.alpha -= Time.deltaTime * 1f / slideCrossfadeDuration;
            }

            //instructions
            if (Time.time >= lastInstructionChangeTime + durationPerInstruction + instructionFadeDuration * 2)
            {
                activeInstructionIndex = (activeInstructionIndex + 1) % instructions.Length;
                lastInstructionChangeTime = Time.time;
            }

            if (Time.time <= lastInstructionChangeTime + instructionFadeDuration)
            {
                CanvasGroup activeInstruction = instructions[activeInstructionIndex];

                activeInstruction.alpha += Time.deltaTime * 1f / instructionFadeDuration;
            }

            else if (Time.time >= lastInstructionChangeTime + durationPerInstruction)
            {
                CanvasGroup activeInstruction = instructions[activeInstructionIndex];

                activeInstruction.alpha -= Time.deltaTime * 1f / instructionFadeDuration;
            }
        }

        else
        {
            self.alpha -= Time.deltaTime;
        }
	}

    private void Reset()
    {
        self.alpha = 0f;

        activeSlideIndex = 0;
        lastSlideChangeTime = Time.time;
        lastInstructionChangeTime = Time.time;
        isActive = false;

        if (slides != null)
        {
            for (int i = 0; i < slides.Length; i++)
            {
                CanvasGroup slide = slides[i];

                slide.alpha = 0;
            }
        }

        if (instructions != null)
        {
            for (int i = 0; i < instructions.Length; i++)
            {
                CanvasGroup instruction = instructions[i];

                instruction.alpha = 0;
            }
        }
    }
}
