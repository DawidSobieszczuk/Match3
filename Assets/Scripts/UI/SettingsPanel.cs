using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenuPanel = null;
    [SerializeField]
    Text sfxText = null;

    void Start()
    {
        sfxText.text = "SFX - " + (PlayerPrefs.GetInt("SFX", 1) != 0 ? "ON" : "OFF");
    }

    public void ToggleSFX()
    {
        bool sfx = PlayerPrefs.GetInt("SFX", 1) != 0;
        sfx = !sfx;

        PlayerPrefs.SetInt("SFX", sfx ? 1 : 0);
        sfxText.text = "SFX - " + (sfx ? "ON" : "OFF");
    }

    public void Back()
    {
        gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
