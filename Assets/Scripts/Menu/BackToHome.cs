using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackToHome : MonoBehaviour
{
    public List<Button> backToHomeButtton;
    private MenuViewManager menuViewManager;
    // Start is called before the first frame update
    void Start()
    {
        menuViewManager = GetComponent<MenuViewManager>();
        foreach (var button  in backToHomeButtton)
        {
            button.onClick.AddListener(BackHome);
        }
        
    }

    // Update is called once per frame
    void BackHome()
    {
        menuViewManager.SetMenu("Home");
    }
}
