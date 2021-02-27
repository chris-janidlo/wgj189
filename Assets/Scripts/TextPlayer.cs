using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using crass;

public class TextPlayer : MonoBehaviour
{
    [TextArea]
    public List<string> TextEntries;
    public float PromptDelay;
    public TransitionableFloat PromptFadeIn;

    public UnityEvent OnFinished;

    public TextSheaf TextSheafPrefab;
    public Transform TextSheafParent, TextSheafThrowTarget;
    public TextMeshProUGUI PromptText;

    void Start ()
    {
        PromptFadeIn.AttachMonoBehaviour(this);
        PromptText.alpha = 0;
    }

    public void Play ()
    {
        StartCoroutine(playRoutine());
    }

    IEnumerator playRoutine ()
    {
        for (int i = 0; i < TextEntries.Count; i++)
        {
            string entry = TextEntries[i];
            bool last = i == TextEntries.Count - 1;

            var sheaf = Instantiate(TextSheafPrefab, TextSheafParent);
            sheaf.Initialize(entry);
            sheaf.ThrowAt(TextSheafThrowTarget);

            yield return new WaitWhile(() => sheaf.Throwing);
            yield return new WaitForSeconds(PromptDelay);

            if (last) PromptText.text = "Continue...";

            PromptFadeIn.FlashFromTo(0, 1);

            while (PromptFadeIn.Transitioning)
            {
                PromptText.alpha = PromptFadeIn.Value;
                yield return null;
            }

            PromptText.alpha = 1;

            yield return new WaitUntil(() => Input.anyKey);

            if (!last) PromptText.alpha = 0;
        }

        OnFinished.Invoke();
    }
}
