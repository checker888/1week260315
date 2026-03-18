using UnityEngine;

public struct MoveInput
{
    public Vector2 move;
    public bool jumpPressed;
    public bool putPressed;
    public bool takePressed;
    public bool switchPressed;
    public bool menuPressed;
    public bool cursorLockPressed;
}

public class PlayerInputManager : MonoBehaviour
{
    PlayerInputActions input;

    public MoveInput CurrentInput { get; private set; }

    void Awake()
    {
        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        CurrentInput = new MoveInput
        {
            move = input.Player.Move.ReadValue<Vector2>(),
            jumpPressed = input.Player.Jump.triggered,
            putPressed = input.Player.PutLight.triggered,
            takePressed = input.Player.TakeLight.triggered,
            switchPressed = input.Player.SwitchLight.triggered,
            menuPressed = input.Player.Menu.triggered,
            cursorLockPressed = input.Player.CursorLock.triggered,
        };
    }
}