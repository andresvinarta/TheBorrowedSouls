using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sala_principal_nv2 : MonoBehaviour
{

    public GameObject altar1, altar2, altar3, altar4;

    public GameObject orb;

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
        if (!playerInside)
        {
            this.gameObject.SetActive(true);
        }

        if (levelComplete)
        {
            SceneManager.LoadScene("Splash");
        }
    }
}
