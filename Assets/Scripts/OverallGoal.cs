using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class OverallGoal : Singleton<OverallGoal>
{
    public bool Fulfilled => Progress.Equals(Goal);

    public FishBag Goal, Progress;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }
}
