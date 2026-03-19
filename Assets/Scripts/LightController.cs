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

        // ① 前フレームのを戻す
        foreach (var block in currentBlocks)
        {
            if (block != null || !isShining)
                block.SetActive(true);
        }
        currentBlocks.Clear();

        if (!isShining) {
            
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

                
                
                

                float distance = hit.distance;
                float dynamicRadius = Mathf.Lerp(minRadius, maxRadius, distance / maxDistance);

                Collider[] cols = Physics.OverlapSphere(hit.point, dynamicRadius);
                bool isExistErasable = false;
                foreach (var col in cols)
                {
                    if (col.CompareTag("Erasable"))
                    {
                        isExistErasable = true;
                        GameObject block = col.gameObject;
                        if (block.activeSelf)
                        {
                            block.SetActive(false);
                            currentBlocks.Add(block);
                        }
                    }
                }

                if (!isExistErasable)
                {
                    break;
                }
            }
        }
        
    }
}