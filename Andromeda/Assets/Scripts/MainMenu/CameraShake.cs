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

    private void Awake()
    {
        Instance = this;
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        defaultAmp = perlin.m_AmplitudeGain;
    }

    public void shake(float strength, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = strength;
        shakeTimer = time;
    }

    private void Update()
    {
        shakeTimer -= Time.deltaTime;

        if(shakeTimer <= 0f)
        {
            CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            perlin.m_AmplitudeGain = defaultAmp;
        }
    }
}
