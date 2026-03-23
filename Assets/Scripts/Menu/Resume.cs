using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resume : MonoBehaviour
{
    public Button resumeButton;
    private MenuViewManager menuViewManager;
    // Start is called before the first frame update
    void Start()
    {
        menuViewManager = GetComponent<MenuViewManager>();
        resumeButton.onClick.AddListener(ResumeGame);
    }

    // Update is called once per frame
    void ResumeGame()
    {
        menuViewManager.SetMenu("");
    }
}
