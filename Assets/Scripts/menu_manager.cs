using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_manager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Testing Grounds");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
