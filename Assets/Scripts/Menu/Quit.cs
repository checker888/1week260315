using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Quit : MonoBehaviour
{
    public Button quittButtton;
    public Button quitConfirmButton;
    private MenuViewManager menuViewManager;
    private void Start()
    {
        menuViewManager = GetComponent<MenuViewManager>();
        quittButtton.onClick.AddListener(PushQuitButton);
        quitConfirmButton.onClick.AddListener(QuitGame);
    }

    public void PushQuitButton()
    {
        menuViewManager.SetMenu("Quit");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Title");
    }
}
