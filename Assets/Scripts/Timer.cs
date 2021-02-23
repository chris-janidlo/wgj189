using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using TMPro;

public class Timer : MonoBehaviour
{
    public float CurrentTime { get; private set; }

    public float MaxTime;

    public TextMeshProUGUI Text;
    public BoolVariable DeathScreenActive;

    void Start ()
    {
        OnPlayerRespawn();
    }

    void Update ()
    {
        if (DeathScreenActive.Value) return;

        CurrentTime -= Time.deltaTime;

        Text.text = TimeSpan.FromSeconds(Mathf.Round(CurrentTime)).ToString(@"m\:ss");

        if (CurrentTime <= 0)
        {
            DeathLoopManager.Instance.EnterScreen();
        }
    }

    public void OnPlayerRespawn ()
    {
        CurrentTime = MaxTime;
    }
}
