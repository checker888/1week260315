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
    private List<SwitchBlockController> scs = new List<SwitchBlockController>();
    private float unlitDelay = 0.1f;
    private Dictionary<SwitchBlockController, float> timers = new();
    void Start()
    {
        InvokeRepeating(nameof(UpdateLight), 0f, 0.08f);
    }
    public void UpdateLight()
    {
        workBlocks = new List<GameObject>();
        foreach (var block in currentBlocks)
        {
            if (block != null || !isShining)
            {
                if (block.tag == "Switch")
                {
                    SwitchBlockController sc = block.GetComponent<SwitchBlockController>();
                    scs.Add(sc);
                }
                else
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
        if (!isShining)
        {
            SCSCheck();
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
                        GameObject block = col.gameObject;
                        SwitchBlockController sc = block.GetComponent<SwitchBlockController>();

                        timers[sc] = Time.time;

                        if (!sc.isLit)
                        {
                            sc.isLit = true;
                            sc.UpdateBlockState();
                        }

                        currentBlocks.Add(block);
                        if (scs.Contains(sc))
                        {
                            scs.Remove(sc);
                        }
                    }
                }
                if (!hit.collider.CompareTag("Erasable") && !hit.collider.CompareTag("Switch") &&!hit.collider.CompareTag("Glass"))
                {
                    break;
                }
            }
        }

        SCSCheck();
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

    public void SCSCheck()
    {
        List<SwitchBlockController> toRemove = new List<SwitchBlockController>();

        foreach (SwitchBlockController sc in scs)
        {
            if (Time.time - timers.GetValueOrDefault(sc, 0f) > unlitDelay)
            {
                sc.isLit = false;
                sc.UpdateBlockState();
                toRemove.Add(sc);
            }
        }

        foreach (var sc in toRemove)
        {
            scs.Remove(sc);
        }
    }

    // 設置物からプレイヤーへSwitchBlockの状態を引き継ぐ
    public void InheritSwitchState(LightController other)
    {
        foreach (var kvp in other.timers)
        {
            timers[kvp.Key] = Time.time; // タイマーをリセットして引き継ぐ
        }
        foreach (var sc in other.scs)
        {
            if (!scs.Contains(sc))
                scs.Add(sc);
        }
        // 照射中だったSwitchBlockもcurrentBlocksに引き継ぐ
        foreach (var block in other.currentBlocks)
        {
            if (block != null && !currentBlocks.Contains(block))
                currentBlocks.Add(block);
        }
    }
}