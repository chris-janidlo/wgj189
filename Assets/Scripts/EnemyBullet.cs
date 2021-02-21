using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float Speed, Damage;
    public Rigidbody2D Rigidbody;
    public Collider2D Collider;
    public Animator Animator;

    void Start ()
    {
        Rigidbody.velocity = transform.up * Speed;
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            player.Damage(Damage);
        }

        Destroy(Collider);
        Destroy(Rigidbody);

        Animator.Play("Death");
    }

    void OnBecameInvisible ()
    {
        Despawn();
    }

    public void Despawn ()
    {
        Destroy(gameObject);
    }
}
