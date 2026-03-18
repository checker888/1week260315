using UnityEngine;

public interface IMovable
{
    Vector3 UpdateHorizontalVelocity(
        Vector2 input,
        float deltaTime,
        Transform transform
    );
}