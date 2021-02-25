using System;

[System.Serializable]
public class FishBag
{
    public int Firefishes, Icefishes, Poisonfishes;
    public int Spikefishes, Bonefishes, Goldenfishes;

    public static FishBag operator + (FishBag a, FishBag b) => new FishBag
    {
        Firefishes = a.Firefishes + b.Firefishes,
        Icefishes = a.Icefishes + b.Icefishes,
        Poisonfishes = a.Poisonfishes + b.Poisonfishes,
        Spikefishes = a.Spikefishes + b.Spikefishes,
        Bonefishes = a.Bonefishes + b.Bonefishes,
        Goldenfishes = a.Goldenfishes + b.Goldenfishes
    };

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

            case FishType.Goldenfish:
                Goldenfishes++;
                break;

            default:
                throw new ArgumentException($"unexpected FishType ${type}");
        }
    }

    public void Clear ()
    {
        Firefishes = 0;
        Icefishes = 0;
        Poisonfishes = 0;
        Spikefishes = 0;
        Bonefishes = 0;
        Goldenfishes = 0;
    }
}
