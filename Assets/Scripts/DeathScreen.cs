using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using crass;

public class DeathScreen : MonoBehaviour
{
    public float ContinueDelay;
    public string VictorySceneName;

    public TextMeshProUGUI LastDaySummary, OverallSummary, ContinuePrompt;
    public TransitionableFloat ContinuePromptAlphaTransition;

    void Start ()
    {
        ContinuePromptAlphaTransition.AttachMonoBehaviour(this);
    }

    public void OnPlayerDied ()
    {
        ContinuePrompt.alpha = 0;

        var haul = Cargo.Instance.FishInCargoHold;

        LastDaySummary.text = "";

        if (haul.Firefishes > 0) LastDaySummary.text += $"Firefishes - {haul.Firefishes}\n";
        if (haul.Icefishes > 0) LastDaySummary.text += $"Icefishes - {haul.Icefishes}\n";
        if (haul.Poisonfishes > 0) LastDaySummary.text += $"Poisonfishes - {haul.Poisonfishes}\n";
        if (haul.Spikefishes > 0) LastDaySummary.text += $"Spikefishes - {haul.Spikefishes}\n";
        if (haul.Bonefishes > 0) LastDaySummary.text += $"Bonefishes - {haul.Bonefishes}\n";
        if (haul.Goldenfishes > 0) LastDaySummary.text += $"Goldenfishes - {haul.Goldenfishes}\n";

        var goal = OverallGoal.Instance.Goal;
        var progress = OverallGoal.Instance.Progress;

        OverallSummary.text =
$@"Firefishes - {progress.Firefishes}/{goal.Firefishes}
Icefishes - {progress.Icefishes}/{goal.Icefishes}
Poisonfishes - {progress.Poisonfishes}/{goal.Poisonfishes}
Spikefishes - {progress.Spikefishes}/{goal.Spikefishes}
Bonefishes - {progress.Bonefishes}/{goal.Bonefishes}
Goldenfishes - {progress.Goldenfishes}/{goal.Goldenfishes}";

        StartCoroutine(screenRoutine());
    }

    IEnumerator screenRoutine ()
    {
        yield return new WaitForSeconds(ContinueDelay);

        ContinuePromptAlphaTransition.FlashFromTo(0, 1);

        while (ContinuePromptAlphaTransition.Transitioning)
        {
            ContinuePrompt.alpha = ContinuePromptAlphaTransition.Value;
            yield return null;
        }

        while (true)
        {
            if (Input.anyKeyDown)
            {
                if (OverallGoal.Instance.Fulfilled)
                {
                    SceneManager.LoadScene(VictorySceneName);
                }
                else
                {
                    DeathLoopManager.Instance.ExitScreen();
                }
            }

            yield return null;
        }
    }
}
