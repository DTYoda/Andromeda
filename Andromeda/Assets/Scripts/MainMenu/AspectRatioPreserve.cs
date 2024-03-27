using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioPreserve : MonoBehaviour
{

    private void Start()
    {
        Adjust();
    }

    //adjusts the screen when the window is not 16:9
    public void Adjust()
    {
        float targetAspect = 16.0f / 9.0f;

        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = GetComponent<Camera>();

        if(scaleHeight < 1)
        {
            Rect rect = camera.rect;

            rect.width = 1;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1 - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1;
            rect.x = (1 - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
