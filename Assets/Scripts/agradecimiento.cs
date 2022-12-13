using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class agradecimiento : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DemoReset), 10f);
    }

    private void Update()
    {
        if (
            Input.GetKeyDown(KeyCode.Escape) || 
            Input.GetKeyDown(KeyCode.Space) || 
            Input.GetKeyDown(KeyCode.Return) || 
            Input.GetMouseButtonDown(0) || 
            Input.GetMouseButtonDown(1)
            )
        {
            DemoReset();
        }
    }

    private void DemoReset()
    {
        SceneManager.LoadScene("Splash");
    }
}
