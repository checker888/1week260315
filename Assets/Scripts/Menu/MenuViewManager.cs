using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuViewManager : MonoBehaviour
{
    public List<GameObject> menuList;
    private bool isActive;
    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }
    public string nowActiveMenu = "";
    //対象のオブジェクトのみ表示し、対象以外のオブジェクトは非表示にする
    public void SetMenu(string name)
    {


        for (int i = 0; i < menuList.Count; i++)
        {
            string s = menuList[i].name;
            menuList[i].SetActive(s == name);
            if(s==name)
            {
                nowActiveMenu = name;
                isActive = true;
                break;
            }
        }
        if (name == "")
        {
            isActive = false;
            nowActiveMenu = "";
        }
    }

    public void SetMenu(GameObject gameObject)
    {

        //対象のオブジェクトのみ表示し、対象以外のオブジェクトは非表示にする
        foreach (GameObject g in menuList)
        {
            g.SetActive(g == gameObject);
            if (g == gameObject)
            {
                nowActiveMenu = gameObject.name;
                isActive = true;
                break;
            }
        }
        if (gameObject == null)
        {
            isActive = false;
            nowActiveMenu = "";
        }
    }



}