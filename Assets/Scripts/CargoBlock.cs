using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;
using TMPro;

public class CargoBlock : MonoBehaviour
{
    public bool Merging { get; private set; }

    public FishType FishType;
    public int FishCount = 1;

    public TransitionableFloat ScaleTransition;
    public TransitionableVector2 PositionTransition;

    public Image FishImage;
    public TextMeshProUGUI CountText;

    public FishSpriteDisambiguator FishSpriteDisambiguator;

    void Update ()
    {
        transform.localScale = Vector3.one * ScaleTransition.Value;
        transform.position = PositionTransition.Value;

        if (!PositionTransition.Transitioning && Merging)
        {
            CountText.text = FishCount.ToString();
            Merging = false;
        }
    }

    public void Initialize (FishType type)
    {
        ScaleTransition.AttachMonoBehaviour(this);
        PositionTransition.AttachMonoBehaviour(this);

        FishType = type;
        FishImage.sprite = FishSpriteDisambiguator.GetSpriteByType(type);

        ScaleTransition.FlashFromTo(0, 1);
    }

    public void Move (Vector2 from, Vector2 to)
    {
        PositionTransition.FlashFromTo(from, to);
    }

    public bool CanMergeWith (CargoBlock other)
    {
        return
            !Merging &&
            !other.Merging &&
            FishType == other.FishType &&
            FishCount == other.FishCount;
    }

    public void MergeWith (CargoBlock other)
    {
        if (!CanMergeWith(other))
        {
            throw new System.ArgumentException("gave a bad cargo block to merge");
        }

        Merging = true;

        FishCount *= 2;
        Destroy(other.gameObject, PositionTransition.Time);
    }
}
