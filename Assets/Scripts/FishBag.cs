using System;

[System.Serializable]
public class FishBag
{
    public int Firefishes, Icefishes, Poisonfishes;
    public int Spikefishes, Bonefishes;

    public static FishBag operator + (FishBag a, FishBag b) => new FishBag
    {
        Firefishes = a.Firefishes + b.Firefishes,
        Icefishes = a.Icefishes + b.Icefishes,
        Poisonfishes = a.Poisonfishes + b.Poisonfishes,
        Spikefishes = a.Spikefishes + b.Spikefishes,
        Bonefishes = a.Bonefishes + b.Bonefishes
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
    }
}
