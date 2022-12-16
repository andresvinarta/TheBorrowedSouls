using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Completed_Rooms : MonoBehaviour
{

    public GameObject[] altars;
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
        if(other.tag == "Player")
        {
            Invoke(nameof(CloseAll), 0.5f);
        }
    }

    public void CloseAll()
    {
        foreach (GameObject altar in altars)
        {
            altar.GetComponent<altar_nv2>().CloseRoom();
        }
    }
}
