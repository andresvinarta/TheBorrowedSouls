using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_manager : MonoBehaviour
{
    public GameObject CanvasCreditos;
    public GameObject CanvasControles;
    public GameObject CanvasPrincipal;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasPrincipal.SetActive(true);
            CanvasCreditos.SetActive(false);
            CanvasControles.SetActive(false);
            
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Nivel 2");
    }

    public void Creditos()
    {
        CanvasPrincipal.SetActive(false);
        CanvasControles.SetActive(false);
        CanvasCreditos.SetActive(true);
    }

    public void Controles()
    {
        CanvasPrincipal.SetActive(false);
        CanvasCreditos.SetActive(false);
        CanvasControles.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
