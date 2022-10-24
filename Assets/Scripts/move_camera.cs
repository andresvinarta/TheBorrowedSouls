using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_camera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cameraPosition;


    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
        transform.rotation = cameraPosition.rotation;
    }
}
