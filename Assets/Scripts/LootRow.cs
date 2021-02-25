using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[CreateAssetMenu(menuName = "Loot Row", fileName = "newLootRow.asset")]
public class LootRow : ScriptableObject
{
    [Serializable]
    public class SchoolOfFish
    {
        [Serializable]
        public class FishBag : BagRandomizer<FishPickup> { }

        public FishBag Fishes;
        public AnimationCurve FishToSpawnProbabilityCurve;
    }

    [Serializable]
    public class SchoolOfFishBag : BagRandomizer<SchoolOfFish> { }

    public SchoolOfFishBag FishSchools;

    public AnimationCurve FishSchoolsToSpawnProbabilityCurve, CannonsToSpawnProbabilityCurve, GhostShipsToSpawnProbabilityCurve;
}
