using UnityEngine;
using System.Collections.Generic;

public class SwitchBlockController : MonoBehaviour
{
    [Header("光を検知しているか")]
    public bool isLit = false;

    [Header("制御するオブジェクト（複数）")]
    public List<GameObject> targetObjects = new List<GameObject>();

    [Header("マテリアル")]
    public Renderer rend;
    public Color offColor = Color.red;
    public Color onColor = Color.green;

    private void Start()
    {
        UpdateBlockState();
    }

    public void UpdateBlockState()
    {
        if (isLit)
        {
            // ON状態
            foreach (var obj in targetObjects)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

            if (rend != null)
                rend.material.color = onColor;
        }
        else
        {
            // OFF状態
            foreach (var obj in targetObjects)
            {
                if (obj != null)
                    obj.SetActive(true);
            }

            if (rend != null)
                rend.material.color = offColor;
        }
    }
}