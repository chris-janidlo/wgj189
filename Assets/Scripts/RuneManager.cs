using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using crass;

public class RuneManager : Singleton<RuneManager>
{
    public List<RuneType> CollectedRunes;

    public RuneTypeEvent RuneCollected;

    public List<Image> RuneUIImages;
    public RuneSpriteDisambiguator RuneSpriteDisambiguator;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    public void Clear ()
    {
        CollectedRunes.Clear();
        
        foreach (var image in RuneUIImages)
        {
            image.sprite = null;
            image.color = Color.clear;
        }
    }

    public void CollectRune (RuneType type)
    {
        CollectedRunes.Add(type);
        RuneCollected.Invoke(type);

        var image = RuneUIImages[CollectedRunes.Count - 1];
        image.sprite = RuneSpriteDisambiguator.GetSpriteByType(type);
        image.color = Color.white;
    }
}

[System.Serializable]
public class RuneTypeEvent : UnityEvent<RuneType> { }
