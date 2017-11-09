using UnityEngine;

public static class GlobalData
{
    // Game Controls
    public static KeyCode key_cycle = KeyCode.W;
    public static KeyCode key_left = KeyCode.A;
    public static KeyCode key_right = KeyCode.D;
    public static KeyCode key_seat = KeyCode.S;

    public static KeyCode key_toggleAutoCycle = KeyCode.Backslash;

    // 0 - English, 1 - Malay
    public static int language = 0;

    public static bool didTimeOut = false;
}
