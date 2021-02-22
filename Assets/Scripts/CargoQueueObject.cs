using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoQueueObject : MonoBehaviour
{
    public Image FishImage;
    public FishSpriteDisambiguator FishSpriteDisambiguator;

    public void Initialize (FishType type)
    {
        FishImage.sprite = FishSpriteDisambiguator.GetSpriteByType(type);
    }
}
