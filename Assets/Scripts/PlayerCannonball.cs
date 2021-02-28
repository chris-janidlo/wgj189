using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannonball : MonoBehaviour
{
    [Serializable]
    public struct Stats
    {
        public float Speed, Damage;
        public AnimationCurve VerticalArc;
    }

    public Stats StatBlock { get; set; }

    public float SinkZ;

    public Rigidbody2D Rigidbody;
    public Collider2D Collider;
    public Animator Animator;

    float timer;
    bool dying;

    void Start ()
    {
        DeathLoopManager.Instance.OnPlayerDied.AddListener(() => Destroy(gameObject));
    }

    void FixedUpdate ()
    {
        if (dying) return;

        var z = StatBlock.VerticalArc.Evaluate(timer);
        timer += Time.deltaTime;

        transform.localScale = new Vector3(z, z, 1);

        if (z <= SinkZ) Despawn();
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.Damage(StatBlock.Damage);
        }

        dying = true;
        switchPhysics(false);
        transform.localScale = Vector3.one;
        Animator.Play("Explode");
    }

    public void Despawn ()
    {
        Destroy(gameObject);
    }

    void switchPhysics (bool on)
    {
        Collider.enabled = on;
        Rigidbody.simulated = on;
    }
}
