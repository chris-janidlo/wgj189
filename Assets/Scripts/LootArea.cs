using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class LootArea : MonoBehaviour
{
    public List<Transform> FishSchoolSpawnPoints, CannonSpawnPoints, GhostShipSpawnPoints;

    public EnemyCannon CannonPrefab;
    public EnemyGhostShip GhostShipPrefab;

    public void SpawnLootRow (LootRow lootRow)
    {
        spawn(lootRow.FishSchoolsToSpawnProbabilityCurve, FishSchoolSpawnPoints, t => spawnSchool(lootRow.FishSchools.GetNext(), t));
        spawn(lootRow.CannonsToSpawnProbabilityCurve, CannonSpawnPoints, t => Instantiate(CannonPrefab, t.position, Quaternion.identity));
        spawn(lootRow.GhostShipsToSpawnProbabilityCurve, GhostShipSpawnPoints, t => Instantiate(GhostShipPrefab, t.position, Quaternion.identity));
    }

    void spawn (AnimationCurve probabilityCurve, IEnumerable<Transform> spawnPoints, Action<Transform> spawnRoutine)
    {
        int numberToSpawn = probabilityCurve.GetNumberOfItemsToSpawn();

        var spawnPointList = new List<Transform>(spawnPoints);
        spawnPointList.ShuffleInPlace();

        for (int i = 0; i < numberToSpawn && i < spawnPointList.Count; i++)
        {
            spawnRoutine(spawnPointList[i]);
        }
    }

    void spawnSchool (LootRow.SchoolOfFish school, Transform spawnPoint)
    {
        int numberToSpawn = school.FishToSpawnProbabilityCurve.GetNumberOfItemsToSpawn();

        for (int i = 0; i < numberToSpawn; i++)
        {
            Instantiate(school.Fishes.GetNext(), spawnPoint.position, Quaternion.identity);
        }
    }
}
