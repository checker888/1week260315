using UnityEngine;

public class WalkMove : MonoBehaviour, IMovable
{
    public float moveSpeed = 3f;
    public float acceleration = 20f;
    public float deceleration = 25f;

    private Vector3 horizontalVelocity;

    public Vector3 UpdateHorizontalVelocity(Vector2 moveInput, float deltaTime, Transform transform)
    {
        // 5) 入力から「目標水平速度」を作る（向いてる方向基準）
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 inputDir =
            right * moveInput.x +
            forward * moveInput.y;

        inputDir = Vector3.ClampMagnitude(inputDir, 1f);

        Vector3 targetHorizontalVelocity = inputDir * moveSpeed;

        // 6) 目標へ加速/減速で寄せる
        float rate = (inputDir.sqrMagnitude > 0f) ? acceleration : deceleration;
        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetHorizontalVelocity,
            rate * Time.deltaTime
        );


        return horizontalVelocity;
    }
}