using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass : MonoBehaviour
{

    public GameObject player;

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z);
    }
}
