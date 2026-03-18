using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera mainCamera;
    private PlayerSwitchManager switchManager;

    [SerializeField] private float nearClipPlane = 0.1f;
    private void Awake()
    {
        switchManager = GetComponent<PlayerSwitchManager>();
        if (mainCamera == null)
        {
            Debug.LogError("MainCamera is not found");
            return;
        }

        if (switchManager == null)
        {
            Debug.LogError("PlayerSwitchManager is not found");
            return;
        }
        SetCameraSettings();
        
        switchManager.OnPlayerSwitched += CameraAttachTo;

    }
    void SetCameraSettings()
    {
        mainCamera.nearClipPlane = nearClipPlane;
    }
    public void CameraAttachTo(PlayerCore player)
    {
        if (player == null || player.cameraPivot == null)
        {
            Debug.LogWarning("Player or cameraPivot is null");
            return;
        }
        mainCamera.transform.SetParent(player.cameraPivot);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;
    }

    void Start()
    {
        LockCursor();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
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


}
