using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhostShip : MonoBehaviour
{
    public float Acceleration, TopSpeed;
    public float Damage;
    public float StealthTime, UnstealthDistance;
    public float WakeupDistance;

    public Rigidbody2D Rigidbody;
    public Collider2D Collider;
    public Animator Animator;

    bool awake, stealthed;
    Vector2 unstealthPosition;

    void FixedUpdate ()
    {
        if (!awake)
        {
            if (Vector3.Distance(Player.Instance.transform.position, transform.position) < WakeupDistance)
            {
                awake = true;
            }
            else
            {
                return;
            }
        }

        if (!stealthed)
        {
            accelerate(Player.Instance.transform.position - transform.position);
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            player.Damage(Damage);
            StartCoroutine(stealthRoutine());
        }
    }

    void accelerate (Vector2 direction)
    {
        var newVelocity = Rigidbody.velocity + direction * Acceleration * Time.deltaTime;
        Rigidbody.velocity = Vector2.ClampMagnitude(newVelocity, TopSpeed);

        var angle = Mathf.Atan2(Rigidbody.velocity.y, Rigidbody.velocity.x) * Mathf.Rad2Deg;
        Rigidbody.MoveRotation(angle);
    }

    IEnumerator stealthRoutine ()
    {
        stealthed = true;
        switchPhysics(false);
        Animator.Play("Stealth");

        yield return new WaitForSeconds(StealthTime);

        yield return findUnstealthPosition();
        transform.position = unstealthPosition;

        Animator.Play("Unstealth");
        switchPhysics(true);
        stealthed = false;
    }

    void switchPhysics (bool on)
    {
        Collider.enabled = on;
        Rigidbody.simulated = on;
    }

    IEnumerator findUnstealthPosition ()
    {
        var maxExtent = Mathf.Max(Collider.bounds.extents.x, Collider.bounds.extents.y);

        while (true)
        {
            var result = (Vector2) transform.position + Random.insideUnitCircle * UnstealthDistance;

            if (!Physics2D.OverlapCircle(result, 2 * maxExtent))
            {
                unstealthPosition = result;
                yield break;
            }

            yield return null;
        }
    }
}
