using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class OverviewCameraToggleAnimation : MonoBehaviour
{
    public TransitionableFloat Transition;
    public Camera OverviewCamera;

    void Start ()
    {
        Transition.AttachMonoBehaviour(this);
    }

    void Update ()
    {
        OverviewCamera.rect = new Rect
        (
            1 - Transition.Value,
            0,
            Transition.Value,
            1
        );
    }

    public void OnOverviewScreenActiveChanged (bool value)
    {
        if (value)
        {
            Transition.StartTransitionTo(1, .4f, EasingFunction.Ease.EaseOutBounce);
        }
        else
        {
            Transition.StartTransitionTo(0, .3f, EasingFunction.Ease.EaseInOutQuint);
        }
    }
}
