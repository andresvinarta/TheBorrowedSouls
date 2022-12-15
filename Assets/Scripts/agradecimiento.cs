using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class agradecimiento : MonoBehaviour
{

    public GameObject musicManager;
    public GameObject imagenAgradecimiento;

    float startTime;

    Color color;

    // Start is called before the first frame update
    void Start()
    {
        color = new Color(0,0,0);
        imagenAgradecimiento.GetComponent<RawImage>().color = color;
        startTime = Time.realtimeSinceStartup;
        Invoke(nameof(DemoReset), 12f);
        musicManager.GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup - startTime < 2f)
        {
            color.r += 0.01f;
            color.g += 0.01f;
            color.b += 0.01f;
            imagenAgradecimiento.GetComponent<RawImage>().color = color;
        }

        if (Time.realtimeSinceStartup - startTime > 10f)
        {
            color.r -= 0.01f;
            color.g -= 0.01f;
            color.b -= 0.01f;
            imagenAgradecimiento.GetComponent<RawImage>().color = color;
        }

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
