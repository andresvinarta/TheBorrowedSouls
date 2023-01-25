using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sala_principal_nv2 : MonoBehaviour
{

    public GameObject altar1, altar2, altar3, altar4;

    public GameObject orb;

    public GameObject player;

    public GameObject respawnPoint;

    GameObject[] alatares;

    int numAltares;

    bool playerInside, levelComplete;

    // Start is called before the first frame update
    void Start()
    {
        alatares = new GameObject[] {altar1, altar2, altar3, altar4};
        numAltares = alatares.Length;
        playerInside = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(numAltares <= 0 && !levelComplete)
        {
            this.gameObject.SetActive(false);
            orb.SetActive(true);
            levelComplete = true;
            Debug.Log("Nivel completado");
        }
    }

    public void AltarCompleted()
    {
        numAltares--;
    }

    public void playerEntered()
    {
        if (!playerInside && !levelComplete)
        {
            this.gameObject.SetActive(true);
            player.GetComponent<player_combat>().RespawnChange(respawnPoint);
        }
        else if (levelComplete)
        {
            SceneManager.LoadScene("Agradecimiento Demo");
        }
    }
}
