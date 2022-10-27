using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_cam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    float yRotation;
    float xRotation;
    float zRotation;
    float xRotationOr;

    public Transform orientation;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //transform.rotation = Quaternion.Euler(xRotation, yRotation, transform.rotation.z);
        //orientation.rotation = Quaternion.Euler(orientation.rotation.x, yRotation, orientation.rotation.z);
        //transform.rotation = Quaternion.Euler(transform.rotation.x + xRotation, transform.rotation.y + yRotation, transform.rotation.z);
        //orientation.rotation = Quaternion.Euler(orientation.rotation.x, orientation.rotation.y + yRotation, orientation.rotation.z);
        transform.rotation = Quaternion.AngleAxis(yRotation, orientation.transform.up);
        transform.rotation = Quaternion.AngleAxis(xRotation, orientation.transform.right);
    }
}
