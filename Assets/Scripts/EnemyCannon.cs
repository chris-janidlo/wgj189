using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    public float SecondsPerShot, PlayerAttackDistance;
    public EnemyBullet BulletPrefab;
    public Transform Barrel;

    float cannonTimer;

    void Update ()
    {
        cannonTimer -= Time.deltaTime;

        if (Vector2.Distance(Player.Instance.transform.position, transform.position) > PlayerAttackDistance) return;

        pointBarrelAtPlayer();

        if (cannonTimer <= 0)
        {
            cannonTimer = SecondsPerShot;
            Instantiate(BulletPrefab, (Vector2) Barrel.position, Barrel.rotation);
        }
    }

    void pointBarrelAtPlayer ()
    { 
        var dir = Barrel.position - Player.Instance.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        Barrel.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
