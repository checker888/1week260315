using UnityEngine;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    public GameObject lightObj;
    public float rayDistance = 20f;
    public Transform cameraPivot;
    [Header("Light Settings")]
    public float minRadius = 1f;
    public float maxRadius = 2.5f;
    public float maxDistance = 10f;
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
        if (Physics.Raycast(cameraPivot.position, cameraPivot.forward, out RaycastHit hit, rayDistance, layerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            float distance = hit.distance;
            float dynamicRadius = Mathf.Lerp(minRadius, maxRadius, distance / maxDistance);
            //範囲内のブロック取得
            Collider[] hits = Physics.OverlapSphere(hit.point, dynamicRadius);

            foreach (var col in hits)
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