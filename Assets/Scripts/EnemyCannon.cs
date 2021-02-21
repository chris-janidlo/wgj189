using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    public float SecondsPerShot;
    public EnemyBullet BulletPrefab;
    public Transform Barrel;

    IEnumerator Start ()
    {
        while (true)
        {
            yield return new WaitForSeconds(SecondsPerShot);
            Instantiate(BulletPrefab, (Vector2) Barrel.position, Barrel.rotation);
        }
    }

    void Update ()
    {
        pointBarrelAtPlayer();
    }

    void pointBarrelAtPlayer ()
    { 
        var dir = Barrel.position - Player.Instance.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        Barrel.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
