using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_cam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    float yRotation;
    float xRotation;
    bool GSready;
    bool Gswaped;
    bool rotando;
    Vector3 lookatPoint;
    public float lookDistance;
    public float duracion;

    public Transform orientation;


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
        //Intento de dejar la cámara fija durante la rotación
        if ((Input.GetKey(KeyCode.Q) ||
             Input.GetKey(KeyCode.E) ||
             Input.GetKey(KeyCode.F) ||
             Input.GetKey(KeyCode.C))
             && GSready)
        {
            GSready = false;
            lookatPoint = transform.position + transform.forward * lookDistance;
            Gswaped = true;
            Invoke(nameof(GSreset), duracion);
        }


        if (Gswaped || rotando)
        {
            transform.LookAt(lookatPoint, orientation.transform.up);
            if (transform.localEulerAngles.x > 90f || transform.localEulerAngles.x < -90f)
            {
                orientation.Rotate(0f, 180f, 0f);
                transform.Rotate(0f, 180f, 0f);
                transform.LookAt(lookatPoint, orientation.transform.up);
            }
            xRotation = transform.localEulerAngles.x;
            yRotation = transform.localEulerAngles.y;
            if (!rotando)
            {
                rotando = true;
                Gswaped = false;
            }
        }
        //Fin del intento

        if (!rotando)
        {
            //Código real y funcional de la cámara
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
                
            xRotation -= mouseY;
            Debug.Log(xRotation);
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation += mouseX;

            orientation.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        }

    }

    private void GSreset()
    {
        GSready = true;
        rotando = false;
    }

}
