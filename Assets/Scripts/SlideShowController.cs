using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShowController : MonoBehaviour {

    [SerializeField]
    private CanvasGroup self;

    [SerializeField]
    private CanvasGroup [] slides;

    [SerializeField]
    private float durationPerSlide = 3f;

    [SerializeField]
    private float crossfadeDuration = 1f;

    private bool isActive = false;
    private int activeSlideIndex = 0;
    private float lastSlideChangeTime = 0f;

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

            if (Time.time >= lastSlideChangeTime + durationPerSlide + crossfadeDuration)
            {
                activeSlideIndex = (activeSlideIndex + 1) % slides.Length;
                lastSlideChangeTime = Time.time;
            }

            if (Time.time <= lastSlideChangeTime + crossfadeDuration)
            {
                CanvasGroup activeSlide = slides[activeSlideIndex];
                CanvasGroup lastSlide = slides[((activeSlideIndex + slides.Length) - 1) % slides.Length];

                activeSlide.alpha += Time.deltaTime * crossfadeDuration;
                lastSlide.alpha -= Time.deltaTime * crossfadeDuration;
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
        isActive = false;

        if (slides != null)
        {
            for (int i = 0; i < slides.Length; i++)
            {
                CanvasGroup slide = slides[i];

                slide.alpha = 0;
            }
        }
    }
}
