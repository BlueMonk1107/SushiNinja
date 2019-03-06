using UnityEngine;
using System.Collections;

public class Resolution
{
    private static int scaleWidth = 0;
    private static int scaleHeight = 0;

    public static void setDesignContentScale()
    {
#if UNITY_ANDROID
        if (scaleWidth == 0 && scaleHeight == 0)
        {
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;
            int designWidth = 480;
            int designHeight = 800;
            float s1 = (float)designWidth / (float)designHeight;
            float s2 = (float)width / (float)height;
            if (s1 < s2)
            {
                designWidth = (int)Mathf.FloorToInt(designHeight * s2);
            }
            else if (s1 > s2)
            {
                designHeight = (int)Mathf.FloorToInt(designWidth / s2);
            }
            float contentScale = (float)designWidth / (float)width;
            if (contentScale < 1.0f)
            {
                scaleWidth = designWidth;
                scaleHeight = designHeight;
            }
        }
        if (scaleWidth > 0 && scaleHeight > 0)
        {
            if (scaleWidth % 2 == 0)
            {
                scaleWidth += 1;
            }
            else {
                scaleWidth -= 1;
            }
            Screen.SetResolution(scaleWidth, scaleHeight, true);
        }
#endif
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
        }
        else {
            setDesignContentScale();
        }
    }
}
