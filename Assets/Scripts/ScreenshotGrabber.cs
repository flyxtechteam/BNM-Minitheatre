using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotGrabber : MonoBehaviour {

    int screenshotCounter = 0;

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ScreenCapture.CaptureScreenshot("Screenshot_" + screenshotCounter.ToString() + ".png");

            screenshotCounter++;
        }
	}
}
