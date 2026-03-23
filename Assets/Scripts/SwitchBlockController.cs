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

    // 追加：どのライトが当たっているか管理
    private HashSet<LightController> lights = new HashSet<LightController>();

    private void Start()
    {
        UpdateBlockState();
    }

    // ライトが当たったとき
    public void OnLightEnter(LightController lc)
    {
        if (lights.Add(lc))
        {
            ApplyState();
        }
    }

    // ライトが外れたとき
    public void OnLightExit(LightController lc)
    {
        if (lights.Remove(lc))
        {
            ApplyState();
        }
    }

    void ApplyState()
    {
        bool isLitNow = lights.Count > 0;

        if (isLit != isLitNow)
        {
            isLit = isLitNow;
            UpdateBlockState();
        }
    }

    public void UpdateBlockState()
    {
        if (isLit)
        {
            // ON状態
            foreach (var obj in targetObjects)
            {
                if (obj != null)
                {
                    BarrierController bc = obj.GetComponent<BarrierController>();
                    bc.PlayDisableSound();
                    obj.SetActive(false);
                }
                    
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