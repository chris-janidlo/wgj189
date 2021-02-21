using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class Player : Singleton<Player>
{
    public float CurrentHealth { get; private set; }

    public float MaxHealth;
    public float Acceleration, Deceleration, TopSpeed;

    public float SecondsPerCannonballShot;
    public PlayerCannonball.Stats CannonballStats;

    public Rigidbody2D Rigidbody;
    public Transform CannonBarrel;

    float cannonballTimer;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    void Start ()
    {
        CurrentHealth = MaxHealth;
    }

    void Update ()
    {
        cannonballTimer -= Time.deltaTime;

        if (cannonballTimer <= 0 && Input.GetButton("Fire"))
        {
            cannonballTimer = SecondsPerCannonballShot;
            PlayerCannonballFactory.Instance.FireCannonball(CannonBarrel.position, transform.right, CannonballStats);
        }
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

    public void Damage (float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            // TODO: load outer loop scene
            Destroy(gameObject);
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
