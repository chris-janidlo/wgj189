using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float CurrentHealth { get; set; }

    public float MaxHealth;

    void Start ()
    {
        CurrentHealth = MaxHealth;
    }

    public void Damage (float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
