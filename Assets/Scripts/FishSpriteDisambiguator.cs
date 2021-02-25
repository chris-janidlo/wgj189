using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Fish Sprite Disambiguator", fileName = "newFishSpriteDisambiguator.asset")]
public class FishSpriteDisambiguator : ScriptableObject
{
    public Sprite FirefishSprite, IcefishSprite, PoisonfishSprite, SpikefishSprite, BonefishSprite, GoldenfishSprite;

    public Sprite GetSpriteByType (FishType type)
    {
        switch (type)
        {
            case FishType.Firefish:
                return FirefishSprite;

            case FishType.Icefish:
                return IcefishSprite;

            case FishType.Poisonfish:
                return PoisonfishSprite;

            case FishType.Spikefish:
                return SpikefishSprite;

            case FishType.Bonefish:
                return BonefishSprite;

            case FishType.Goldenfish:
                return GoldenfishSprite;

            default:
                throw new ArgumentException($"unexpected FishType ${type}");
        }
    }
}
