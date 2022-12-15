using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_manager : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject CanvasCreditos;
    public GameObject CanvasControles;
    public GameObject CanvasPrincipal;
    public Texture2D cursor;

    [Header("Sonidos")]
    public GameObject sonido;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    public void Update()
    {

        if( (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetMouseButtonDown(0) ||
            Input.GetMouseButtonDown(1)) && CanvasPrincipal.activeSelf == false)
        {
            CanvasPrincipal.SetActive(true);
            CanvasCreditos.SetActive(false);
            CanvasControles.SetActive(false);
            sonido.GetComponents<AudioSource>()[2].Play();
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

    public void HoverButton()
    {
        sonido.GetComponents<AudioSource>()[1].Play();
    }

    public void ClickButton()
    {
        sonido.GetComponents<AudioSource>()[2].Play();
    }
}
