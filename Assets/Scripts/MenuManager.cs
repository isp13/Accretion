using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public Button startGameButton;
    // Start is called before the first frame update
    void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        GameObject.Find("Music").GetComponent<AudioManager>().PlayMusic();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        Debug.Log("CLICK");
        SceneManager.LoadScene("Assets/Scenes/SampleScene.unity");
    }
}
