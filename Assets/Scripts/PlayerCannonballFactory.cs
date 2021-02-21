using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class PlayerCannonballFactory : Singleton<PlayerCannonballFactory>
{
    public PlayerCannonball CannonballPrefab;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    public void FireCannonball (Vector2 position, Vector2 direction, PlayerCannonball.Stats stats)
    {
        var orientation = Quaternion.identity; // TODO
        var cannonball = Instantiate(CannonballPrefab, position, orientation);
        cannonball.StatBlock = stats;
        cannonball.Rigidbody.velocity = stats.Speed * direction;
    }
}
