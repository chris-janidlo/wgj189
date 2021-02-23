using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class OverallGoal : Singleton<OverallGoal>
{
    public FishBag Goal, Progress;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }
}
