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
    bool GSready;

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
        if (!Input.GetKey(KeyCode.Q) || !GSready)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            orientation.transform.Rotate(new Vector3(0, mouseX, 0));
            transform.rotation = orientation.rotation;
            transform.Rotate(new Vector3(xRotation, 0, 0));
        }
        else if(Input.GetKey(KeyCode.Q) && GSready)
        {
            //Transform anterior = transform;
            GSready = false;
            Invoke(nameof(GSreset), 0.5f);
        }

    }

    private void GSreset()
    {
        GSready = true;
    }

}
