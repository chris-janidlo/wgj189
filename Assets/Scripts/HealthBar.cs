using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;

public class HealthBar : MonoBehaviour
{
    public TransitionableFloat JuiceAnimation;
    public Image Bar;

    void Start ()
    {
        JuiceAnimation.AttachMonoBehaviour(this);
    }

    void Update ()
    {
        float percent = Player.Instance.CurrentHealth / Player.Instance.MaxHealth;

        if (Bar.fillAmount != percent && !JuiceAnimation.Transitioning)
        {
            JuiceAnimation.StartTransitionTo(percent);
        }

        Bar.fillAmount = JuiceAnimation.Value;
    }
}
