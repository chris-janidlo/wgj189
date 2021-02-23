using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;
using TMPro;

public class CargoBlock : MonoBehaviour
{
    [System.Serializable]
    public class IntColorPair
    {
        public int Count;
        public Color Color;
    }

    public bool Merging { get; private set; }

    public FishType FishType;
    public int FishCount = 1;

    public List<IntColorPair> ColorByFishCount;

    public TransitionableFloat ScaleTransition;
    public TransitionableVector2 PositionTransition;

    public Image FishImage, BackgroundImage;
    public TextMeshProUGUI CountText;

    public FishSpriteDisambiguator FishSpriteDisambiguator;

    void Start ()
    {
        DeathLoopManager.Instance.OnPlayerDied.AddListener(() => Destroy(gameObject));
    }

    void Update ()
    {
        transform.localScale = Vector3.one * ScaleTransition.Value;
        transform.position = PositionTransition.Value;

        if (!PositionTransition.Transitioning && Merging)
        {
            CountText.text = FishCount.ToString();
            Merging = false;

            BackgroundImage.color = ColorByFishCount
                .Where(icp => icp.Count <= FishCount)
                .OrderByDescending(icp => icp.Count)
                .First()
                .Color;
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
