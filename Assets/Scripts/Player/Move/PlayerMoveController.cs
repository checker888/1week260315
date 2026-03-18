using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]//CharacterController‚ÌƒAƒ^ƒbƒ`‚µ–Y‚ê–hŽ~
public class PlayerMoveController : MonoBehaviour
{
    private PlayerCore playerCore;
    private CharacterController characterController;
    private PlayerInputManager inputManager;
    private IMovable movable;
    private GravityController gravityController;


    
    void Awake()
    {
        playerCore = GetComponent<PlayerCore>();
        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<PlayerInputManager>();
        gravityController = GetComponent<GravityController>();

        movable = GetComponent<IMovable>(); 
    }


    void Update()
    {
        MoveInput input = inputManager.CurrentInput;

        Vector3 horizontal = playerCore.CanMove ?
            movable.UpdateHorizontalVelocity(
                input.move,
                Time.deltaTime,
                transform
            )
            : Vector3.zero;


        float vertical = gravityController.UpdateVerticalVelocity(
            characterController.isGrounded,
            playerCore.CanMove ? input.jumpPressed : false,
            Time.deltaTime
        );

        Vector3 velocity = horizontal + Vector3.up * vertical;

        characterController.Move(velocity * Time.deltaTime);
    }
}