using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartManager : MonoBehaviour
{
    public Button restartButtton;
    public Button restartConfirmButton;
    private MenuViewManager menuViewManager;
    private void Start()
    {
        menuViewManager = GetComponent<MenuViewManager>();
        restartButtton.onClick.AddListener(PushRestartButton);
        restartConfirmButton.onClick.AddListener(RestartGame);
    }

    public void PushRestartButton()
    {
        menuViewManager.SetMenu("Restart");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
