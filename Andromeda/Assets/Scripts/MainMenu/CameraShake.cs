using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera cam;
    private float shakeTimer;
    private float defaultAmp;

    public bool keepPos = false;
    public bool fade = false;
    private Vector3 originalPosition;

    private void Awake()
    {
        Instance = this;
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        defaultAmp = perlin.m_AmplitudeGain;
        if (keepPos)
        {
            originalPosition = Camera.main.gameObject.transform.localPosition;
        }
    }

    public void shake(float strength, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = strength;
        shakeTimer = time;
    }

    private void Update()
    {
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if(fade)
            perlin.m_AmplitudeGain -= perlin.m_AmplitudeGain * Time.deltaTime;
        shakeTimer -= Time.deltaTime;
        

        if (shakeTimer <= 0f)
        {
            perlin.m_AmplitudeGain = defaultAmp;
            if (keepPos)
            {
                Camera.main.gameObject.transform.localPosition = originalPosition;
            }
        }

        
    }
}
