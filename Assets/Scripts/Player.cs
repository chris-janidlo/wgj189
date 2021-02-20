using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class Player : Singleton<Player>
{
    public float Acceleration, Deceleration, TopSpeed;
    public Rigidbody2D Rigidbody;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    void FixedUpdate ()
    {
        var inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if (inputDirection != Vector2.zero)
        {
            accelerate(inputDirection);
        }
        else
        {
            decelerate();
        }
    }

    void accelerate (Vector2 direction)
    {
        var newVelocity = Rigidbody.velocity + direction * Acceleration * Time.deltaTime;
        Rigidbody.velocity = Vector2.ClampMagnitude(newVelocity, TopSpeed);

        var angle = Mathf.Atan2(Rigidbody.velocity.y, Rigidbody.velocity.x) * Mathf.Rad2Deg;
        Rigidbody.MoveRotation(angle);
    }

    void decelerate ()
    {
        var decelerationPerFrame = Deceleration * Time.deltaTime;

        if (Rigidbody.velocity.magnitude <= decelerationPerFrame)
        {
            Rigidbody.velocity = Vector2.zero;
        }
        else
        {
            Rigidbody.velocity -= Rigidbody.velocity.normalized * decelerationPerFrame;
        }
    }
}
