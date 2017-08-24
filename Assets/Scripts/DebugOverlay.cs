using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text indicator_leftKey, indicator_rightKey, indicator_cycleKey, indicator_seatKey, indicator_info;

    Dictionary<UnityEngine.UI.Text, KeyCode> inputPairs;

    [SerializeField]
    UnityEngine.Playables.PlayableDirector timeline;

    [SerializeField]
    PlaybackControl playback;

    private void Start()
    {
        inputPairs = new Dictionary<UnityEngine.UI.Text, KeyCode>
        {
            {indicator_leftKey, GlobalData.key_left},
            {indicator_rightKey, GlobalData.key_right},
            {indicator_cycleKey, GlobalData.key_cycle},
            {indicator_seatKey, GlobalData.key_seat}
        };
    }
    void Update()
    {
        // Light input indicators if corresponding button is pressed
        foreach (KeyValuePair<UnityEngine.UI.Text, KeyCode> pair in inputPairs)
        {
            if (Input.GetKey(pair.Value))
            {
                pair.Key.color = Color.green;
            }
            else
            {
                pair.Key.color = Color.red;
            }
        }

        // Update other indicators
        indicator_info.text = "FPS: " + (1f / Time.deltaTime).ToString("n0") + "\nPlaytime: " + timeline.time.ToString("n2") + " / " + timeline.duration.ToString("n2") + "\nPause Timeout: " + playback.timeLeft.ToString("n2");
    }
}
