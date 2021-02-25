using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrenRockAnsuzSpawner : MonoBehaviour
{
    public Transform SpawnPoint;
    public RunePickup AnsuzPrefab;

    void Start ()
    {
        Spawn();
    }

    public void Spawn ()
    {
        Instantiate(AnsuzPrefab, SpawnPoint);
    }
}
