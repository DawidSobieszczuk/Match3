using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopPanel : MonoBehaviour
{
    public void PauseButton()
    {
        GameManager.Instance.IsPaused = true;
    }
}
