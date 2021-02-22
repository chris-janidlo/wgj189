using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;

public class CargoBlock : MonoBehaviour
{
    public TransitionableFloat ScaleTransition;
    public TransitionableVector2 PositionTransition;

    public Image FishImage;
    public FishSpriteDisambiguator FishSpriteDisambiguator;

    void Update ()
    {
        transform.localScale = Vector3.one * ScaleTransition.Value;
        transform.position = PositionTransition.Value;
    }

    public void Initialize (FishType type)
    {
        ScaleTransition.AttachMonoBehaviour(this);
        PositionTransition.AttachMonoBehaviour(this);

        FishImage.sprite = FishSpriteDisambiguator.GetSpriteByType(type);

        ScaleTransition.FlashFromTo(0, 1);
    }

    public void Move (Vector2 from, Vector2 to)
    {
        PositionTransition.FlashFromTo(from, to);
    }
}
