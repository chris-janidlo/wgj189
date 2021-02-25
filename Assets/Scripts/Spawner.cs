using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class LootTable : BagRandomizer<LootRow> { }

    [Header("Loot Tables")]
    public LootTable ParadiseLoot;
    public LootTable FortressLoot, LockedIslandLoot, RuneIslandLoot, BasicIslandLoot, GuaranteedFishingSpotLoot, OtherFishingSpotLoot;

    [Header("Island References")]
    public LootArea Paradise;
    public LootArea Fortress;
    public List<LootArea> LockedIslands;
    public List<PotentialRuneIsland> PotentialRuneIslands;
    public List<RuneDoor> BasicRuneDoors, BarrenRockRuneDoors;
    public RunePickup GeboPrefab, JeraPrefab, OthalaPrefab;

    [Header("Fishing Spot References")]
    public LootArea FishingSpotPrefab;
    public List<Transform> FishingSpotSpawnPoints, GuaranteedFishingSpotZone;
    public AnimationCurve FishingSpotsToSpawnProbabilityCurve;

    void Start ()
    {
        Spawn();
    }

    public void Spawn ()
    {
        var shuffledPotentialRuneIslands = shuffle(PotentialRuneIslands);

        var runeIslands = shuffledPotentialRuneIslands.Take(3).ToList();
        var basicIslands = shuffledPotentialRuneIslands.Skip(3).ToList();

        spreadRunes(runeIslands);
        spawnFishingSpots();
        spawnIslandLoot(runeIslands, basicIslands);
    }

    private void spreadRunes (List<PotentialRuneIsland> runeIslands)
    {
        Instantiate(GeboPrefab, runeIslands[0].RuneSpawnPoint.position, Quaternion.identity);
        Instantiate(JeraPrefab, runeIslands[1].RuneSpawnPoint.position, Quaternion.identity);
        Instantiate(OthalaPrefab, runeIslands[2].RuneSpawnPoint.position, Quaternion.identity);

        var basicRuneDoorsShuffled = shuffle(BasicRuneDoors);
        var runesToRegister = new List<RuneType> { RuneType.Gebo, RuneType.Jera, RuneType.Othala };
        runesToRegister.ShuffleInPlace();

        // rune 1 opens door a, b
        // rune 2 opens door b, barren rock 1
        // rune 3 opens door c, barren rock 2

        basicRuneDoorsShuffled[0].RegisterLocks(runesToRegister[0]);
        basicRuneDoorsShuffled[1].RegisterLocks(runesToRegister[0], runesToRegister[1]);
        basicRuneDoorsShuffled[2].RegisterLocks(runesToRegister[2]);

        BarrenRockRuneDoors[0].RegisterLocks(runesToRegister[1]);
        BarrenRockRuneDoors[1].RegisterLocks(runesToRegister[2]);
    }

    private void spawnFishingSpots ()
    {
        var guaranteedSpot = GuaranteedFishingSpotZone.PickRandom();
        Instantiate(FishingSpotPrefab, guaranteedSpot.position, Quaternion.identity).SpawnLootRow(GuaranteedFishingSpotLoot.GetNext());

        int spotsToSpawn = FishingSpotsToSpawnProbabilityCurve.GetNumberOfItemsToSpawn();
        var shuffledFishSpots = shuffle(FishingSpotSpawnPoints);

        for (int i = 0; i < spotsToSpawn; i++)
        {
            var t = shuffledFishSpots[i];
            if (t == guaranteedSpot) continue;

            Instantiate(FishingSpotPrefab, t.position, Quaternion.identity).SpawnLootRow(OtherFishingSpotLoot.GetNext());
        }
    }

    private void spawnIslandLoot (List<PotentialRuneIsland> runeIslands, List<PotentialRuneIsland> basicIslands)
    {
        Paradise.SpawnLootRow(ParadiseLoot.GetNext());
        Fortress.SpawnLootRow(FortressLoot.GetNext());

        foreach (var island in LockedIslands)
        {
            island.SpawnLootRow(LockedIslandLoot.GetNext());
        }

        foreach (var island in runeIslands)
        {
            island.SpawnLootRow(RuneIslandLoot.GetNext());
        }

        foreach (var island in basicIslands)
        {
            island.SpawnLootRow(BasicIslandLoot.GetNext());
        }
    }

    List<T> shuffle<T> (List<T> input)
    {
        var result = new List<T>(input);
        result.ShuffleInPlace();
        return result;
    }
}
