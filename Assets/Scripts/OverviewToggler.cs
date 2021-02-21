using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class OverviewToggler : MonoBehaviour
{
    public BoolVariable OverviewScreenActive;

    void Update ()
    {
        if (Input.GetButtonDown("Toggle Overview Screen"))
        {
            OverviewScreenActive.Value = !OverviewScreenActive.Value;
        }
    }
}
