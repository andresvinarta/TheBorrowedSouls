using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class cinematica : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(LoadMenu), ((float)GetComponent<VideoPlayer>().clip.length) + 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) ||
            Input.GetKey(KeyCode.Return) ||
            Input.GetKey(KeyCode.Space))
        {
            LoadMenu();
        }
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
