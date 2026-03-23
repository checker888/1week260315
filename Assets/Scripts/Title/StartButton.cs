using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button startButton;

    public void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }


    void StartGame()
    {
        PlayerPrefs.SetInt("NowCheckPoint", 0);
        SceneManager.LoadScene("Main");

    }
}
