using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class FishPickup : MonoBehaviour
{
    public FishType Type;

    public float WanderDistance, WanderMagnitude, TopSpeed;
    public float TurnSpeed;

    public Rigidbody2D Rigidbody;
    public SpriteRenderer SpriteRenderer;

    Vector2 originalPosition;

    void Start ()
    {
        originalPosition = transform.position;
    }

    void FixedUpdate ()
    {
        Vector2 dir = Vector2.Distance(transform.position, originalPosition) > WanderDistance
            ? (originalPosition - (Vector2) transform.position)
            : Vector2.Scale(Random.insideUnitCircle, new Vector2(1, .5f));

        Rigidbody.AddForce(dir * Random.Range(0, WanderMagnitude));
        Rigidbody.velocity = Vector2.ClampMagnitude(Rigidbody.velocity, TopSpeed);

        float absX = Mathf.Abs(Rigidbody.velocity.x);
        float absY = Mathf.Abs(Rigidbody.velocity.y);

        if (absX >= TurnSpeed && absX > absY)
        {
            SpriteRenderer.flipX = Rigidbody.velocity.x < 0;
        }
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        
        if (player != null)
        {
            player.CollectFish(Type);
            Destroy(gameObject);
        }
    }
}
