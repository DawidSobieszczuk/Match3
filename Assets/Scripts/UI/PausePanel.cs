using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public void ResumeButton()
    {
        GameManager.Instance.IsPaused = false;
    }

    public void MenuBatton()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
