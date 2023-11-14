using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float minX = -60f;
    public float maxX = 60f;

    public float sensitivity;
    private Camera cam;

    float rotY = 0f;
    float rotX = 0f;

    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        rotY += Input.GetAxis("Mouse X") * sensitivity;
        rotX += Input.GetAxis("Mouse Y") * sensitivity;

        rotX = Mathf.Clamp(rotX, minX, maxX);

        if (Cursor.visible && Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void FixedUpdate()
    {
        if (Time.timeScale == 1)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotY, transform.localEulerAngles.z);
            cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
        }

    }
}
