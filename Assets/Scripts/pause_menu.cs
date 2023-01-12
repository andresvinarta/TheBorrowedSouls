using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class pause_menu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject options;
    bool isPaused;
    [SerializeField] public Texture2D cursor;
    [SerializeField] public AudioMixer mainMixer;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(isPaused);
        HUD.SetActive(!isPaused);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            pauseMenu.SetActive(isPaused);
            HUD.SetActive(!isPaused);
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public bool GetIsPaused() { return isPaused; }

    public void ExitGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void OpenOptions()
    {
        buttons.SetActive(false);
        options.SetActive(true);
    }

    public void CloseOptions()
    {
        buttons.SetActive(true);
        options.SetActive(false);
    }


    //OPTIONS METHODS
    public void SetVolume (float volume)
    {
        mainMixer.SetFloat("MainVolume",volume);
    }

}
