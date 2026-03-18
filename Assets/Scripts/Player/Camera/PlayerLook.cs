using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private PlayerCore playerCore;
    private Transform cam;
    public float sensitivity = 0.5f;

    //角度制限(x:上下、y:左右)
    [SerializeField] private float minX = -90f;
    [SerializeField] private float maxX = 90f;

    //(左右の制限は必要な場合のみ)
    [SerializeField] private bool limitY = false;
    [SerializeField] private float minY = -90f;
    [SerializeField] private float maxY = 90f;
    
   


    private float initialYaw;
    //新InputSystem
    private PlayerInputActions inputActions;
    private Vector2 lookInput;

    private float yaw;
    private float pitch;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        // InputActionsからLookの値を受け取る
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }
    void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 0.5f);
        playerCore = GetComponent<PlayerCore>();
        cam = playerCore.cameraPivot;

        initialYaw = transform.localEulerAngles.y;
        yaw = initialYaw;

        float camX = cam.localEulerAngles.x;
        if (camX > 180f) camX -= 360f;
        pitch = camX;
    }
    void OnEnable()  => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Update(){ if(playerCore.CanMove)CameraUpdate();}

    void CameraUpdate()
    {
        // マウス入力を使って角度更新
        yaw += lookInput.x * sensitivity;

        //左右方向の制限チェック
        if (limitY)
        {
            float relativeYaw = Mathf.DeltaAngle(initialYaw, yaw);
            relativeYaw = Mathf.Clamp(relativeYaw, minY, maxY);
            yaw = initialYaw + relativeYaw;
        }

        pitch -= lookInput.y * sensitivity;

        pitch = Mathf.Clamp(pitch, minX, maxX);

        transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}