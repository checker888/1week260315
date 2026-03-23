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

    private List<GameObject> workBlocks;
    private List<GameObject> currentBlocks = new List<GameObject>();

    // 変更：Switch管理をHashSetに
    private HashSet<SwitchBlockController> currentSwitches = new HashSet<SwitchBlockController>();

    void Start()
    {
        InvokeRepeating(nameof(UpdateLight), 0f, 0.08f);
    }

    public void UpdateLight()
    {
        workBlocks = new List<GameObject>();

        // --- 前フレームのSwitchを保存 ---
        var previousSwitches = new HashSet<SwitchBlockController>(currentSwitches);
        currentSwitches.Clear();

        foreach (var block in currentBlocks)
        {
            if (block != null || !isShining)
            {
                if (block.tag != "Switch")
                {
                    EraseCheck(block);
                }
            }
        }

        currentBlocks.Clear();
        foreach (var block in workBlocks)
        {
            currentBlocks.Add(block);
        }

        // 光が消えてるなら全Exit
        if (!isShining)
        {
            foreach (var sc in previousSwitches)
            {
                sc.OnLightExit(this);
            }
            return;
        }

        int layerMask = ~LayerMask.GetMask("Player");

        RaycastHit[] hits = Physics.RaycastAll(
            cameraPivot.position,
            cameraPivot.forward,
            rayDistance,
            layerMask
        );

        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                GameObject obj = hit.collider.gameObject;

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
                    else if (col.CompareTag("Switch"))
                    {
                        var sc = col.GetComponent<SwitchBlockController>();

                        // 今フレーム当たってる
                        if (currentSwitches.Add(sc))
                        {
                            sc.OnLightEnter(this);
                        }

                        currentBlocks.Add(col.gameObject);
                    }
                }

                if (!hit.collider.CompareTag("Erasable") &&
                    !hit.collider.CompareTag("Switch") &&
                    !hit.collider.CompareTag("Glass"))
                {
                    break;
                }
            }
        }

        // 前フレームにあって今ない = Exit
        foreach (var sc in previousSwitches)
        {
            if (!currentSwitches.Contains(sc))
            {
                sc.OnLightExit(this);
            }
        }
    }

    public void EraseCheck(GameObject block)
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
        }
    }

    // 設置物からプレイヤーへSwitchBlockの状態を引き継ぐ
    public void InheritSwitchState(LightController other)
    {
        foreach (var sc in other.currentSwitches)
        {
            sc.OnLightEnter(this);
            currentSwitches.Add(sc);
        }
    }

    void OnDisable()
    {
        foreach (var sc in currentSwitches)
        {
            sc.OnLightExit(this);
        }
        currentSwitches.Clear();
    }
}