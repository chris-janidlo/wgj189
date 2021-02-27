using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public string MainMenuSceneName;
    public TextMeshProUGUI DeathCountText;

    void Start ()
    {
        int count = DeathLoopManager.Instance.DeathCount;
        DeathCountText.text = $"You freed Thiefbeard from his curse in <u>{count}</u> death{(count == 1 ? "" : "s")}.";
    }

    public void LoadMainMenu ()
    {
        Destroy(DeathLoopManager.Instance.gameObject);
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
