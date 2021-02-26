using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class NavigatorCursor : MonoBehaviour
{
    public Vector2 Position => transform.position;

    public Rigidbody2D Rigidbody;
    public Camera OverviewCamera;

    public BoolVariable OverviewScreenActive;
    public OverviewCameraToggleAnimation OverviewCameraToggleAnimation;

    void Update ()
    {
        if (!OverviewScreenActive.Value || OverviewCameraToggleAnimation.Transition.Transitioning) return;

        Vector2 currentMousePosition = OverviewCamera.ScreenToWorldPoint(Input.mousePosition);
        Rigidbody.MovePosition(currentMousePosition);
    }
}
