using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using crass;

public class TextSheaf : MonoBehaviour
{
    public bool Throwing { get; private set; }

    public float MaxDistanceFromTarget, LandTravel;
    public Vector3 InitialRelativePosition;
    public Vector2 AngleRange;

    public TransitionableVector3 ThrowTransition, LandTransition;
    public TextMeshProUGUI Text;

    void Awake ()
    {
        ThrowTransition.AttachMonoBehaviour(this);
        LandTransition.AttachMonoBehaviour(this);
    }

    public void Initialize (string text)
    {
        Text.text = text;
    }

    public void ThrowAt (Transform target)
    {
        StartCoroutine(throwAtTarget(target));
    }

    IEnumerator throwAtTarget (Transform target)
    {
        transform.position = target.transform.position + InitialRelativePosition;
        transform.rotation = Quaternion.Euler(0, 0, RandomExtra.Range(AngleRange));

        var landTarget = target.transform.position + (Vector3) (Random.insideUnitCircle * MaxDistanceFromTarget);
        var throwDir = (Vector2) landTarget - (Vector2) transform.position;
        var throwTarget = landTarget - (Vector3) throwDir.normalized * Random.Range(0, LandTravel);

        ThrowTransition.FlashFromTo(transform.position, throwTarget);

        while (ThrowTransition.Transitioning)
        {
            transform.position = ThrowTransition.Value;
            yield return null;
        }

        transform.position = throwTarget;

        LandTransition.FlashFromTo(transform.position, landTarget);

        while (LandTransition.Transitioning)
        {
            transform.position = LandTransition.Value;
            yield return null;
        }

        transform.position = landTarget;
    }
}
