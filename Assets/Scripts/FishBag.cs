using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishBag
{
    public int Firefishes, Icefishes, Poisonfishes;
    public int Spikefishes, Bonefishes;

    public void AddFish (FishType type)
    {
        switch (type)
        {
            case FishType.Firefish:
                Firefishes++;
                break;

            case FishType.Icefish:
                Icefishes++;
                break;

            case FishType.Poisonfish:
                Poisonfishes++;
                break;

            case FishType.Spikefish:
                Spikefishes++;
                break;

            case FishType.Bonefish:
                Bonefishes++;
                break;
        }
    }
}
