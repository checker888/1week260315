using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Continue : MonoBehaviour
{
    public GameObject continueButtonObj;
    private Button continueButton;

    public void Start()
    {
        continueButton = continueButtonObj.GetComponent<Button>();
        continueButton.onClick.AddListener(ContinueGame);
        if(PlayerPrefs.GetInt("NowCheckPoint", -1) == -1)
        {
            continueButtonObj.SetActive(false);

        }
        else
        {
            continueButtonObj.SetActive(true);
        }
    }


    void ContinueGame()
    {
        SceneManager.LoadScene("Main");

    }
}
