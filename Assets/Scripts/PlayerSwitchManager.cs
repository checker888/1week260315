using System;
using UnityEngine;

public class PlayerSwitchManager: MonoBehaviour
{
    [SerializeField] private PlayerCore firstPlayer;
    public event Action<PlayerCore> OnPlayerSwitched;

    private PlayerCore currentPlayer;
    public PlayerCore CurrentPlayer => currentPlayer;


    void Start()
    {
        SwitchPlayerTo(firstPlayer);
    }

    public void SwitchPlayerTo(PlayerCore nextPlayer)
    {
        if (nextPlayer == null) return;
        if (currentPlayer == nextPlayer) return;

        //今のプレイヤーを行動不能にして、次のプレイヤーを移動可能に
        if (currentPlayer != null){ currentPlayer.CanMove = false;}
        nextPlayer.CanMove = true;

        //操作キャラは見えなくする
        if (currentPlayer != null) { currentPlayer.visual.SetActive(true); }
        nextPlayer.visual.SetActive(false);

        currentPlayer = nextPlayer;


        //イベント発火　CameraManagerでカメラの移動処理
        OnPlayerSwitched?.Invoke(currentPlayer);
    }


}