using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_manager : MonoBehaviour
{
    public GameObject CanvasCreditos;
    public GameObject CanvasPrincipal;

    public void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasCreditos.SetActive(false);
            CanvasPrincipal.SetActive(true);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Nivel 2");
    }

    public void Creditos()
    {
        CanvasPrincipal.SetActive(false);
        CanvasCreditos.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
