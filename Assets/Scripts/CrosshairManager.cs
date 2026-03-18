using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public GameObject crosshairObj;
    private int activeState = 1;
    void Start()
    {
        activeState = PlayerPrefs.GetInt("Crosshair", 1);
        SetState(activeState);
    }

    // Update is called once per frame
    public void SetState(int state)
    {
        if(state == 0)
        {
            crosshairObj.SetActive(false);
        }
        else
        {
            crosshairObj.SetActive(true);
        }
    }
}
