using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
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

    public BoolVariable OverviewScreenActive;

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

        if (!OverviewScreenActive.Value) controlCannon();
    }

    void FixedUpdate ()
    {
        var inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if (!OverviewScreenActive.Value && inputDirection != Vector2.zero)
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

    void controlCannon ()
    {
        aimCannonAtMouse();

        if (cannonballTimer <= 0 && Input.GetButton("Fire"))
        {
            cannonballTimer = SecondsPerCannonballShot;
            PlayerCannonballFactory.Instance.FireCannonball(CannonBarrel.position, CannonBarrel.right, CannonballStats);
        }
    }

    void aimCannonAtMouse ()
    {
        var mousePos = CameraCache.Main.ScreenToWorldPoint(Input.mousePosition);
        float angle = directionToAngle(mousePos - CannonBarrel.position);
        CannonBarrel.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void accelerate (Vector2 direction)
    {
        var newVelocity = Rigidbody.velocity + direction * Acceleration * Time.deltaTime;
        Rigidbody.velocity = Vector2.ClampMagnitude(newVelocity, TopSpeed);

        Rigidbody.MoveRotation(directionToAngle(Rigidbody.velocity));
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

    float directionToAngle (Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
