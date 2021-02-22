using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoBlock : MonoBehaviour
{
    public Image FishImage;
    public FishSpriteDisambiguator FishSpriteDisambiguator;

    public void Initialize (FishType type)
    {
        FishImage.sprite = FishSpriteDisambiguator.GetSpriteByType(type);
    }
}
