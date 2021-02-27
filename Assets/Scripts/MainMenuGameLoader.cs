using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuGameLoader : MonoBehaviour
{
    public string MainSceneName;
    public Button Button;

    void Start ()
    {
        Button.onClick.AddListener(() => Button.interactable = false);
    }

    public void LoadMainScene ()
    {
        SceneManager.LoadScene(MainSceneName);
    }
}
