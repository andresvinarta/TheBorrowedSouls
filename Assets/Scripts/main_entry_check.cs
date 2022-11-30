using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_entry_check : MonoBehaviour
{
    public GameObject MainCover;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Invoke(nameof(AlertMain), 0.5f);
        }
    }

    void AlertMain()
    {
        MainCover.gameObject.GetComponent<sala_principal_nv2>().playerEntered();
    }
}
