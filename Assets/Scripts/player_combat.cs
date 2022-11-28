using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_combat : MonoBehaviour
{

    public float playerHealth;

    public GameObject WeaponHolder;
    public GameObject guadana;
    Animator guadanaAnim;
    // Start is called before the first frame update
    void Start()
    {
        //guadana = WeaponHolder.gameObject.transform.GetChild(0).gameObject;
        guadana.SetActive(false);
        guadanaAnim = guadana.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth <= 0)
        {
            Debug.Log("Tas muelto pibe");
        }

        if (Input.GetMouseButtonDown(1))
        {
            guadana.SetActive(true);
            guadanaAnim.Play("MeleeGuadana");
            Invoke(nameof(MeleeReset), 1f);
        } 
    }

    private void MeleeReset()
    {
        guadana.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            playerHealth -= 10;
        }
    }

    public void healPlayer(float healthChange)
    {
        playerHealth += healthChange;
    }
}
