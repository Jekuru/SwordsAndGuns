using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playLocale()
    {
        SceneManager.LoadScene("MainTestingScene");
    }

    public void exitButton()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }
}
