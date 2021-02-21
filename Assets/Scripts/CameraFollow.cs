using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 OffsetFromTarget;
    public float FollowDelay;

    Vector3 velocity;

    void Update ()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target.position + OffsetFromTarget, ref velocity, FollowDelay);
    }
}
