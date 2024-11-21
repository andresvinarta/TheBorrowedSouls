using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSoulsPortal : MonoBehaviour
{
    private InfiniteSoulsManager InfiniteSoulsManager;

    void Start()
    {
        InfiniteSoulsManager = FindObjectOfType<InfiniteSoulsManager>();
    }

    private void Awake()
    {
        InfiniteSoulsManager = FindObjectOfType<InfiniteSoulsManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InfiniteSoulsManager.StatsScreen();
        }
    }
}
