using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDeath : MonoBehaviour
{
    public GameObject panel;

    public void Show_Panel ()
    {
        panel.SetActive(true);
    }

    public void Game_start()
    {
        SceneManager.LoadScene("Assets/Scenes/SampleScene.unity");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Assets/Scenes/Main Menu.unity");
    }
}
