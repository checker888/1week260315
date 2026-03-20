using UnityEngine;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    public GameObject lightObj;
    public float rayDistance = 40f;
    public Transform cameraPivot;
    [Header("Light Settings")]
    public float minRadius = 1f;
    public float maxRadius = 2.5f;
    public float maxDistance = 50f;
    public bool isShining = false;

    public bool isDynamic = false;
    // 今フレームで消えてるブロック
    private List<GameObject> currentBlocks = new List<GameObject>();
    void Start()
    {
        //if (isDynamic)
        //{
        //    InvokeRepeating(nameof(UpdateLight), 0f, 0.08f);
        //}else if(isShining)
        //{
        //    UpdateLight();
        //}
        InvokeRepeating(nameof(UpdateLight), 0f, 0.08f);
    }
    public void UpdateLight()
    {
        List<GameObject> workBlocks = new List<GameObject>();
        // ① 前フレームのを戻す
        foreach (var block in currentBlocks)
        {
            if (block != null || !isShining)
            {
                Collider col = block.GetComponent<Collider>();
                if (col != null)
                {
                    if (isDynamic)
                    {
                        Collider[] hitsc = Physics.OverlapBox(
                            col.bounds.center,
                            col.bounds.extents,
                            block.transform.rotation
                        );

                        bool isTouchingPlayer = false;

                        foreach (var h in hitsc)
                        {
                            if (h.CompareTag("Player"))
                            {
                                isTouchingPlayer = true;
                                break;
                            }
                        }

                        if (isTouchingPlayer)
                        {
                            // 接触してるなら戻さない
                            workBlocks.Add(block);
                        }
                        else
                        {
                            block.SetActive(true);
                        }
                    }
                    else
                    {
                        block.SetActive(true);
                    }
                    // Playerとの接触チェック


                }
            }
        }
        currentBlocks.Clear();
        foreach (var block in workBlocks)
        {
            currentBlocks.Add(block);
        }
        if (!isShining)
        {

            return;
        }

        //ライト中心を取得
        int layerMask = ~LayerMask.GetMask("Player");
        RaycastHit[] hits = Physics.RaycastAll(
            cameraPivot.position,
            cameraPivot.forward,
            rayDistance,
            layerMask
        );
        if (hits.Length > 0)
        {
            // 距離順に並べる（重要）
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                GameObject obj = hit.collider.gameObject;

                // Erasableでなければ遮蔽物として終了
                if (!hit.collider.CompareTag("Erasable"))
                {
                    break;
                }

                // OverlapSphereで周囲を消す
                float dynamicRadius = Mathf.Lerp(minRadius, maxRadius, hit.distance / maxDistance);
                Collider[] cols = Physics.OverlapSphere(hit.point, dynamicRadius);
                foreach (var col in cols)
                {
                    if (col.CompareTag("Erasable"))
                    {
                        GameObject block = col.gameObject;
                        if (block.activeSelf)
                        {
                            block.SetActive(false);
                            currentBlocks.Add(block);
                        }
                    }
                }
            }
        }

    }
}