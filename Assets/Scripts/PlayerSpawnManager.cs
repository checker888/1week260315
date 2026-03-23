using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject player;

    private void Start()
    {
        int checkpoint = PlayerPrefs.GetInt("NowCheckPoint", 0);

        // ”ÍˆÍŠO‘Îô
        if (checkpoint < 0 || checkpoint >= spawnPoints.Length)
        {
            checkpoint = 0;
        }

        Transform spawn = spawnPoints[checkpoint];

        player.transform.position = spawn.position;
        player.transform.rotation = spawn.rotation;
    }
}