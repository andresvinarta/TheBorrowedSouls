using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entry_check : MonoBehaviour
{

    public GameObject altar;

    public GameObject respawnPoint;

    public GameObject player;

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
            Invoke(nameof(AlertAltar), 0.5f);
            player.GetComponent<player_combat>().RespawnChange(respawnPoint);
        }
    }

    void AlertAltar()
    {
        altar.gameObject.GetComponent<altar_nv2>().playerEntered();
    }
}
