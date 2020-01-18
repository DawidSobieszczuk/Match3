using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceGameTimePanel : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenuPanel = null;

    public void One()
    {
        StaticClass.GameTime = 60;
        SceneManager.LoadScene("GameScene");
    }

    public void Two()
    {
        StaticClass.GameTime = 60*2;
        SceneManager.LoadScene("GameScene");
    }

    public void Five()
    {
        StaticClass.GameTime = 60*5;
        SceneManager.LoadScene("GameScene");
    }

    public void Ten()
    {
        StaticClass.GameTime = 60*10;
        SceneManager.LoadScene("GameScene");
    }

    public void Back()
    {
        gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
