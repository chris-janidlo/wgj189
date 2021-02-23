using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using crass;

public class CargoHider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float MouseOverAlpha;
    public TransitionableFloat AlphaTransition;
    public CanvasGroup CanvasGroup;

    void Start ()
    {
        AlphaTransition.AttachMonoBehaviour(this);
    }

    void Update ()
    {
        CanvasGroup.alpha = AlphaTransition.Value;
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        AlphaTransition.StartTransitionTo(MouseOverAlpha);
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        AlphaTransition.StartTransitionTo(1);
    }
}
