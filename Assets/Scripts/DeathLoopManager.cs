using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityAtoms.BaseAtoms;
using crass;

public class DeathLoopManager : Singleton<DeathLoopManager>
{
    public UnityEvent OnPlayerDied, OnPlayerRespawn;

    public RectTransform DeathScreenParent;
    public BoolVariable DeathScreenActive, OverviewScreenActive;

    void Awake ()
    {
        SingletonOverwriteInstance(this);
    }

    void Start ()
    {
        DeathScreenParent.gameObject.SetActive(false);
    }

    public void EnterScreen ()
    {
        OverviewScreenActive.Value = false;

        DeathScreenActive.Value = true;
        DeathScreenParent.gameObject.SetActive(true);

        OnPlayerDied.Invoke();
    }

    public void ExitScreen ()
    {
        DeathScreenActive.Value = false;
        DeathScreenParent.gameObject.SetActive(false);

        OnPlayerRespawn.Invoke();
    }
}
