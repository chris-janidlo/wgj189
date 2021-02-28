using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlpool : MonoBehaviour
{
    public float SpinSpeed;

    void Update ()
    {
        var rotation = new Vector3
        {
            x = transform.eulerAngles.x,
            y = transform.eulerAngles.y,
            z = Mathf.Repeat(transform.eulerAngles.z + SpinSpeed * Time.deltaTime, 360)
        };

        transform.rotation = Quaternion.Euler(rotation);
    }
    void OnTriggerEnter2D (Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.Damage(float.MaxValue);
        }
    }
}
