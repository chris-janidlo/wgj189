using UnityEngine;

public static class AnimationCurveExtensions
{
    public static int GetNumberOfItemsToSpawn (this AnimationCurve probabilityCurve)
    {
        return (int) probabilityCurve.Evaluate(Random.Range(0f, 1f));
    }
}
