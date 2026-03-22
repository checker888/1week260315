using UnityEngine;
using System.Collections.Generic;

public class WallGridController : MonoBehaviour
{
    [Header("セルサイズ")]
    public float cellSize = 0.25f;

    [Header("壁マテリアル（任意）")]
    public Material cellMaterial;

    private int cols, rows, deps;
    private GameObject[,,] cells;
    private bool[,,] erased;
    private HashSet<Vector3Int> pendingRestore = new HashSet<Vector3Int>();
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        if(playerTransform == null)
        {
            playerTransform =  GameObject.FindWithTag("MainCamera")?.transform;
        }
        BuildGrid();
    }

    void BuildGrid()
    {
        // 親ブロックのサイズをcellSizeで割ってグリッド数を決定
        Vector3 size = GetComponent<Renderer>().bounds.size;
        cols = Mathf.RoundToInt(size.x / cellSize);
        rows = Mathf.RoundToInt(size.y / cellSize);
        deps = Mathf.RoundToInt(size.z / cellSize);

        cols = Mathf.Max(cols, 1);
        rows = Mathf.Max(rows, 1);
        deps = Mathf.Max(deps, 1);

        cells = new GameObject[cols, rows, deps];
        erased = new bool[cols, rows, deps];

        // 親ブロックの見た目・Colliderを無効化（子で代替するため）
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // 左下手前を起点にセルを生成
        Vector3 origin = transform.position
            - transform.right * size.x * 0.5f
            - transform.up * size.y * 0.5f
            - transform.forward * size.z * 0.5f;

        for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
                for (int d = 0; d < deps; d++)
                {
                    Vector3 pos = origin
                        + transform.right * (c * cellSize + cellSize * 0.5f)
                        + transform.up * (r * cellSize + cellSize * 0.5f)
                        + transform.forward * (d * cellSize + cellSize * 0.5f);

                    GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cell.transform.position = pos;
                    cell.transform.rotation = transform.rotation;
                    cell.transform.localScale = Vector3.one * cellSize;
                    cell.transform.SetParent(transform);
                    cell.tag = "Erasable";

                    if (cellMaterial != null)
                        cell.GetComponent<Renderer>().material = cellMaterial;

                    cells[c, r, d] = cell;
                }

        Debug.Log($"[Grid] cols={cols} rows={rows} deps={deps} total={cols * rows * deps}");
    }

    public void EraseAround(Vector3 worldHitPoint, float radius)
    {
        Vector3Int center = WorldToCell(worldHitPoint);
        int cellRadius = Mathf.CeilToInt(radius / cellSize);

        for (int dc = -cellRadius; dc <= cellRadius; dc++)
            for (int dr = -cellRadius; dr <= cellRadius; dr++)
                for (int dd = -cellRadius; dd <= cellRadius; dd++)
                {
                    Vector3Int idx = new Vector3Int(center.x + dc, center.y + dr, center.z + dd);
                    if (!IsValid(idx)) continue;
                    if (erased[idx.x, idx.y, idx.z]) continue;
                    if (cells[idx.x, idx.y, idx.z] == null) continue;

                    erased[idx.x, idx.y, idx.z] = true;
                    cells[idx.x, idx.y, idx.z].SetActive(false);
                }
    }

    public void TryRestoreAll()
    {
        List<Vector3Int> restored = new List<Vector3Int>();
        foreach (Vector3Int idx in pendingRestore)
        {
            if (!IsTouchingPlayer(idx))
            {
                cells[idx.x, idx.y, idx.z].SetActive(true);
                erased[idx.x, idx.y, idx.z] = false;
                restored.Add(idx);
            }
        }
        foreach (var idx in restored) pendingRestore.Remove(idx);
    }

    bool IsTouchingPlayer(Vector3Int idx)
    {
        Vector3 cellCenter = cells[idx.x, idx.y, idx.z].transform.position;
        Vector3 playerPos = playerTransform.position;
        float half = cellSize * 0.5f + 0.5f;
        return Mathf.Abs(playerPos.x - cellCenter.x) < half
            && Mathf.Abs(playerPos.y - cellCenter.y) < half
            && Mathf.Abs(playerPos.z - cellCenter.z) < half;
    }

    Vector3Int WorldToCell(Vector3 worldPos)
    {
        Vector3 local = transform.InverseTransformPoint(worldPos);
        Vector3 size = new Vector3(cols, rows, deps) * cellSize;
        int c = Mathf.FloorToInt((local.x + size.x * 0.5f) / cellSize);
        int r = Mathf.FloorToInt((local.y + size.y * 0.5f) / cellSize);
        int d = Mathf.FloorToInt((local.z + size.z * 0.5f) / cellSize);
        return new Vector3Int(
            Mathf.Clamp(c, 0, cols - 1),
            Mathf.Clamp(r, 0, rows - 1),
            Mathf.Clamp(d, 0, deps - 1)
        );
    }

    bool IsValid(Vector3Int idx)
        => idx.x >= 0 && idx.x < cols
        && idx.y >= 0 && idx.y < rows
        && idx.z >= 0 && idx.z < deps;
}