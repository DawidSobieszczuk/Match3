using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel = null;
    public GameObject choiceGameTimePanel = null;
    public Button leaderboardButton = null;

    void Update()
    {
        leaderboardButton.interactable = PlayServicesManager.Instance.IsSignIn;
    }

    public void Play()
    {
        gameObject.SetActive(false);
        choiceGameTimePanel.SetActive(true);
    }

    public void Settings()
    {
        gameObject.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void Leaderboard()
    {
        PlayServicesManager.Instance.ShowLeaderboardUI();
    }
}
