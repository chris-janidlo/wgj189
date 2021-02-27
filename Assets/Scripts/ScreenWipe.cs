using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using crass;

public class ScreenWipe : MonoBehaviour
{
    public TransitionableFloat WipeTransition;
    public RectTransform RectToWipe;

    public UnityEvent OnWiped;

    void Start ()
    {
        WipeTransition.AttachMonoBehaviour(this);
    }

    public void Wipe ()
    {
        StartCoroutine(anim());
    }

    IEnumerator anim ()
    {
        WipeTransition.FlashFromTo(0, -1024);

        while (WipeTransition.Transitioning)
        {
            setX(RectToWipe, WipeTransition.Value);
            yield return null;
        }

        setX(RectToWipe, -1024);

        OnWiped.Invoke();
    }

    void setX (RectTransform rectTransform, float value)
    {
        var rect = rectTransform.anchoredPosition;
        rect.x = value;
        rectTransform.anchoredPosition = rect;
    }
}
