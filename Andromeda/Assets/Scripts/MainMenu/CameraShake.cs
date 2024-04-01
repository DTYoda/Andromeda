using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera cam;
    public float shakeTimer;
    private float defaultAmp;

    public bool keepPos = false;
    public bool fade = false;
    public bool changeFOV;
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
        shakeTimer = time;
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = strength;
    }

    private void Update()
    {
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (fade)
            perlin.m_AmplitudeGain -= perlin.m_AmplitudeGain * Time.deltaTime;
        
        if (shakeTimer <= 0f)
        {
            shakeTimer = 0;
            perlin.m_AmplitudeGain = defaultAmp;
            if (keepPos)
            {
                Camera.main.gameObject.transform.localPosition = originalPosition;
            }
            GetComponent<CinemachineBrain>().enabled = false;
            
        }
        else
        {
            shakeTimer -= Time.deltaTime;
            GetComponent<CinemachineBrain>().enabled = true;
        }

        if(changeFOV)
        {
            cam.m_Lens.FieldOfView = Camera.main.fieldOfView;
        }
        
    }
}
