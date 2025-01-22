using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPG : MonoBehaviour
{
    public LayerGenerator LG;

    // Start is called before the first frame update
    void Start()
    {
        LG.GenerateLayersAtPostition(Vector3.zero, 14, 1, 14, 14, 2, 2, 15, 1);
    }

    
}
