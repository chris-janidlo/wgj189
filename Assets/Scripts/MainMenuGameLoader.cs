using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using crass;

public class MainMenuGameLoader : MonoBehaviour
{
    public TransitionableFloat ScaleTransition;

    public string MainSceneName;
    public RectTransform TitleScreenContainer;
    public Button Button;

    void Start ()
    {
        ScaleTransition.AttachMonoBehaviour(this);
        Button.onClick.AddListener(() => Button.interactable = false);
    }

    public void LoadMainScene ()
    {
        StartCoroutine(anim());
    }

    IEnumerator anim ()
    {
        ScaleTransition.FlashFromTo(0, -1024);

        while (ScaleTransition.Transitioning)
        {
            var rect = TitleScreenContainer.anchoredPosition;
            rect.x = ScaleTransition.Value;
            TitleScreenContainer.anchoredPosition = rect;

            yield return null;
        }

        TitleScreenContainer.localScale = new Vector3(0, 1, 1);
        SceneManager.LoadScene(MainSceneName);
    }
}
