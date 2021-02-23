using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

public class Player : Singleton<Player>
{
    public float CurrentHealth { get; private set; }

    public bool MousedOver { get; private set; }

    public float MaxHealth;
    public float Acceleration, Deceleration, TopSpeed;

    public float SecondsPerCannonballShot;
    public PlayerCannonball.Stats CannonballStats;

    public FishBag Fishes;

    public Rigidbody2D Rigidbody;
    public Transform CannonBarrel;

    public BoolVariable OverviewScreenActive, DeathScreenActive;

    float cannonballTimer;

    int lineIndex;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    void Start ()
    {
        OnRespawn();
    }

    void Update ()
    {
        if (DeathScreenActive.Value) return;

        cannonballTimer -= Time.deltaTime;

        // currently, you can use the cannon while navigating. might disable that to encourage loot management instead
        if (!OverviewScreenActive.Value) controlCannon();

        if (!Navigator.Instance.Navigating && lineIndex > 0) stopFollowingLine();
    }

    void FixedUpdate ()
    {
        if (DeathScreenActive.Value)
        {
            Rigidbody.velocity = Vector2.zero;
            return;
        }

        var inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if (!OverviewScreenActive.Value && inputDirection != Vector2.zero)
        {
            stopFollowingLine();
            accelerate(inputDirection);
        }
        else if (Navigator.Instance.Navigating)
        {
            followLine();
        }
        else
        {
            decelerate();
        }
    }

    void OnMouseEnter ()
    {
        MousedOver = true;
    }

    void OnMouseExit ()
    {
        MousedOver = false;
    }

    public void OnRespawn ()
    {
        CurrentHealth = MaxHealth;
    }

    public void Damage (float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            DeathLoopManager.Instance.EnterScreen();
            transform.position = Vector3.zero;
        }
    }

    void controlCannon ()
    {
        lookAt2D(CannonBarrel, CameraCache.Main.ScreenToWorldPoint(Input.mousePosition));

        if (cannonballTimer <= 0 && Input.GetButton("Fire"))
        {
            cannonballTimer = SecondsPerCannonballShot;
            PlayerCannonballFactory.Instance.FireCannonball(CannonBarrel.position, CannonBarrel.right, CannonballStats);
        }
    }

    void followLine ()
    {
        Rigidbody.bodyType = RigidbodyType2D.Kinematic;

        var line = Navigator.Instance.Line;

        if (lineIndex >= line.positionCount)
        {
            stopFollowingLine();
            return;
        }

        Vector2 target = line.GetPosition(lineIndex);

        lookAt2D(transform, target);
        transform.position = Vector2.MoveTowards(transform.position, target, TopSpeed * Time.deltaTime);

        if ((Vector2) transform.position == target) lineIndex++;
    }

    void stopFollowingLine ()
    {
        Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        Navigator.Instance.DestroyLine();
        lineIndex = 0;
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

    void lookAt2D (Transform transform, Vector2 point)
    {
        var angle = directionToAngle(point - (Vector2) transform.position);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    float directionToAngle (Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
