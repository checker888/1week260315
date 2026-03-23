using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BackToTitle : MonoBehaviour
{
    public Button quitConfirmButton;
    private void Start()
    {
        quitConfirmButton.onClick.AddListener(PushBackToTitle);
    }

    public void PushBackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
