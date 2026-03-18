using System;
using UnityEngine;

public class PlayerSwitchManager: MonoBehaviour
{
    public GameObject firstPlayerObj;
    private PlayerCore firstPlayer;
    private PlayerInputManager inputManager;

    public event Action<PlayerCore> OnPlayerSwitched;
    private MenuViewManager menuViewManager;

    private PlayerCore currentPlayer;
    private SettingManager settingManager;

    void Start()
    {
        firstPlayer = firstPlayerObj.GetComponent<PlayerCore>();
        inputManager = firstPlayerObj.GetComponent<PlayerInputManager>();
        menuViewManager = GetComponent<MenuViewManager>();
        settingManager = GetComponent<SettingManager>();
        LockCursor();
        SetPlayer(firstPlayer);
    }

    void Update()
    {
        MoveInput input = inputManager.CurrentInput;

        if (input.menuPressed)
        {
            UnlockCursor();
            if(menuViewManager.IsActive && menuViewManager.nowActiveMenu == "Home")
            {
                
                LockCursor();
                menuViewManager.SetMenu("");
            }
            else
            {
                if(menuViewManager.nowActiveMenu == "Settings")
                {
                    settingManager.ApplyAndBackHome();
                }
                else
                {
                    
                    menuViewManager.SetMenu("Home");
                }
                
            }
        }

        if (input.cursorLockPressed && !menuViewManager.IsActive)
        {
            LockCursor();
        }

        if(menuViewManager.nowActiveMenu == "")
        {
            SetCanMove(true);
        }
        else
        {
            SetCanMove(false);
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void SetPlayer(PlayerCore nextPlayer)
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
        OnPlayerSwitched?.Invoke(currentPlayer);
    }


    public void SetCanMove(bool f)
    {
        firstPlayer.CanMove = f;
    }

}