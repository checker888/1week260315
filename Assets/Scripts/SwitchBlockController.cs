using UnityEngine;

public class SwitchBlockController : MonoBehaviour
{
    [Header("光を検知しているか")]
    public bool isLit = false;

    [Header("制御するオブジェクト")]
    public GameObject targetObject;

    [Header("マテリアル")]
    public Renderer rend;
    public Color offColor = Color.red;
    public Color onColor = Color.green;

    void Update()
    {
        if (isLit)
        {
            // ON状態
            if (targetObject != null)
                targetObject.SetActive(false);

            if (rend != null)
                rend.material.color = onColor;
        }
        else
        {
            // OFF状態
            if (targetObject != null)
                targetObject.SetActive(true);

            if (rend != null)
                rend.material.color = offColor;
        }

        // 毎フレームリセット（光検知側でtrueにする想定）
        isLit = false;
    }
}