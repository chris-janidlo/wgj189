using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoQueueObject : MonoBehaviour
{
    public Image FishImage;
    public FishSpriteDisambiguator FishSpriteDisambiguator;

    void Start ()
    {
        DeathLoopManager.Instance.OnPlayerDied.AddListener(() => Destroy(gameObject));
    }

    public void Initialize (FishType type)
    {
        FishImage.sprite = FishSpriteDisambiguator.GetSpriteByType(type);
    }
}
