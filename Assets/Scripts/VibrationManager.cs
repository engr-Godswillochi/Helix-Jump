using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Light vibration for passing through levels
    public static void VibrateLight()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidVibrate(20); // Very light 20ms vibration
        }
        else
        {
            Handheld.Vibrate();
        }
#endif
    }

    // Medium vibration for perfect passes or special events
    public static void VibrateMedium()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidVibrate(50); // 50ms vibration
        }
        else
        {
            Handheld.Vibrate();
        }
#endif
    }

    // Heavy vibration for game over or breaking platforms
    public static void VibrateHeavy()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidVibrate(200); // 200ms vibration
        }
        else
        {
            Handheld.Vibrate();
        }
#endif
    }

#if UNITY_ANDROID
    private static void AndroidVibrate(long milliseconds)
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
                {
                    vibrator.Call("vibrate", milliseconds);
                }
            }
        }
    }
#endif
}