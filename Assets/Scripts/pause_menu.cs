using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class pause_menu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject controls;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject options;
    [SerializeField] TextMeshProUGUI volumeValue;
    [SerializeField] TextMeshProUGUI sensValueX;
    [SerializeField] TextMeshProUGUI sensValueY;
    bool isPaused;
    [SerializeField] public Texture2D cursor;
    [SerializeField] public AudioMixer mainMixer;
    [SerializeField] public GameObject menuAudio;
    [Header("Sliders")]
    [SerializeField] public Slider sliderVolume;
    [SerializeField] public Slider sliderX;
    [SerializeField] public Slider sliderY;
    // Start is called before the first frame update
    void Start()
    {
        sliderVolume.onValueChanged.AddListener(
            (newVolume) =>
            {
                volumeValue.text = (newVolume+100).ToString("0");
            }
        );
        sliderX.onValueChanged.AddListener(
            (newSensX) =>
            {
                sensValueX.text = newSensX.ToString("0");
            }
        );
        sliderY.onValueChanged.AddListener(
            (newSensY) =>
            {
                sensValueY.text = newSensY.ToString("0");
            }
        );
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
                CloseOptions();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        if(isPaused && Input.GetMouseButtonDown(0))
        {
            controls.SetActive(false);
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

    public void OpenControls()
    {
        controls.SetActive(true);
    }

    public void HoverButton()
    {
        menuAudio.GetComponents<AudioSource>()[0].Play();
    }

    public void ClickButton()
    {
        menuAudio.GetComponents<AudioSource>()[1].Play();
    }


    //OPTIONS METHODS
    public void SetVolume (float volume)
    {
        mainMixer.SetFloat("MainVolume",volume);
    }

}
