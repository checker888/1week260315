using UnityEngine;

public class GravityController : MonoBehaviour
{
    public bool jumpEnabled = true;
    public float gravity = -20f;
    public float jumpHeight = 1.2f;
    public float fallGravityMultiplier = 1.5f; // 落下中だけ重力を強く/弱く
    public float maxFallSpeed = 20f;
    public float groundedStickVelocity = -2f;

    private float verticalVelocity;

    public float UpdateVerticalVelocity(bool isGrounded, bool jumped, float deltaTime)
    {
        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedStickVelocity; // 地面吸着用
        }

        if (jumpEnabled && jumped && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }
        float gravityScale = 1f;
        if (!isGrounded && verticalVelocity < 0f)
        {
            gravityScale = fallGravityMultiplier;
        }
        verticalVelocity += gravity * gravityScale * deltaTime;
        //落下速度の上限
        if (verticalVelocity < -maxFallSpeed)
        {
            verticalVelocity = -maxFallSpeed;
        }
        return verticalVelocity;
    }
}