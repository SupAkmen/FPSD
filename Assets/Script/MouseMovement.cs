using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    float xRotation;
    float yRotation;

    public float topClamp = -90f;
    public float botClamp = 90f;
    private void Start()
    {
        // khoa con tro vao giua man hinh vaf lam no tang hinh
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Lay dau vao cu chuot 

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotaion around the x axis ( look up and down )

        xRotation -= mouseY;

        // clamp the rotation ( tam nhin chi trong khoang truoc mat ) 

        xRotation = Mathf.Clamp(xRotation, topClamp, botClamp);

        // Rotation around the y axis ( look left and right)

        yRotation += mouseX;

        // apply rotation to our transform

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
