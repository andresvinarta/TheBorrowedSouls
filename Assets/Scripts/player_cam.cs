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
    bool Gswaped;
    bool rotando;
    bool test;
    Vector3 lookatPoint;
    public float lookDistance;
    public float duracion;

    public Transform orientation;
    public Transform punto;


    // Start is called before the first frame update
    void Start()
    {
        GSready = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.Q) || !GSready)
        {
            if (!rotando)
            {
                float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
                float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
                
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                yRotation += mouseX; 

                orientation.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
                transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
            }

            if (Gswaped || rotando)
            {
                transform.LookAt(lookatPoint, orientation.transform.up);
                Debug.Log("xRotation antes: " + xRotation);
                xRotation = transform.localEulerAngles.x;
                Debug.Log("xRotation despues: " + xRotation);
                if (xRotation >= 360f)
                {
                    xRotation -= 360f;
                }
                else if (xRotation <= -360f)
                {
                    xRotation += 360f;
                }
                Debug.Log("xRotation despues: " + xRotation);
                yRotation = transform.localEulerAngles.y;
                if(xRotation > 90f || xRotation < -90f)
                { 
                    //transform.Rotate(0,0,180f);
                    //zRotation = transform.localEulerAngles.z;
                }
                if (!rotando)
                {
                    rotando = true;
                    Gswaped = false;
                }
            }
        }
        else if(Input.GetKey(KeyCode.Q) && GSready)
        {
            GSready = false;
            lookatPoint = transform.position + transform.forward * lookDistance;
            punto.position = lookatPoint;
            Gswaped = true;
            Invoke(nameof(GSreset), duracion);
        }

    }

    private void GSreset()
    {
        GSready = true;
        rotando = false;
        test = false;
    }

}
