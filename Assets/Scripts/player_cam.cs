using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_cam : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float sensX;
    public float sensY;
    public Slider sliderX;
    public Slider sliderY;

    float yRotation;
    float xRotation;
    bool GSready;
    bool Gswaped;
    bool rotando;
    Vector3 lookatPoint;
    public float lookDistance;
    public float duracion;

    public Transform orientation;

    [Header("Pause Menu")]
    pause_menu pauseMenu;

    [SerializeField]
    private StatsMenu StatsMenu;

    // Start is called before the first frame update
    void Start()
    {
        sliderX.onValueChanged.AddListener(
            (newSensX) =>
            {
                sensX = newSensX;
            }
        );
        sliderY.onValueChanged.AddListener(
            (newSensY) =>
            {
                sensY = newSensY;
            }
        );
        pauseMenu = FindObjectOfType<pause_menu>();
        //StatsMenu = FindObjectOfType<StatsMenu>();
        GSready = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (pauseMenu.GetIsPaused() || StatsMenu.AreStatsShowing()) { return; }

        //Intento de dejar la cámara fija durante la rotación
        //if ((Input.GetKey(KeyCode.Q) ||
        //     Input.GetKey(KeyCode.E) ||
        //     Input.GetKey(KeyCode.F) ||
        //     Input.GetKey(KeyCode.C))
        //     && GSready)
        //{
        //    GSready = false;
        //    lookatPoint = transform.position + transform.forward * lookDistance;
        //    Gswaped = true;
        //    Invoke(nameof(GSreset), duracion);
        //}


        //if (Gswaped || rotando)
        //{
        //    transform.LookAt(lookatPoint, orientation.transform.up);
        //    if (transform.localEulerAngles.x > 90f || transform.localEulerAngles.x < -90f)
        //    {
        //        orientation.Rotate(0f, 180f, 0f);
        //        transform.Rotate(0f, 180f, 0f);
        //        transform.LookAt(lookatPoint, orientation.transform.up);
        //    }
        //    xRotation = transform.localEulerAngles.x;
        //    yRotation = transform.localEulerAngles.y;
        //    if (!rotando)
        //    {
        //        rotando = true;
        //        Gswaped = false;
        //    }
        //}
        //Fin del intento

        if (!rotando)
        {
            //Código real y funcional de la cámara
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
                
            xRotation -= mouseY;
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
