using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resume : MonoBehaviour
{
    public Button resumeButtton;
    private MenuViewManager menuViewManager;
    // Start is called before the first frame update
    void Start()
    {
        menuViewManager = GetComponent<MenuViewManager>();
        resumeButtton.onClick.AddListener(ResumeGame);
    }

    // Update is called once per frame
    void ResumeGame()
    {
        menuViewManager.SetMenu("");
    }
}
